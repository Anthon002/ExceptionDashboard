using ExceptionDashboard.Application.Services.IRepositories;
using ExceptionDashboard.Core.Models;
using ExceptionDashboard.Core.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using sib_api_v3_sdk.Api;   // Brevo Email for sending mails using apikeys
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using System.Net.Mail;
using System.Net;

namespace ExceptionDashboard.Application.Services
{
    public class ExceptionService : IExceptionService
    {
        private readonly IExceptionRepository _IexceptionRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _HttpContextAccessor;
       // private readonly ApplicationDbContext
        public ExceptionService(IExceptionRepository IexcptionRepository, UserManager<ApplicationUser> userManager, IHttpContextAccessor HttpContextAccessor)
        {
            _IexceptionRepository = IexcptionRepository;
            _userManager = userManager;
            _HttpContextAccessor = HttpContextAccessor;
        }

        public ExceptionDTO SaveExceptionToDb(ExceptionDTO exception)
        {
            if (exception != null)
            {
                return (_IexceptionRepository.SaveExceptionToDb(exception));
            }
            else
            {
                return (null);
            }
        }
        public ExceptionDTO DeleteException(string key)
        {
            if (key != null)
            {
                return (_IexceptionRepository.DeleteException(key));
            }
            else
            {
                return (null);
            }
        }
        public IEnumerable<ExceptionDTO> ViewAllExceptions(int pageNumber, int pageSize)
        {
            return (_IexceptionRepository.ViewAllExceptions(pageNumber,pageSize));
        }
        public ExceptionDTO UpdateStatus(string key, int status)
        {
            return _IexceptionRepository.UpdateStatus(key, status);
        }
        public IEnumerable<ExceptionDTO> ViewSpecificExceptions(string Id, int pageNumber, int pageSize)
        {
            return _IexceptionRepository.ViewSpecificExceptions(Id, pageNumber, pageSize);
        }

        public async Task<ExceptionDTO> SendExceptionMail(ApplicationUser user)
        {
            if(user == null) { return (null); }
            /**
             * Logic of getting exceptions:
             *      _IexceptionRepository.GetExceptionByUser(user)
             *      from the user:
             *          var app where userId == user.Id is defined
             *          var exceptions where appId == app.Id and where exceptionStatus != completed are put in a list
             *          the eceptions.count() is sent to the user.Email
             *          
             */
            var exceptions = _IexceptionRepository.GetExceptionsByUser(user);
            if(exceptions == null) { return (null); }
            var length = exceptions.Count();

            string apiKey = "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";
           // var user = await _userManager.FindByIdAsync(id);

            if (user == null) { return (null); }

            Configuration.Default.ApiKey["api-key"] = apiKey;

            var apiInstance = new TransactionalEmailsApi();
            string SenderName = "Chinedu Anulugwo";
            string SenderEmail = "chineduanulugwo@gmail.com";
            SendSmtpEmailSender Email = new SendSmtpEmailSender(SenderName, SenderEmail);
            string ToEmail = user.Email.ToString().ToLower();
            string ToName = user.UserName.ToString();
            SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(ToEmail, ToName);
            List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo>();
            To.Add(smtpEmailTo);
            string HtmlContent = null;
            string TextContent = "There are " + length.ToString() + " Exceptions You need To Attend To";
            string Subject = "Email Confirmation";

            try
            {
                var sendSmtpEmail = new SendSmtpEmail(Email, To, null, null, HtmlContent, TextContent, Subject);
                CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);
                Console.WriteLine("Response: \n" + result.ToJson());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }

            return await (Task<ExceptionDTO>)exceptions;

            // //initialize the to, from and email i.e MailAddress to = new MailAddress("recipient")
            // MailAddress to = new MailAddress(user.Email.ToString().ToLower());
            // MailAddress from = new MailAddress("chineduanulugwo@gmail.com");
            // MailMessage email = new MailMessage(from,to);
            // email.Body = "There are " + length.ToString() + " Exceptions You need To Attend To";

            // SmtpClient smtp = new SmtpClient();
            // smtp.Host = "smtp-relay.brevo.com";
            // smtp.Port = 587;
            // smtp.Credentials = new NetworkCredential("","");
            //// smtp.EnableSsl = true;
            // smtp.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
            // smtp.PickupDirectoryLocation = @"c::\Users\PTAD\Desktop\ExceptionHandlerTesting";
            // smtp.Send(email);

            //return await (Task<ExceptionDTO>)exceptions;
        }
        public ExceptionDTO UpdateException(string id)
        {
            return (_IexceptionRepository.UpdateException(id));
        }

        public ExceptionDTO ViewExceptionById(string id)
        {
            return (_IexceptionRepository.ViewExceptionById(id));
        }
    }
}
