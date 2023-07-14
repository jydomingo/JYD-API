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
using ServiceStack;


namespace JYD.Controllers
{
    public class PlateAllocationReportDtlController : BaseController
    {
        private readonly IProcedureRepository _procedureRepository;
        private ILogger<PlateAllocationReportDtlController> log;
        private SeriLogger<PlateAllocationReportDtlController> logger;

        public PlateAllocationReportDtlController(SeriLogger<PlateAllocationReportDtlController> seriLogger, IProcedureRepository procedureRepository)
        {
            logger = seriLogger;
            _procedureRepository = procedureRepository;
        }

        [HttpPost]
        public async Task<ActionResult> PlateAllocationRepDtl([FromBody] List<RequestParameter> request)
        {
            string? user_id = ParameterExtension.GetParameterValue(request, "USER_ID");
            string? ref_no = ParameterExtension.GetParameterValue(request, "REF_NO");
            string? status = ParameterExtension.GetParameterValue(request, "STATUS");

            List<PlateAllocationReportDtl>? PlateAllocRepDtlVar;

            try
            {
                log = logger.LogFromContext(user_id);

                PlateAllocRepDtlVar = await _procedureRepository.PlateAllocationRepDtl(ref_no, status );

                if (PlateAllocRepDtlVar == null)
                {
                    log.LogInformation("Plate Allocation Report Detail {0}", user_id);
                    log.LogInformation("Plate Allocation Report Detail not found.");

                    return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "Plate Allocation Report Detail not found.." });
                    
                }

                log.LogInformation("User Login Request:\r\n{0} ", PlateAllocRepDtlVar.Dump());

            }
            catch (Exception ex)
            {
                log.LogError(ex, "Plate Allocation Report Detail Request: {0}", user_id);

                return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "Plate Allocation Report Detail not found." });
               
            }

            return Json(new ResponseParameter { Code = HttpStatusCode.OK, Data = JsonConvert.SerializeObject(PlateAllocRepDtlVar), ErrorMessage = null });

        }

    }
}
