using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IntlOps.Services;
using Microsoft.AspNetCore.Mvc.Razor;
using IntlOps.Data;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;

namespace IntlOps
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //Services
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("intlops")));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddDistributedMemoryCache();

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });

            //Configure Views
            services.Configure<RazorViewEngineOptions>(o =>
            {
                // {2} is area, {1} is controller,{0} is the action    
                o.ViewLocationFormats.Clear();
                o.ViewLocationFormats.Add("/Views/Manage/PasswordFunctions/{0}" + RazorViewEngine.ViewExtension);
                o.ViewLocationFormats.Add("/Views/Manage/ExternalOptions/{0}" + RazorViewEngine.ViewExtension);
                o.ViewLocationFormats.Add("/Views/Account/PasswordFunctions/{0}" + RazorViewEngine.ViewExtension);
                o.ViewLocationFormats.Add("/Views/Account/ExternalAndLogout/{0}" + RazorViewEngine.ViewExtension);
                o.ViewLocationFormats.Add("/Views/Admin/UserOperations/{0}" + RazorViewEngine.ViewExtension);
                o.ViewLocationFormats.Add("/Views/Admin/RoleOperations/{0}" + RazorViewEngine.ViewExtension);
            });

            //Cookie Settings
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.Expiration = TimeSpan.FromDays(150);
                options.LoginPath = "/Account/Login"; 
                options.LogoutPath = "/Account/Logout"; 
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            services.AddSession();
            services.AddMvc();
        }

        //Configuration
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            IServiceProvider serviceProvider, 
            ILoggerFactory loggerFactory, 
            RoleManager<ApplicationRole> roleManager
            )
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseSession();

            var options = new RewriteOptions()
                .AddRedirectToHttps();

            app.UseRewriter(options);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Login}/{id?}");
            });
            RoleInitializer.Initialize(roleManager);
        }
    }   
}
