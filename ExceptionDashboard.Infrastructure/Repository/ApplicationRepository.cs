using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExceptionDashboard.Application.Services.IRepositories;
using ExceptionDashboard.Core.Models;
using ExceptionDashboard.Core.Models.DTOs;
using ExceptionDashboard.Infrastructure.Data;

/**
 * This file is responsible for getting data from api controller and saving the data to database
 * contains the dbcontext injection
 */
namespace ExceptionDashboard.Infrastructure.Repository
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ApplicationRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            
        }

        public ApplicationDTO SaveApplicationToDb(Applications newapplication)
        {
            if (newapplication != null)
            {
                _dbContext.ApplicationDb.Add(newapplication);
                _dbContext.SaveChanges();
                
            }
            return (null);
            
        }

        public IEnumerable<ApplicationDTO> ViewAllApplications()
        {
            var application = _dbContext.ApplicationDb.Select(item => new ApplicationDTO{Id = item.Id, Name = item.Name, Code = item.Code, UserId = item.UserId,UserName = item.UserName}).ToList();
            return (application);
        }
        

        public ApplicationDTO DeleteApplication(string key)
        {

            var application = _dbContext.ApplicationDb.FirstOrDefault(x => x.Name == key);
            if (application != null)
            {
                var applicationDTO = new ApplicationDTO() { Name = application.Name, Code = application.Code, UserId = application.UserId };
                if (application != null)
                {
                    _dbContext.Remove(application);
                    _dbContext.SaveChanges();
                    return (applicationDTO);
                }
                else
                {
                    return (null);
                }
            }
            else
            {
                return (null);
            }
        }
    }
}
