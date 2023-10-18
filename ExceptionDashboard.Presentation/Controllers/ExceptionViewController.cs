using ExceptionDashboard.Application.Services;
using ExceptionDashboard.Core.Models.DTOs;
using ExceptionDashboard.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ExceptionDashboard.WebApi.Controllers
{
    public class ExceptionViewController : Controller
    {
        private readonly IExceptionService _IexceptionService;
        public ExceptionViewController(ApplicationDbContext dbContext, IExceptionService IexceptionService)
        {
            
            _IexceptionService = IexceptionService;
        }
        [HttpGet]
        [RequireConfirmedEmail]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ExceptionDTO>>> ViewAllExceptions()
        {
            return View();
        }
        [RequireConfirmedEmail]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExceptionDTO>>> ViewSpecificExceptions(string Id) //Recieving App Id
        {
            /*
             * Passing the Id to view to use in the request URL 
             */
            ViewData["appId"] = Id;
            return View();
        }

        public async Task<ActionResult<ExceptionDTO>> UpdateException(string id)
        {
            return View(_IexceptionService.UpdateException(id));
        }
        public async Task<ActionResult<ExceptionDTO>> UpdateExceptionStatus(string id, int status)
        {
            var exception = _IexceptionService.ViewExceptionById(id);
            _IexceptionService.UpdateStatus(id,status);
            return RedirectToAction("ViewSpecificExceptions", "ExceptionView", new {id = exception.AppId});
        }
        public async Task<ActionResult<ExceptionDTO>> DeleteException(string id)
        {
            var exception = _IexceptionService.ViewExceptionById(id);
            _IexceptionService.DeleteException(id);
            return RedirectToAction("ViewSpecificExceptions", "ExceptionView", new { id = exception.AppId });


        }

    }
}
