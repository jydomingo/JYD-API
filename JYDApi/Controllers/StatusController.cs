using Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiceStack;
using ServiceStack.Text;
using JYD.Controllers.Base;
using JYD.DataContext.DbModel;
using JYD.DataContext.Repository;
using JYD.Helper;
using System.Net;

namespace JYD.Controllers
{
    public class StatusController : BaseController
    {
        private readonly IProcedureRepository _procedureRepository;
        private ILogger<StatusController> log;
        private SeriLogger<StatusController> logger;
        public StatusController(SeriLogger<StatusController> seriLogger, IProcedureRepository procedureRepository)
        {
            logger = seriLogger;            
            _procedureRepository = procedureRepository;
        }

        [HttpPost]
        public async Task<ActionResult> GetStatusRef([FromBody] List<RequestParameter> request)
        {
            string? table_name = ParameterExtension.GetParameterValue(request, "TABLE_NAME");
            string? user_id = ParameterExtension.GetParameterValue(request, "USER_ID");
            List<Status_Ref>? status;
            try
            {
                log = logger.LogFromContext(user_id);
                status = await _procedureRepository.Status_Ref(table_name);
                if (status == null)
                {
                    log.LogInformation("Status Reference Request: {0}", table_name);
                    log.LogInformation("The status reference not found.");

                    return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "The status reference not found." });
                }                        
                log.LogInformation("Status Reference Request:\r\n{0} ", status.Dump());
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Status Reference Request: {0}", table_name);

                return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "The status reference not found." });
            }
            return Json(new ResponseParameter { Code = HttpStatusCode.OK, Data = JsonConvert.SerializeObject(status), ErrorMessage = null });
        }
    }
}
