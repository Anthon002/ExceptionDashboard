using ExceptionDashboard.Core.Models;
using ExceptionDashboard.Core.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionDashboard.Application.Services
{
    public interface IExceptionService
    {
        IEnumerable<ExceptionDTO> ViewAllExceptions(int pageNumber, int pageSize);
        ExceptionDTO SaveExceptionToDb(ExceptionDTO exceptionrequest);
        ExceptionDTO DeleteException(string key);
        ExceptionDTO UpdateStatus(string key, int status);
        IEnumerable<ExceptionDTO> ViewSpecificExceptions(string Id, int pageNumber, int pageSize);
        Task<ExceptionDTO> SendExceptionMail(ApplicationUser user);
        ExceptionDTO UpdateException(string id);
        ExceptionDTO ViewExceptionById(string id);
    }
}
