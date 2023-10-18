using ExceptionDashboard.Core.Models;
using ExceptionDashboard.Core.Models.DTOs;

namespace ExceptionDashboard.Application.Services
{
    public interface IUserAuthenticationService
    {
        ApplicationUser SignUp(ApplicationUserDTO userdto, string role);
        Task<bool> SendEmail(string id);
        Task<ApplicationUser> Login(ApplicationUser user);
        List<ApplicationUserDTO> GetAllUsers();
    }
}
