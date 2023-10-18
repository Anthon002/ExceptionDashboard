using ExceptionDashboard.Core.Models;
using ExceptionDashboard.Core.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using sib_api_v3_sdk.Api;   // Brevo Email for sending mails using apikeys
using sib_api_v3_sdk.Client;    // Brevo Email for sending mails using apikeys
using sib_api_v3_sdk.Model; // Brevo Email for sending mails using apikeys
using System.Net;
using System.Net.Mail;

namespace ExceptionDashboard.Application.Services
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        // private readonly IUserAuthenticationRepository _IuserRepositiory;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserAuthenticationService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public ApplicationUser SignUp(ApplicationUserDTO userdto, string role)
        {
            var roleId = _roleManager.Roles.FirstOrDefault(x => x.Name == role).Id;
            if (userdto == null) { return (null); }
            Random random = new Random();
            var otp = random.Next(1000, 9999);
                var user = new ApplicationUser()
                {
                    FirstName = userdto.FirstName,
                    LastName = userdto.LastName,
                    UserName = userdto.FirstName,
                    Email = userdto.Email,
                    OTP = otp,
                    RoleId = roleId
                };
            //  _userManager.CreateAsync in the ApplicationUserController
            return (user);
        }
        public async Task<bool> SendEmail(string id)
        {
            /*
             * Sending The Confirmation OTP to the Users email using Brevo API key from their API documentation (Package name: sib_api_v3_sdk, From: Brevo.com)
             */
            string apiKey = "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";
            var user = await _userManager.FindByIdAsync(id);

            if (user == null) { return (false); }

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
            string TextContent = "Your OTP is " + user.OTP.ToString();
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

            return (true);
        }

        public int ConfirmEmail(string confirmationotp, ApplicationUser user)
        {
            return (0);
        }

        public async Task<ApplicationUser> Login(ApplicationUser user)
        {
            if (user == null) { return (null); }
            SendEmail(user.Id.ToString());
            return (user);

        }
        public List<ApplicationUserDTO> GetAllUsers()
        {
            var users = _userManager.Users.ToList().Select(x => new ApplicationUserDTO(){ Id = x.Id, Email = x.Email, FirstName = x.FirstName, LastName = x.LastName }).ToList();
            return (users);

        }
    }
}
