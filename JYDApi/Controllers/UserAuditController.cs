using Logging;
using Microsoft.AspNetCore.Mvc;
using JYD.DataContext.Repository;
using Newtonsoft.Json;
using ServiceStack.Text;
using JYD.DataContext.DbModel;
using JYD.Helper;
using System.Net;
using JYD.Controllers.Base;

namespace JYD.Controllers
{
    public class UserAuditController : BaseController
    {
        private readonly IProcedureRepository _procedureRepository;
        private ILogger<UserAuditController> log;
        private SeriLogger<UserAuditController> logger;

        public UserAuditController(SeriLogger<UserAuditController> seriLogger, IProcedureRepository procedureRepository)
        {
            logger = seriLogger;
            _procedureRepository = procedureRepository;
        }

        [HttpPost]
        public async Task<ActionResult> GetUserAudit([FromBody] List<RequestParameter> request)
        {
            string? user_id = ParameterExtension.GetParameterValue(request, "USER_ID");


            UserAuditDB? UserAudit;

            try
            {
                log = logger.LogFromContext(user_id);

                UserAudit = await _procedureRepository.GetUserAudit(user_id);

                if (UserAudit == null)
                {
                    log.LogInformation("User History Request: {0}", user_id);
                    log.LogInformation("The user id not found.");

                    return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "The user id not found." });
                }

                log.LogInformation("User History Request:\r\n{0} ", UserAudit.Dump());

            }
            catch (Exception ex)
            {
                log.LogError(ex, "User History Request: {0}", user_id);

                return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "The user id not found." });
            }

            return Json(new ResponseParameter { Code = HttpStatusCode.OK, Data = JsonConvert.SerializeObject(UserAudit), ErrorMessage = null });

        }
    }
}
