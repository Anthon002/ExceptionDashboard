using ExceptionDashboard.Application.Services;
using ExceptionDashboard.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;

/*
 * This attribute restrict users that have not confirmed their email from accessing the pages
 */
public class RequireConfirmedEmailAttribute : TypeFilterAttribute
{
    public RequireConfirmedEmailAttribute() : base(typeof(RequireConfirmedEmailFilter))
    {
    }
}

public class RequireConfirmedEmailFilter : IAsyncAuthorizationFilter
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserAuthenticationService _IUserauthenticationService;

    public RequireConfirmedEmailFilter(UserManager<ApplicationUser> userManager, IUserAuthenticationService IUserauthenticationService)
    {
        _userManager = userManager;
        _IUserauthenticationService = IUserauthenticationService;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        // Get the current user
        var user = await _userManager.GetUserAsync(context.HttpContext.User);

        // Check if the user is signed in and their email is confirmed
        if (user != null && !await _userManager.IsEmailConfirmedAsync(user))
        {
            context.Result = new RedirectToActionResult("EmailNotConfirmed", "Account", null);
        }
    }
}
