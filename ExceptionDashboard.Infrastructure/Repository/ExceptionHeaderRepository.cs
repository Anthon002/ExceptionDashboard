using ExceptionDashboard.Application.Services.IRepositories;
using ExceptionDashboard.Core.Models;
using ExceptionDashboard.Core.Models.DTOs;
using ExceptionDashboard.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionDashboard.Infrastructure.Repository
{
    public class ExceptionHeaderRepository : IExceptionHeaderRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ExceptionHeaderRepository( ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public ExceptionHeaderDTO SaveExceptionHeaderToDB(ExceptionHeaderDTO exceptionheaderdto)
        {
            if (exceptionheaderdto != null)
            {
                var exceptionheader = new ExceptionHeader()
                {
                    AppId = exceptionheaderdto.AppId,
                    ExceptionCode = exceptionheaderdto.ExceptionCode,
                    Id = Guid.NewGuid().ToString(),
                };
                _dbContext.ExceptionHeaderDb.Add(exceptionheader);
                _dbContext.SaveChanges();
                return (exceptionheaderdto);
            }
            else
            { return (null); };
        }
        public IEnumerable<ExceptionHeaderDTO> ViewExceptionHeaders() 
        {
            var exceptionheaders = _dbContext.ExceptionHeaderDb.Select(x => new ExceptionHeaderDTO {  ExceptionCode = x.ExceptionCode, AppId = x.AppId }).ToList();
            return (exceptionheaders);
        }
        public ExceptionHeaderDTO DeleteExceptionHeader(string key)
        {
            var exceptionHeader = _dbContext.ExceptionHeaderDb.FirstOrDefault(x => x.Id == key);
            if (exceptionHeader != null)
            {
                var exceptionheaderdto = new ExceptionHeaderDTO() {  ExceptionCode = exceptionHeader.ExceptionCode, AppId = exceptionHeader.AppId };
                _dbContext.Remove(exceptionHeader);
                _dbContext.SaveChanges();
                return (exceptionheaderdto );
            }
            else
            { return (null); }
        }


    }
}
