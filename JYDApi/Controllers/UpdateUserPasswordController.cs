using Logging;
using Microsoft.AspNetCore.Mvc;
using JYD.Controllers.Base;
using JYD.DataContext.Repository;
using JYD.Helper;
using System.Net;

namespace JYD.Controllers
{
    public class UpdateUserPasswordController : BaseController
    {
        private readonly IProcedureRepository _procedureRepository;
        private ILogger<UpdateUserPasswordController> log;
        private SeriLogger<UpdateUserPasswordController> logger;
        public UpdateUserPasswordController(SeriLogger<UpdateUserPasswordController> seriLogger, IProcedureRepository procedureRepository)
        {
            logger = seriLogger;            
            _procedureRepository = procedureRepository;
        }
        
        [HttpPost]        
        public async Task<ActionResult> UpdatePassword([FromBody] List<RequestParameter> request)
        {
            string? user_id = ParameterExtension.GetParameterValue(request, "USER_ID");
            string? password_hash = ParameterExtension.GetParameterValue(request, "PASSWORD_HASH");

            int affectedRows;

            try
            {
                log = logger.LogFromContext(user_id);

                affectedRows = await _procedureRepository.UpdateUserPassword(user_id, password_hash);

                if(affectedRows <= 0)
                {
                    log.LogInformation("User {0} Change Password Request", user_id);
                    log.LogInformation("Unable to change password.");

                    return Json(new ResponseParameter { Code = HttpStatusCode.NotModified, Data = null, ErrorMessage = "Unable to change password." });
                }
                        
                log.LogInformation("User {0} Password has been successfully changed ", user_id);

            }
            catch (Exception ex)
            {
                log.LogError(ex, "User {0} Change Password Request", user_id);

                return Json(new ResponseParameter { Code = HttpStatusCode.NotModified, Data = null, ErrorMessage = "Unable to change password." });
            }

            return Json(new ResponseParameter { Code = HttpStatusCode.OK, Data = null, ErrorMessage = null });

        }
    }
}
