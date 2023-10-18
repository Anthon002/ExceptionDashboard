using ExceptionDashboard.Application.Services;
using ExceptionDashboard.Core.Models;
using ExceptionDashboard.Core.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExceptionDashboard.WebApi.Areas.Admin.Controllers
{
    public class ApplicationAdminUser : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserAuthenticationService _IuserAuthenticationService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ApplicationAdminUser(UserManager<ApplicationUser> userManager, IUserAuthenticationService IuserAuthenticationService, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _IuserAuthenticationService = IuserAuthenticationService;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        [HttpPost]
        public async Task<ActionResult> AutoCreateRoles()
        {
            /* 
             * The Admin and User Authorization Roles are created here if not already created
             */
            var adminExists = _roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult();
            var userExists = _roleManager.RoleExistsAsync("User").GetAwaiter().GetResult();
            if (!adminExists)
            {
                var adminRole = new IdentityRole() { Name = "Admin" };
                _roleManager.CreateAsync(adminRole).GetAwaiter().GetResult();
            }
            if(!userExists)
            {
                var userRole = new IdentityRole()
                {
                    Name = "User"
                };
                _roleManager.CreateAsync(userRole).GetAwaiter().GetResult();
            }
            return (null);

        }
        [HttpGet]
        public async Task<ActionResult> SignUp()
        {
            return View("SignUp");
        }
        [HttpPost]
        public async Task<ActionResult> SignUp(ApplicationUserDTO newuser)
        {
            //url https://localhost:44323/ApplicationAdminUser/SignUp
            /*
             * Admin registration action
             */
            AutoCreateRoles();
            var user = _IuserAuthenticationService.SignUp(newuser, "Admin");
            string password = Guid.NewGuid().ToString();
            try
            {
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    ViewData["UserId"] = _userManager.GetUserId(this.User);
                }


            }
            catch (Exception ex) { }

            _IuserAuthenticationService.SendEmail(user.Id);
            return RedirectToAction("ConfirmEmail","ApplicationUser",new {area = ""});
        }
    }
}
