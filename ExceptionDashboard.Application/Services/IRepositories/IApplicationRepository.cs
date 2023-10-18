using ExceptionDashboard.Core.Models.DTOs;
using ExceptionDashboard.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionDashboard.Application.Services.IRepositories
{
    public interface IApplicationRepository
    {
        IEnumerable<ApplicationDTO> ViewAllApplications();
        ApplicationDTO SaveApplicationToDb(Applications application);
        ApplicationDTO DeleteApplication(string key);
    }
}
