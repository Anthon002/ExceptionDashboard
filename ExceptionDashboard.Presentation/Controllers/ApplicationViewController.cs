using ExceptionDashboard.Application.Services;
using ExceptionDashboard.Core.Models;
using ExceptionDashboard.Core.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ExceptionDashboard.WebApi.Controllers
{
    public class ApplicationViewController : Controller
    {
        private readonly IApplicationService _IapplicationService;
        private readonly IUserAuthenticationService _userAuthenticationService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationViewController(IApplicationService IapplicationService, IUserAuthenticationService userAuthenticationService, UserManager<ApplicationUser> userManager)
        {
            _IapplicationService = IapplicationService;
            _userAuthenticationService = userAuthenticationService;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        [RequireConfirmedEmail]
        public async Task<ActionResult<IEnumerable<ApplicationDTO>>> ViewApplications()
        {
            ViewData["UserId"] = _userManager.GetUserId(this.User);
            var userid = ViewData["UserId"];
            return View(_IapplicationService.ViewAllApplications());
        }
        [HttpGet]
        [RequireConfirmedEmail]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApplicationDTO>> AddApplication()
        {
            var users =  _userAuthenticationService.GetAllUsers();
            ViewData["UserList"] = users.ToList();
            
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApplicationDTO>> AddApplication(ApplicationDTO newApplication)
        {
            if (newApplication == null) { return NoContent(); }
            _IapplicationService.SaveApplicationToDb(newApplication);
             return RedirectToAction(nameof(ViewApplications));
        }

    }
    
}
