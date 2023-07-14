using Logging;
using Microsoft.AspNetCore.Mvc;
using JYD.DataContext.Repository;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using ServiceStack.Text;
using JYD.DataContext.DbModel;
using JYD.Helper;
using System.Net;
using System.Text.RegularExpressions;
using JYD.Controllers.Base;

namespace JYD.Controllers
{
    public class ProcessAcceptanceController : BaseController
    {

        private readonly IProcedureRepository _procedureRepository;
        private ILogger<ProcessAcceptanceController> log;
        private SeriLogger<ProcessAcceptanceController> logger;

        public ProcessAcceptanceController(SeriLogger<ProcessAcceptanceController> seriLogger, IProcedureRepository procedureRepository)
        {
            logger = seriLogger;
            _procedureRepository = procedureRepository;
        }

        [HttpPost]
        public async Task<ActionResult> UpdateProcess([FromBody] List<RequestParameter> request)
        {
            string? ref_no = ParameterExtension.GetParameterValue(request, "REF_NO");
            string? user_id = ParameterExtension.GetParameterValue(request, "USER_ID");


            int processA;

            try
            {
                log = logger.LogFromContext(user_id);

                processA = await _procedureRepository.ProcessAcceptance(ref_no, user_id);

                if (processA == null)
                {
                    log.LogInformation("Control Number Request: {0}", ref_no);
                    log.LogInformation("The Control Number not found.");

                    return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "The Reference Number not found." });
                }

                log.LogInformation("User Login Request:\r\n{0} ", processA.Dump());

            }
            catch (Exception ex)
            {
                log.LogError(ex, "Control Number Request: {0}", ref_no);

                return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "The Reference Number not found." });
            }

            return Json(new ResponseParameter { Code = HttpStatusCode.OK, Data = null, ErrorMessage = null });

        }
















    }
}
