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
    public class NewAllocationController : BaseController
    {
        private readonly IProcedureRepository _procedureRepository;
        private ILogger<NewAllocationController> log;
        private SeriLogger<NewAllocationController> logger;

        public NewAllocationController(SeriLogger<NewAllocationController> seriLogger, IProcedureRepository procedureRepository)
        {
            logger = seriLogger;
            _procedureRepository = procedureRepository;
        }

        [HttpPost]
        public async Task<ActionResult> ProcessNewAllocation([FromBody] List<RequestParameter> request)
        {
            string? control_no = ParameterExtension.GetParameterValue(request, "CONTROL_NO");
            string? user_id = ParameterExtension.GetParameterValue(request, "USER_ID");

            NewAllocationDB? NewAllocationDtl;

            //int affectedRows;

            try
            {
                log = logger.LogFromContext(user_id);

                NewAllocationDtl = await _procedureRepository.ProcessNewAllocation(control_no, user_id);

                if (NewAllocationDtl == null)
                {
                    log.LogInformation("Process New Allocation  Request: {0},{1}", control_no, user_id);
                    log.LogInformation("Unable to Process New Allocation.");

                    return Json(new ResponseParameter { Code = HttpStatusCode.NotModified, Data = null, ErrorMessage = "Unable to Process New Allocation." });
                }

                log.LogInformation("New Allocation has been successfully processed {0},{1}", control_no, user_id);

            }
            catch (Exception ex)
            {
                log.LogError(ex, "Process New Allocation Request: {0},{1}", control_no, user_id);

                //return Json(new ResponseParameter { Code = HttpStatusCode.NotModified, Data = null, ErrorMessage = "Unable to Process New Allocation." });
                return Json(new ResponseParameter { Code = HttpStatusCode.NotModified, Data = null, ErrorMessage = ex.Message });
            }

            return Json(new ResponseParameter { Code = HttpStatusCode.OK, Data = JsonConvert.SerializeObject(NewAllocationDtl), ErrorMessage = null });

        }


    }
}
