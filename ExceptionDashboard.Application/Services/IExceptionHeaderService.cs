using ExceptionDashboard.Core.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionDashboard.Application.Services
{
    public interface IExceptionHeaderService
    {
        ExceptionHeaderDTO SaveExceptionHeaderToDB(ExceptionHeaderDTO exceptionheaderdto);
        IEnumerable<ExceptionHeaderDTO> ViewExceptionHeader();
        ExceptionHeaderDTO DeleteException(string key);
    }
}
