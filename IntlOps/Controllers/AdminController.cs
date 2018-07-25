using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using IntlOps.Code;
using IntlOps.Data;
using Microsoft.AspNetCore.Identity;
using static IntlOps.Models.AccountViewModel;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace IntlOps.Controllers
{
    [Authorize (Roles = "Admin")]
    [Route("[controller]/[action]")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private ApplicationDbContext _context;

        public AdminController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            ApplicationDbContext context
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        //Admin Home Page
        [HttpGet]
        public IActionResult AdminHome(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //---------------------------------------------------Start CRUD Operations for Users Section------------------------------------------------//

        //Index
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var users = from s in _context.ApplicationUser
                        select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.Lastname.Contains(searchString) || s.Firstname.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    users = users.OrderByDescending(s => s.Lastname);
                    break;
                default:
                    users = users.OrderBy(s => s.Email);
                    break;
            }
            int pageSize = 3;
            return View(await PaginatedList<ApplicationUser>.CreateAsync(users.AsNoTracking(), page ?? 1, pageSize));
        }

        //Details
        public async Task<IActionResult> Details(int? id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);
            ApplicationUser model = new ApplicationUser
            {
                Id = user.Id,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                UserName = user.Email,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                AccountName = user.AccountName,
                Street1 = user.Street1,
                Street2 = user.Street2,
                City = user.City,
                State = user.State,
                Zipcode = user.Zipcode

            };
            return PartialView("_Details", model);
        }
      
        //Create
        public IActionResult Create()
        {
            return PartialView("_Create");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> CreateUser(RegisterViewModel model)
        {
            var user = new ApplicationUser
            {
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                UserName = model.Email,
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
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        //Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var aspNetUsers = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
            if (id == null || aspNetUsers == null)
            {
                return NotFound();
            }          
            return PartialView("_Edit",aspNetUsers);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> EditUser(int? id, ApplicationUser aspNetUsers)
        {
            var user = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);
            user.UserName = aspNetUsers.UserName;
            user.Firstname = aspNetUsers.Firstname;
            user.Lastname = aspNetUsers.Lastname;
            user.PhoneNumber = aspNetUsers.PhoneNumber;
            user.Email = aspNetUsers.Email;
            user.AccountName = aspNetUsers.AccountName;
            user.Street1 = aspNetUsers.Street1;
            user.Street2 = aspNetUsers.Street2;
            user.City = aspNetUsers.City;
            user.State = aspNetUsers.State;
            user.Zipcode = aspNetUsers.Zipcode;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }

        //Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);
            ApplicationUser model = new ApplicationUser
            {
                Firstname = user.Firstname,
                Lastname = user.Lastname
            };
            return PartialView("_Delete",model);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var result = await _userManager.DeleteAsync(user);
            return RedirectToAction("Index");
        }

        //-------------------------------------------------------------End CRUD Operations Section----------------------------------------------------//

        //-------------------------------------------------------------Start Credit Application Section-----------------------------------------------//

        //Display Credit
        public IActionResult DisplayCreditAcc()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<ApproveAccModel> ApproveList = new List<ApproveAccModel>();
            var applist = (from User in db.ApplicationUser
                           join Applications in db.Application on User.Id equals Applications.ClientId
                           select new { User.Id, Applications.ApplicationId, User.Firstname, User.Lastname, Applications.ApplicationStatus, Applications.ApplicationDate, Applications.Income, Applications.CreditRequested }).ToList();
            foreach (var item in applist)
            {
                ApproveAccModel approve = new ApproveAccModel
                {
                    Id = item.Id,
                    ApplicationId = item.ApplicationId,
                    Firstname = item.Firstname,
                    Lastname = item.Lastname,
                    ApplicationStatus = item.ApplicationStatus,
                    ApplicationDate = item.ApplicationDate,
                    Income = item.Income,
                    CreditRequested = item.CreditRequested
                };
                ApproveList.Add(approve);
            }
            return View(ApproveList);
        }

        //Approve Credit
        public async Task<IActionResult> ApproveCredit(int? id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);
            Applications result = (from Applications in _context.Application
                                   where Applications.ClientId == user.Id
                                   select Applications).SingleOrDefault();
            result.ApplicationStatus = "Approved";
            _context.SaveChanges();
            return RedirectToAction("DisplayCreditAcc");
        }

        //Decline Credit
        public async Task<IActionResult> DeclineCredit(int? id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);
            Applications result = (from Applications in _context.Application
                                   where Applications.ClientId == user.Id
                                   select Applications).SingleOrDefault();
            result.ApplicationStatus = "Declined";
            _context.SaveChanges();
            return RedirectToAction("DisplayCreditAcc");
        }

        //-------------------------------------------------------------End Credit Application Section--------------------------------------------------//

        //-------------------------------------------------------------Start Role Management Section---------------------------------------------------//

        //Role Management
        public async Task<IActionResult> RoleControl(int? id)
        {
            List<RolesViewModel> RolesList = new List<RolesViewModel>();
            var userList = _context.Users.ToList();
            var userid = _userManager.GetUserId(HttpContext.User);
            foreach (var item in userList)
            {
                RolesViewModel model = new RolesViewModel()
                {
                    Id = item.Id,
                    Firstname = item.Firstname,
                    Lastname = item.Lastname,
                    RoleNames = await _userManager.GetRolesAsync(_userManager.Users.First(s => s.UserName == item.UserName)),
                    RolesList = _roleManager.Roles.ToList().Select(x => new SelectListItem()
                    {
                        Selected = x.Name.Contains(x.Name),
                        Text = x.Name,
                        Value = x.Name
                    })
                };
                RolesList.Add(model);
            }
            return View(RolesList);
        }

        //Save Roles to User
        public async Task<IActionResult> SaveRoles(int? id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);
            //ApplicationRole roles = (from Roles in _context.Roles where  )
            return RedirectToAction("RoleControl");
        }

        //Role List
        public IActionResult RoleList()
        {
            var roles = _context.Roles.ToList();
            return View(roles);
        }

        //Add Role
        public IActionResult CreateRole(int id)
        {
            return PartialView("~/Views/Admin/RoleOperations/_CreateRole.cshtml");
        }
        [HttpPost, ActionName("CreateRole")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(int id, RolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                var applicationRole = new ApplicationRole
                {
                    Id = model.Id,
                    Name = model.Name
                };
                IdentityResult result = await _roleManager.CreateAsync(applicationRole);
                if (result.Succeeded)
                {
                    await _roleManager.UpdateAsync(applicationRole);
                    return RedirectToAction("RoleControl");
                }
            }
            return RedirectToAction("RoleControl");
        }

        //Edit Role
        [HttpGet]
        public async Task<IActionResult> EditRole(int id)
        {
            RolesViewModel model = new RolesViewModel();
            if (!String.IsNullOrEmpty(id.ToString()))
            {
                ApplicationRole applicationRole = await _roleManager.FindByIdAsync(id.ToString());
                if (applicationRole != null)
                {
                    model.Id = applicationRole.Id;
                    model.Name = applicationRole.Name;
                }
            }
            return PartialView("_EditRole", model);
        }
        [HttpPost, ActionName("EditRole")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(int id, RolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationRole applicationRole = await _roleManager.FindByIdAsync(id.ToString());
                applicationRole.Id = model.Id;
                applicationRole.Name = model.Name;
                IdentityResult result = await _roleManager.UpdateAsync(applicationRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("RoleControl");
                }
            }
            return RedirectToAction("RoleControl");
        }

        //Delete Role
        [HttpGet]
        public async Task<IActionResult> DeleteRole(int? id, RolesViewModel model)
        {
            if (id != null)
            {
                ApplicationRole applicationRole = await _roleManager.FindByIdAsync(id.ToString());
                if (applicationRole != null)
                {
                    model.Name = applicationRole.Name;
                }
            }
            return PartialView("_DeleteRole",model);
        }
        [HttpPost, ActionName("DeleteRole")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRole(int? id)
        {
            if (id != null)
            {
                ApplicationRole applicationRole = await _roleManager.FindByIdAsync(id.ToString());
                if (applicationRole != null)
                {
                    IdentityResult result = await _roleManager.DeleteAsync(applicationRole);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("RoleControl");
                    }
                }
            }
            return RedirectToAction("RoleControl");
        }

        //-------------------------------------------------------------End Role Management Section------------------------------------------------------//

    }
}

