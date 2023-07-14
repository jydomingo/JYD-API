using Logging;
using Microsoft.AspNetCore.Mvc;
using JYD.Controllers.Base;
using JYD.DataContext.Repository;
using JYD.Helper;
using System.Net;


namespace JYD.Controllers
{
    public class UpdateSessionController : BaseController
    {

        private readonly IProcedureRepository _procedureRepository;
        private ILogger<UpdateSessionController> log;
        private SeriLogger<UpdateSessionController> logger;
        public UpdateSessionController(SeriLogger<UpdateSessionController> seriLogger,IProcedureRepository procedureRepository)
        {
            logger= seriLogger;
            _procedureRepository= procedureRepository;
        }

        [HttpPost]
        public async Task<ActionResult> UpdateSession([FromBody] List<RequestParameter> request)
        {
            string? user_id = ParameterExtension.GetParameterValue(request, "USER_ID");
            string? session_log = ParameterExtension.GetParameterValue(request, "SESSION_LOG");

            int affectedRows;

            try
            {
                log = logger.LogFromContext(user_id);

                affectedRows = await _procedureRepository.UpdateSession(user_id, session_log);

                if (affectedRows <= 0)
                {
                    log.LogInformation("User {0} Change Session Log Request.", user_id);
                    log.LogInformation("Unable to change Session Log.");

                    return Json(new ResponseParameter { Code = HttpStatusCode.NotModified, Data = null, ErrorMessage = "Unable to change Session Log." });
                }

                log.LogInformation("User {0} Session Log has been successfully changed ", user_id);

            }
            catch (Exception ex)
            {
                log.LogError(ex, "User {0} Change Session Log Request", user_id);

                return Json(new ResponseParameter { Code = HttpStatusCode.NotModified, Data = null, ErrorMessage = "Unable to change Session Log." });
            }

            return Json(new ResponseParameter { Code = HttpStatusCode.OK, Data = null, ErrorMessage = null });

        }





















    }
}
