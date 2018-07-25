using IntlOps.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace IntlOps.Services
{
    public static class RoleInitializer
    {
        public static void Initialize(RoleManager<ApplicationRole> roleManager)
        {
            Func<Task> func = async () =>
            {
                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    var role = new ApplicationRole("Admin");
                    await roleManager.CreateAsync(role);
                }
                if (!await roleManager.RoleExistsAsync("Manager"))
                {
                    var role = new ApplicationRole("Manager");
                    await roleManager.CreateAsync(role);
                }
                if (!await roleManager.RoleExistsAsync("User"))
                {
                    var role = new ApplicationRole("User");
                    await roleManager.CreateAsync(role);
                }
            };
            Task task = func();
            task.Wait();
        }
    }
}
