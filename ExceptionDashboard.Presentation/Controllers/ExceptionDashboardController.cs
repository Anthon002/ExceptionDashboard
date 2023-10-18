using ExceptionDashboard.Application.Services;
using ExceptionDashboard.Core.Models;
using ExceptionDashboard.Core.Models.DTOs;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

/*
 * This is the entry point of the application
 * Login Url (ApplicationUser/Login)
 * Registration Url (ApplicationUser/SignUp)
 * Regsitration Url Admin Privileges (ApplicationAdminUser/Signup)
 * 
 * 
 * The Clean Architecture structure used in the code is Core, Application, Infrastructure and Presentation
 * The Design Patterns used in the code are the:
 *      - Service pattern 
 *      - The Repository Pattern
 * The Packages that were used in this code were 
 *      - EnitityFrameworkCore(dbContext)
 *      - Identity(Authentication and Authorization)
 *      - sib_api_v3_sdk(Brevo Mail service for api emailing)
 */

namespace ExceptionDashboard.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExceptionDashboardController : Controller
    {
        private readonly IApplicationService _IapplicationService;
        private readonly IExceptionService _IexceptionService;
        private readonly IExceptionHeaderService _IexceptionHeaderService;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public ExceptionDashboardController(
            IApplicationService IapplicationService, 
            IExceptionService IexceptionService,
            IExceptionHeaderService IexceptionHeaderService,
            UserManager<ApplicationUser> userManager
            )
        {
            _IapplicationService = IapplicationService;
            _IexceptionService = IexceptionService;
            _IexceptionHeaderService = IexceptionHeaderService;
            _userManager = userManager;
        }
        /**
         * Application
         */
        [HttpPost("GetApplication")]
        public async Task<ActionResult<ApplicationDTO>> GetApplication([FromBody] ApplicationDTO applicationdto)
        {
            /*
             * This is the endpoint for recieving application POST request 
             * This endpoint saves the recieved application to the databsae using Service and Repositiory Patterns
             */
            if (applicationdto != null)
            {
                _IapplicationService.SaveApplicationToDb(applicationdto);
                return Ok(applicationdto);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet("ViewApplications")]
        public async Task<ActionResult<IEnumerable<ApplicationDTO>>> ViewApplications()
        {
            /*
             * This endpoint lists out all registered appliations
             */
            TempData["User"] = _userManager.GetUserId(this.User);
            return Ok(_IapplicationService.ViewAllApplications());
        }
        [HttpDelete("DeleteApplication")]
        public async Task<ActionResult<ApplicationDTO>> DeleteApplication(string key)
        {
            return Ok(_IapplicationService.DeleteApplication(key));
        }

        /**
         * Exceptions
         */
        [HttpPost("GetException")]
        public async Task<ActionResult<ExceptionRequest>> GetException([FromBody] ExceptionDTO exceptionrequest)
        {
            /*
             * This is the endpoint for recieving exception POST requests
             * This endpoint saves the recieved exception to the databsae using Service and Repositiory Patterns
             */

            if (exceptionrequest != null)
            {
                return Ok(_IexceptionService.SaveExceptionToDb(exceptionrequest));
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet("ViewExceptions")]
        public async Task<ActionResult> ViewExceptions(int pageNumber, int pageSize)
        {
            var result =  _IexceptionService.ViewAllExceptions(pageNumber, pageSize);
            return Ok(result);
        }
        [HttpDelete("DeleteException")]
        public async Task<ActionResult<ExceptionDTO>> DeleteException(string Id)
        {
            if (Id != null)
            {
                return Ok(_IexceptionService.DeleteException(Id));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut("UpdateStatus")]
        public async Task<ActionResult<ExceptionDTO>> UpdateExceptionStatus(string Id, int status)
        {
            /*
             * This enpoint updates the status (pending, inprogress, completed) of an exception
             */
            if (Id == null) { return NotFound();}
            if (status == null) { return (null); }
            return(_IexceptionService.UpdateStatus(Id, status));
        }

        [HttpGet("ViewSpecificExceptions")]
        public async Task<ActionResult<IEnumerable<ExceptionDTO>>> ViewSpecificExceptions(string AppId, int pageNumber, int pageSize)
        {
            return Ok(_IexceptionService.ViewSpecificExceptions(AppId, pageNumber, pageSize));
        }
        [HttpPost("SendExceptionsToUser")]
        public async Task<ActionResult> BackgroundTask()
        {
            /*
             * This enpoints sends periodic mails about the number of unhandled exceptions each use is responsible for perhour
             */
            RecurringJob.AddOrUpdate("myrecurringjob", () => SendExceptionMail(), Cron.MinuteInterval(59));
            return Ok();
        }
        [HttpPost("ExceptionMail")]
        public async Task<ActionResult<ExceptionDTO>> SendExceptionMail()
        {
            /* 
             * This endpoint is called by the BackgroundTask() enpoint
             */
            var users = _userManager.Users.ToList();
            foreach(var user in users)
            {
                _IexceptionService.SendExceptionMail(user);

            }
            return Ok();
        }
        /**
         * ExceptionHeaders
         */

        [HttpPost("GetExceptionHeader")]
        public async Task<ActionResult<ExceptionHeaderDTO>> GetExceptionHeader([FromBody] ExceptionHeaderDTO exceptoinheaderdto)
        {
            /*
             * This is the endpoint for recieving exceptionHeader POST request 
             * This endpoint saves the recieved exceptionHeader to the databsae using Service and Repositiory Patterns
             */

            if (exceptoinheaderdto != null)
            {
                return _IexceptionHeaderService.SaveExceptionHeaderToDB(exceptoinheaderdto);

            }
            else
            { return BadRequest(null); }
        }


        [HttpGet("ViewExceptionHeaders")]
        public async Task<ActionResult<IEnumerable<ExceptionHeaderDTO>>> ViewExceptionHeader()
        {
            return Ok(_IexceptionHeaderService.ViewExceptionHeader());
        }

        [HttpDelete("DeleteExceptionHeader")]
        public async Task<ActionResult<ExceptionHeaderDTO>> DeleteExceptionHeader(string key)
        {
            if (key == null) { return (null); }
            return (_IexceptionHeaderService.DeleteException(key));
        }
        
    }
}
