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
    public class UserDetailController : BaseController
    {
        private readonly IProcedureRepository _procedureRepository;
        private ILogger<UserDetailController> log;
        private SeriLogger<UserDetailController> logger;

        public UserDetailController(SeriLogger<UserDetailController> seriLogger, IProcedureRepository procedureRepository)
        {
            logger = seriLogger;
            _procedureRepository = procedureRepository;
        }

        [HttpPost]
        public async Task<ActionResult> GetUserDetail([FromBody] List<RequestParameter> request)
        {
            string? user_id = ParameterExtension.GetParameterValue(request, "USER_ID");

            Retrieve_UserdtlDB? UserdtlDBs;

            try
            {
                log = logger.LogFromContext(user_id);

                UserdtlDBs = await _procedureRepository.GetUserDetail(user_id);

                if (UserdtlDBs == null)
                {
                    log.LogInformation("User ID Request: {0}", user_id);
                    log.LogInformation("The User ID1 not found.");

                    return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "The User ID1 not found." });
                }

                log.LogInformation("User Login Request:\r\n{0} ", UserdtlDBs.Dump());

            }
            catch (Exception ex)
            {
                log.LogError(ex, "User ID Request: {0}", user_id);

                return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "The User ID2 not found." });
            }

            return Json(new ResponseParameter { Code = HttpStatusCode.OK, Data = JsonConvert.SerializeObject(UserdtlDBs), ErrorMessage = null });
        }
    }
}
