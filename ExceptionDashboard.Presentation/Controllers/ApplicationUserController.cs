using ExceptionDashboard.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ExceptionDashboard.Core.Models.DTOs;
using Microsoft.Identity.Client;
using ExceptionDashboard.Application.Services;
using System.Net.Mail;
using FluentEmail.Smtp;
using FluentEmail.Core;
namespace ExceptionDashboard.WebApi.Controllers
{
    public class ApplicationUserController : Controller
    {
        private readonly IUserAuthenticationService _IuserAuthenticationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ApplicationUserController(IUserAuthenticationService IuserAuthenticationService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _IuserAuthenticationService = IuserAuthenticationService;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        [HttpGet]
        public async Task<ActionResult> SignUp()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("ViewApplications", "ApplicationView");
            }
            await _signInManager.SignOutAsync();
            return View();
        }
        [HttpPost]
        public async Task<ActionResult<ApplicationUserDTO>> SignUp(ApplicationUserDTO newUser)
        {
            var user = _IuserAuthenticationService.SignUp(newUser,"User");
            string password = Guid.NewGuid().ToString();
            try
            {
                //var roleResult = await _userManager.AddToRoleAsync(user,"User");
                //user.RoleId = _roleManager.FindByNameAsync("User").Id.ToString();
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded) {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    ViewData["UserId"] = _userManager.GetUserId(this.User);
                }
                

            }
            catch (Exception ex) { }

            SendEmail(user.Id); //Function to send the confirmation key for email confirmation

            return RedirectToAction(nameof(ConfirmEmail));

        }
        [HttpGet]
        public async Task<ActionResult<bool>> SendEmail(string id)
        {
              _IuserAuthenticationService.SendEmail(id);
            return RedirectToAction("ConfirmEmail");
        }
        [HttpGet]
        public async Task<ActionResult<int>> ConfirmEmail()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult<int>> ConfirmEmail(string confirmationotp)
        {
            ViewData["UserId"] = _userManager.GetUserId(this.User);
            try
            {
                 await _userManager.FindByIdAsync(ViewData["UserId"].ToString());
            }
            catch(Exception ex)
            {
                return RedirectToAction("Login");
            }
            var user = await _userManager.FindByIdAsync(ViewData["UserId"].ToString());
            if (user == null) { return (null); }
            if (int.Parse(confirmationotp ) == user.OTP)
            {
                Random random = new Random();
                user.EmailConfirmed = true;
                user.OTP = random.Next(1000, 9999);
                await _userManager.UpdateAsync(user);
                await _signInManager.SignInAsync(user, isPersistent: true);
                return RedirectToAction("ViewApplications", "ApplicationView");
            }
            else
            {
                await _signInManager.SignOutAsync();
                return BadRequest("Incorrect Confirmation Key");
            }

        }
        [HttpGet]
        public async Task<ActionResult> Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("ViewApplications", "ApplicationView");
            }
            await _signInManager.SignOutAsync();
            return View();
        }
        [HttpPost]
        public async Task<ActionResult<ApplicationUser>> Login(string Email)
        {
            if (Email == null) { return (null); }
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null) { return (null);}
            user.EmailConfirmed = false;
            await _userManager.UpdateAsync(user);
            await _signInManager.SignInAsync(user, isPersistent: true);
            _IuserAuthenticationService.Login(user);
            return RedirectToAction(nameof(ConfirmEmail));
        }
        [HttpGet]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("Login");
        }

    }
}
