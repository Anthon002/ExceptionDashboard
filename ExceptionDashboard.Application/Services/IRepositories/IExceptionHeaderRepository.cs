using ExceptionDashboard.Core.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionDashboard.Application.Services.IRepositories
{
    public interface IExceptionHeaderRepository
    {
        ExceptionHeaderDTO SaveExceptionHeaderToDB(ExceptionHeaderDTO exception);
        IEnumerable<ExceptionHeaderDTO> ViewExceptionHeaders();
        ExceptionHeaderDTO DeleteExceptionHeader(string key);
    }
}
