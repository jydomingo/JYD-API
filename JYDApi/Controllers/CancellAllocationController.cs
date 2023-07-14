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
    public class CancellAllocationController : BaseController
    {
        private readonly IProcedureRepository _procedureRepository;
        private ILogger<CancellAllocationController> log;
        private SeriLogger<CancellAllocationController> logger;

        public CancellAllocationController(SeriLogger<CancellAllocationController> seriLogger, IProcedureRepository procedureRepository)
        {
            logger = seriLogger;
            _procedureRepository = procedureRepository;
        }

        [HttpPost]
        public async Task<ActionResult> CancellAllocation([FromBody] List<RequestParameter> request)
        {
            string control_no = ParameterExtension.GetParameterValue(request, "CONTROL_NO");
            string user_id = ParameterExtension.GetParameterValue(request, "USER_ID");

            int affectedRows;

            try
            {
                log = logger.LogFromContext(user_id);

                affectedRows = await _procedureRepository.CancellAllocation(control_no);

                if (affectedRows <= 0)
                {
                    log.LogInformation("Cancell Allocation Request: {0}", control_no);
                    log.LogInformation("Unable to Cancell Allocation.");

                    return Json(new ResponseParameter { Code = HttpStatusCode.NotModified, Data = null, ErrorMessage = "Unable to Cancell Allocation." });
                }

                log.LogInformation("Cancell allocation has been successfully executed {0}", control_no);

            }
            catch (Exception ex)
            {
                log.LogError(ex, "Cancell Allocation Request: {0}", control_no);

                return Json(new ResponseParameter { Code = HttpStatusCode.NotModified, Data = null, ErrorMessage = "Unable to Cancell Allocation." });
            }

            return Json(new ResponseParameter { Code = HttpStatusCode.OK, Data = null, ErrorMessage = null });

        }

        [HttpPost]
        public async Task<ActionResult> RemoveTMAllocation([FromBody] List<RequestParameter> request)
        {            
            string user_id = ParameterExtension.GetParameterValue(request, "USER_ID");

            int affectedRows;

            try
            {
                log = logger.LogFromContext(user_id);

                affectedRows = await _procedureRepository.RemoveTMAllocation(user_id);

                if (affectedRows <= 0)
                {
                    log.LogInformation("Remove Temporary Allocation Request: {0}", user_id);
                    log.LogInformation("Unable to Remove Temporary Allocation.");

                    return Json(new ResponseParameter { Code = HttpStatusCode.NotModified, Data = null, ErrorMessage = "Unable to Remove Temporary Allocation." });
                }

                log.LogInformation("Remove Temporary Allocation has been successfully executed {0}", user_id);

            }
            catch (Exception ex)
            {
                log.LogError(ex, "Remove Temporary Allocation Request: {0}", user_id);

                return Json(new ResponseParameter { Code = HttpStatusCode.NotModified, Data = null, ErrorMessage = "Unable to Remove Temporary Allocation." });
            }

            return Json(new ResponseParameter { Code = HttpStatusCode.OK, Data = null, ErrorMessage = null });

        }
    }
}
