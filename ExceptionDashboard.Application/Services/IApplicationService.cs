using ExceptionDashboard.Core.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionDashboard.Application.Services
{
    public interface IApplicationService
    {
        ApplicationDTO SaveApplicationToDb(ApplicationDTO application);
        IEnumerable<ApplicationDTO> ViewAllApplications();
        ApplicationDTO DeleteApplication(string key);
    }
}