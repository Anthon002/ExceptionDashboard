using ExceptionDashboard.Application.Services.IRepositories;
using ExceptionDashboard.Core.Models;
using ExceptionDashboard.Core.Models.DTOs;
using ExceptionDashboard.Core.Models.Enums;
using ExceptionDashboard.Infrastructure.Data;
using Microsoft.AspNetCore.Http.Features;

namespace ExceptionDashboard.Infrastructure.Repository
{
    public class ExceptionRepository : IExceptionRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public int pagenumber = 1;

        public ExceptionRepository( ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public ExceptionDTO SaveExceptionToDb(ExceptionDTO exceptionrequest)
        {
            if (exceptionrequest != null)
            {
                var exception = new Exceptions()
                {
                  Id = Guid.NewGuid().ToString(),
                  ExceptionMessage = exceptionrequest.ExceptionMessage,
                  ExceptionHeaderId = _dbContext.ExceptionHeaderDb.FirstOrDefault(x => x.ExceptionCode == exceptionrequest.ExceptionCode).Id, //Checks the list of exceptionheaders and assigns its id to the exception with the same exceptioncode 
                  StackTrace = exceptionrequest.StackTrace,
                  Status = ExceptionStatus.Pending,
                  ExceptionCode = exceptionrequest.ExceptionCode,
                  AppId = exceptionrequest.AppId,
                };
                _dbContext.ExceptionDb.Add(exception);
                _dbContext.SaveChanges();
                return (exceptionrequest);
            }
            else
            {
                return (null);
            }
        }

        public IEnumerable<ExceptionDTO> ViewAllExceptions(int pageNumber, int pageSize)
        {
            var pagedExeptions = _dbContext.ExceptionDb
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .Select(exception => new ExceptionDTO
                                {
                                    Id = exception.Id,
                                    ExceptionCode = exception.ExceptionCode,
                                    StackTrace = exception.StackTrace,
                                    Status = exception.Status,
                                    ExceptionHeaderId = exception.ExceptionHeaderId,
                                    ExceptionMessage = exception.ExceptionMessage,
                                    AppId = exception.AppId,
                                    AppName = _dbContext.ApplicationDb.FirstOrDefault(app => app.Id == exception.AppId).Name,
                                    StatusText = exception.Status.GetDescription()
                                }).ToList();
            return (pagedExeptions);
        }

        public ExceptionDTO DeleteException(string key)
        {
            var exception = _dbContext.ExceptionDb.FirstOrDefault(x => x.Id ==key);
            if (exception == null) { return (null); }
            var exceptiondto = new ExceptionDTO()
            {
                ExceptionHeaderId = exception.ExceptionHeaderId,
                ExceptionMessage = exception.ExceptionMessage,
                StackTrace= exception.StackTrace,
                Status = exception.Status
            };
            if (exception != null)
            {
                _dbContext.Remove(exception);
                _dbContext.SaveChanges();
                return (exceptiondto);
            }
            else
            {
                return (null);
            }
        }

        public ExceptionDTO UpdateStatus(string key, int status)
        {
            var exception = _dbContext.ExceptionDb.FirstOrDefault(x => x.Id == key);
            exception.Status = (ExceptionStatus)status;
            _dbContext.SaveChanges();
            var exceptiondto = new ExceptionDTO()
            {
                ExceptionCode = exception.ExceptionCode,
                ExceptionHeaderId = exception.ExceptionHeaderId,                                 
                ExceptionMessage = exception.ExceptionMessage,
                Status = exception.Status,
                StackTrace = exception.StackTrace,
            };
            return (exceptiondto);
        }
        public IEnumerable<ExceptionDTO> ViewSpecificExceptions(string Id, int pageNumber, int pageSize)
        {
            var pagedexception = _dbContext.ExceptionDb
                                    .Where(x => x.AppId == Id)      // The exceptions are filtered by AppId
                                    .Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .Select(x => new ExceptionDTO
                                    {
                                        ExceptionMessage = x.ExceptionMessage,
                                        ExceptionCode = x.ExceptionCode,
                                        ExceptionHeaderId = x.ExceptionHeaderId,
                                        StackTrace = x.StackTrace,
                                        Status = x.Status,
                                        StatusText = x.Status.GetDescription(),
                                        AppId = x.AppId,
                                        Id = x.Id
                                    })
                                    .ToList();
            return (pagedexception);                                                              
        }

        public IEnumerable<ExceptionDTO> GetExceptionsByUser(ApplicationUser user)
        {
            var exceptions = new List<ExceptionDTO>();
            var appByUser = _dbContext.ApplicationDb
                            .Where(app => app.UserId == user.Id)
                            .ToList();
                foreach (var app in appByUser) {

                var exceptionfilterlist = _dbContext.ExceptionDb.Where(exception => exception.AppId == app.Id).Where(exception => exception.Status != ExceptionStatus.Completed).Select(item => new ExceptionDTO { Id = item.Id}).ToList();
                //return (exceptionfilterlist);
                exceptions.AddRange(exceptionfilterlist);
                
            }
                return (exceptions);
        }
        public ExceptionDTO UpdateException(string id)
        {
            var exception = _dbContext.ExceptionDb.FirstOrDefault(x => x.Id == id);
            if (exception == null) { return (null); }
            var exceptiondto = new ExceptionDTO()
            {
                Id = exception.Id,
                AppId = exception.AppId,
                ExceptionHeaderId = exception.ExceptionHeaderId,
                StackTrace = exception.StackTrace,
                Status = exception.Status,
                ExceptionCode = exception.ExceptionCode,
                ExceptionMessage = exception.ExceptionMessage,
            };
            return (exceptiondto);
        }

        public ExceptionDTO ViewExceptionById(string id)
        {
            var exception = _dbContext.ExceptionDb.FirstOrDefault(x => x.Id == id);
            var exceptiondto = new ExceptionDTO() { Id = exception.Id, AppId = exception.AppId };
            return (exceptiondto);
        }
    }
}
