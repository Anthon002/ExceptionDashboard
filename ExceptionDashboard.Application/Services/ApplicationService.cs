using ExceptionDashboard.Application.Services.IRepositories;
using ExceptionDashboard.Core.Models;
using ExceptionDashboard.Core.Models.DTOs;
using Microsoft.AspNetCore.Identity;


namespace ExceptionDashboard.Application.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _Iapplicationrepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public ApplicationService(IApplicationRepository Iapplicationrepository, UserManager<ApplicationUser> userManager)
        {
            _Iapplicationrepository = Iapplicationrepository;
            _userManager = userManager;
        }

        public ApplicationDTO SaveApplicationToDb(ApplicationDTO applicationdto)
        {
            var user = _userManager.FindByIdAsync(applicationdto.UserId).Result;
            if (user == null) { return (null); }
            if (applicationdto != null)
            {
                var newApplication = new Applications()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = applicationdto.Name,
                    Code = applicationdto.Code,
                    UserId = applicationdto.UserId,
                    UserName = user.UserName
                };
                _Iapplicationrepository.SaveApplicationToDb(newApplication);
            }
            return (applicationdto);
            
        }

        public IEnumerable<ApplicationDTO> ViewAllApplications()
        {
            return _Iapplicationrepository.ViewAllApplications();
           
        }
        public ApplicationDTO DeleteApplication(string key) 
        {
            return _Iapplicationrepository.DeleteApplication(key);
        }
    }
}
