using Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiceStack.Text;
using JYD.Controllers.Base;
using JYD.DataContext.DbModel;
using JYD.DataContext.Repository;
using JYD.Helper;
using System.Net;

namespace JYD.Controllers
{
    public class UserController : BaseController
    {
        private readonly IProcedureRepository _procedureRepository;
        private ILogger<UserController> log;
        private SeriLogger<UserController> logger;
        public UserController(SeriLogger<UserController> seriLogger, IProcedureRepository procedureRepository)
        {
            logger = seriLogger;            
            _procedureRepository = procedureRepository;
        }

        [HttpPost]
        public async Task<ActionResult> GetUser([FromBody] List<RequestParameter> request)
        {
            string? user_id = ParameterExtension.GetParameterValue(request, "USER_ID");
            string? password = ParameterExtension.GetParameterValue(request, "PASSWORD_HASH");
            string? temp = ParameterExtension.GetParameterValue(request, "TEMP_PASSWORD");

            UserDB? user;

            try
            {
                log = logger.LogFromContext(user_id);

                user = await _procedureRepository.GetDBs(user_id, password, temp);

                if(user == null)
                {
                    log.LogInformation("User Login Request: {0}", user_id);
                    log.LogInformation("The user name or password are incorrect.");

                    return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "The user name or password are incorrect." });
                }
                        
                log.LogInformation("User Login Request:\r\n{0} ", user.Dump());

            }
            catch (Exception ex)
            {
                log.LogError(ex, "User Login Request: {0}", user_id);

                return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "The user name or password are incorrect." });
            }

            return Json(new ResponseParameter { Code = HttpStatusCode.OK, Data = JsonConvert.SerializeObject(user), ErrorMessage = null });

        }
    }
}
