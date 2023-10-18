using ExceptionDashboard.Application.Services.IRepositories;
using ExceptionDashboard.Core.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionDashboard.Application.Services
{
    public class ExceptionHeaderService : IExceptionHeaderService
    {
        private readonly IExceptionHeaderRepository _Iexceptionheaderepository;
        public ExceptionHeaderService(IExceptionHeaderRepository Iexceptionheaderrepository)
        {
            _Iexceptionheaderepository = Iexceptionheaderrepository;
        }


        public ExceptionHeaderDTO SaveExceptionHeaderToDB(ExceptionHeaderDTO exceptionheaderdto)
        {
            if (exceptionheaderdto == null) { return (null); }
            return(_Iexceptionheaderepository.SaveExceptionHeaderToDB(exceptionheaderdto));
        }

        public IEnumerable<ExceptionHeaderDTO> ViewExceptionHeader()
        {
            return (_Iexceptionheaderepository.ViewExceptionHeaders());
        }

        public ExceptionHeaderDTO DeleteException(string key)
        {
            if (key != null)
            {
                return (_Iexceptionheaderepository.DeleteExceptionHeader(key));
            }
            else { return (null); }
        }

    }
}
