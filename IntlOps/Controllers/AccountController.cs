using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IntlOps.Services;
using IntlOps.Data;
using static IntlOps.Models.AccountViewModel;

namespace IntlOps.Controllers
{
    [Authorize (Roles = "User")]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly RoleManager<ApplicationRole> _roleManager;

        //Initialize Database Instance
        private ApplicationDbContext db;

        public AccountController(
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

        //Account Home Page
        [HttpGet]
        public IActionResult AccountHomePage(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //View Account Info
        [HttpGet]
        public IActionResult ViewAccountInfo(int? id)
        {
            var userid = _userManager.GetUserId(HttpContext.User);
            var user = _userManager.FindByIdAsync(userid).Result;
            return View(user);
        }

        //Apply for Credit
        [HttpGet]
        public IActionResult CreditApply(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreditApply(ApplicationsViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                var user = new Applications
                {
                    ClientId = int.Parse(userId),
                    ApplicationDate = model.ApplicationDate,
                    Income = model.Income,
                    CreditRequested = model.CreditRequested
                };
                db.Application.Add(user);
                await db.SaveChangesAsync();
            }
            return View(model);
        }

        //Confirm Email
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }
    }
}
