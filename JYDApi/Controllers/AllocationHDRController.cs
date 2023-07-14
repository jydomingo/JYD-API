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
    public class AllocationHDRController : BaseController
    {
        private readonly IProcedureRepository _procedureRepository;
        private ILogger<AllocationHDRController> log;
        private SeriLogger<AllocationHDRController> logger;

        public AllocationHDRController(SeriLogger<AllocationHDRController> seriLogger, IProcedureRepository procedureRepository)
        {
            logger = seriLogger;
            _procedureRepository = procedureRepository;
        }

        [HttpPost]
        public async Task<ActionResult> DelAllocHDRDetails([FromBody] List<RequestParameter> request)
        {
            string control_no = ParameterExtension.GetParameterValue(request, "CONTROL_NO");
            int seq_no = ParameterExtension.GetParameterValue(request, "SEQ_NO");
            string user_id = ParameterExtension.GetParameterValue(request, "USER_ID");

            int affectedRows;

            try
            {
                log = logger.LogFromContext(user_id);

                affectedRows = await _procedureRepository.DelAllocHDRDetails(control_no, seq_no);

                if (affectedRows <= 0)
                {
                    log.LogInformation("Delete Allocation HDR Request: {0}", control_no);
                    log.LogInformation("Unable to Delete Allocation.");

                    return Json(new ResponseParameter { Code = HttpStatusCode.NotModified, Data = null, ErrorMessage = "Unable to Delete Allocation." });
                }

                log.LogInformation("Allocation HRD has been successfully Deleted {0}", control_no);

            }
            catch (Exception ex)
            {
                log.LogError(ex, "Delete Allocation HRD Request: {0}", control_no);

                return Json(new ResponseParameter { Code = HttpStatusCode.NotModified, Data = null, ErrorMessage = "Unable to Delete Allocation." });
            }

            return Json(new ResponseParameter { Code = HttpStatusCode.OK, Data = null, ErrorMessage = null });

        }
    }
}
