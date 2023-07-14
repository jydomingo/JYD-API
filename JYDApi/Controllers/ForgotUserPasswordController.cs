using Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using JYD.DataContext.DbModel;
using JYD.DataContext.Repository;
using JYD.Helper;
using System.Net;
using JYD.Controllers.Base;
using System.Net.Mail;
using JYD.Mailer;

namespace JYD.Controllers
{    
    public class ForgotUserPasswordController : BaseController
    {
        private readonly IMailService _mailService;

        private readonly IProcedureRepository _procedureRepository;
        private ILogger<ForgotUserPasswordController> log;
        private SeriLogger<ForgotUserPasswordController> logger;

        private static bool upper, lower, number, special;
        private static int passwordlenght;

        public ForgotUserPasswordController(SeriLogger<ForgotUserPasswordController> seriLogger, IProcedureRepository procedureRepository, IMailService mailService)
        {
            logger = seriLogger;
            _procedureRepository = procedureRepository;
            _mailService = mailService;
        }

        [HttpPost]
        public async Task<ActionResult> ForgotPassword([FromBody] List<RequestParameter> request)
        {

            upper = lower = number = special = true;

            Random random = new Random();
            passwordlenght = random.Next(8, 20);

            string? user_id = ParameterExtension.GetParameterValue(request, "USER_ID");
            string? emailAddress = ParameterExtension.GetParameterValue(request, "EMAIL_ADDRESS");
            string? temp_password = PasswordGenerate.GeneratePassword(upper, lower, number, special, passwordlenght);
            

            int affectedRows;

            try
            {
                log = logger.LogFromContext(user_id);

                affectedRows = await _procedureRepository.ForgotUserPassword(user_id, temp_password, emailAddress);

                if (affectedRows <= 0)
                {
                    log.LogInformation("User {0} Forgot Password Request.", user_id);
                    log.LogInformation("Unable to request temporary password.");

                    return Json(new ResponseParameter { Code = HttpStatusCode.NotModified, Data = null, ErrorMessage = "Unable to request temporary password." });
                }

                log.LogInformation("User {0} Temporary Password has been set successfully.", user_id);

            }
            catch (Exception ex)
            {
                log.LogError(ex, "User {0} Forgot Password Request.", user_id);

                return Json(new ResponseParameter { Code = HttpStatusCode.NotModified, Data = null, ErrorMessage = "Unable to request temporary password." });
            }

            /* Start Get User Details */
            UserDB? UserdtlDBs;

            try
            {
                log = logger.LogFromContext(user_id);

                string? password_hash = null;

                UserdtlDBs = await _procedureRepository.GetDBs(user_id, password_hash, temp_password);

                if (UserdtlDBs == null)
                {
                    log.LogInformation("Request to get User Details: {0}", user_id);
                    log.LogInformation("User Details not found.");

                    return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "User Details not found." });
                }

                log.LogInformation("User Details found: {0}", user_id);

            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error getting User Details: {0}", user_id);

                return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "Error getting User Details." });
            }

            /* End Get User Details */
            return Json(new ResponseParameter { Code = HttpStatusCode.OK, Data = JsonConvert.SerializeObject(UserdtlDBs), ErrorMessage = null });

        }

        [HttpPost]
        public async Task<ActionResult> SendTemporaryPassword([FromBody] List<RequestParameter> request)
        {
            string? userId = ParameterExtension.GetParameterValue(request, "USER_ID");
            string? givenName = ParameterExtension.GetParameterValue(request, "GIVEN_NAME");
            string? emailAddress = ParameterExtension.GetParameterValue(request, "EMAIL_ADDRESS");
            string? tempPassword = ParameterExtension.GetParameterValue(request, "TEMP_PASSWORD");

            try
            {
                log = logger.LogFromContext(userId);
                log.LogInformation("Sending email to User {0} temporary password", userId);

                //checking if email address is valid
                using (var mailMessage = new MailMessage())
                {
                    if (!string.IsNullOrEmpty(emailAddress))
                        mailMessage.To.Add(emailAddress);
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Invalid Email Address: {0}", emailAddress);

                return Json(new ResponseParameter { Code = HttpStatusCode.BadRequest, Data = null, ErrorMessage = "Email address is not valid." });
            }

            try
            {
                await _mailService.SendTemporaryPassword(userId, emailAddress, tempPassword, givenName);

                log.LogInformation("Temporary Password to User {0} has been successfuly send.", userId);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error sending email to {0}", emailAddress);

                return Json(new ResponseParameter { Code = HttpStatusCode.InternalServerError, Data = null, ErrorMessage = "Internal Server Error" });
            }

            return Json(new ResponseParameter { Code = HttpStatusCode.OK, Data = null, ErrorMessage = null });
        }
    }

    public static class PasswordGenerate
    {
        private static string Upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static string Numbers = "0123456789";
        private static string SpecialChars = "!@#$&*";

        public static string GeneratePassword(
            bool useUpper,
            bool useLower,
            bool useNumbers,
            bool useSpecialChars,
            int PasswordSize)
        {
            Random rand = new Random();
            string charSet = string.Empty;
            char[] password = new char[PasswordSize];

            if (useUpper) charSet += Upper;
            if (useLower) charSet += Upper.ToLower();
            if (useNumbers) charSet += Numbers;
            if (useSpecialChars) charSet += SpecialChars;

            for (int i = 0; i < PasswordSize; i++)
            {
                password[i] = charSet[rand.Next(charSet.Length)];
            }
                        
            return String.Join(null, password);
        }
    }


}

