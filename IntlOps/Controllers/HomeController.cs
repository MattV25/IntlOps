using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using IntlOps.Data;
using Microsoft.AspNetCore.Identity;
using IntlOps.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using static IntlOps.Models.AccountViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System;

namespace IntlOps.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly RoleManager<ApplicationRole> _roleManager;

        //Initialize Database Instance
        private ApplicationDbContext db;

        public HomeController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger,
            RoleManager<ApplicationRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _roleManager = roleManager;
            db = context;
        }

        [TempData]
        public string ErrorMessage { get; set; }

        //Core Home Pages
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        
        //Login
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    ApplicationUser user = await _userManager.FindByEmailAsync(model.Email);
                    var roles = await _userManager.GetRolesAsync(user);
                    if (String.IsNullOrEmpty(returnUrl))
                    {
                        _logger.LogInformation("User logged in.");
                        if (roles.Contains("Admin"))
                        {
                            return RedirectToAction("AdminHome", "Admin");
                        }
                        else
                        {
                            return RedirectToAction("AccountHomePage", "Account");
                        }
                    }
                    else
                     {
                         return RedirectToLocal(returnUrl);
                     }
                }
                if (result.RequiresTwoFactor)
                {
                    //return RedirectToAction(nameof(LoginWith2fa), new { returnUrl, model.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToAction(nameof(Lockout));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }
            return View(model);
        }

        //Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    UserName = model.Email,
                    Birthdate = model.Birthdate,
                    Gender = model.Gender,
                    JobTitle = model.JobTitle,
                    JobType = model.JobType,
                    MaritalStatus = model.MaritalStatus,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    AccountName = model.AccountName,
                    Street1 = model.Street1,
                    Street2 = model.Street2,
                    City = model.City,
                    State = model.State,
                    Zipcode = model.Zipcode
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.EmailConfirmationLink(user.Id.ToString(), code, Request.Scheme);
                    await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);
                    return RedirectToLocal(returnUrl);
                }
                AddErrors(result);
            }
            return View(model);
        }

        //Logout/Lockout/Denial
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(HomeController.Login), "Home");
        }
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        //Load State list to Register Page
        public static List<SelectListItem> GetStates()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<SelectListItem> states = new List<SelectListItem>();
            var stateList = (from stateName in db.States select stateName);
            foreach (var temp in stateList)
            {
                states.Add(new SelectListItem() { Text = temp.StateName, Value = temp.StateName });
            }
            return states;
        }
        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}
