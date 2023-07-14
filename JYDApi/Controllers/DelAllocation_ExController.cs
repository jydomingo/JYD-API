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
    public class DelAllocation_ExController : BaseController
    {

        private readonly IProcedureRepository _procedureRepository;
        private ILogger<DelAllocation_ExController> log;
        private SeriLogger<DelAllocation_ExController> logger;

        public DelAllocation_ExController(SeriLogger<DelAllocation_ExController> seriLogger, IProcedureRepository procedureRepository)
        {
            logger = seriLogger;
            _procedureRepository = procedureRepository;
        }


        [HttpPost]
        public async Task<ActionResult> DelAllocationEx([FromBody] List<RequestParameter> request)
        {

            string? control_no = ParameterExtension.GetParameterValue(request, "CONTROL_NO");
            int seq_no = ParameterExtension.GetParameterValue(request, "SEQ_NO");
            string? plate_no = ParameterExtension.GetParameterValue(request, "PLATE_NO");
            string? user_id = ParameterExtension.GetParameterValue(request, "USER_ID");


            List<AllocationException>? delAllocExecpt;

            try
            {
                log = logger.LogFromContext(user_id);

                delAllocExecpt = await _procedureRepository.delallex(control_no, seq_no, plate_no, user_id);

                if (delAllocExecpt == null)
                {
                    log.LogInformation("Control Request: {0}", control_no);
                    log.LogInformation("The Control number not found.");

                    return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "The Control number not found." });
                }

                log.LogInformation("User Login Request:\r\n{0} ", delAllocExecpt.Dump());

            }
            catch (Exception ex)
            {
                log.LogError(ex, "Control Request: {0}", control_no);

                return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "Error in (DelAllocation_ExController) catch Exception." });
            }

            return Json(new ResponseParameter { Code = HttpStatusCode.OK, Data = JsonConvert.SerializeObject(delAllocExecpt), ErrorMessage = null });

        }


    }
}
