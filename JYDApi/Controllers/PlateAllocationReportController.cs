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
    public class PlateAllocationReportController : BaseController
    {
        private readonly IProcedureRepository _procedureRepository;
        private ILogger<PlateAllocationReportController> log;
        private SeriLogger<PlateAllocationReportController> logger;

        public PlateAllocationReportController(SeriLogger<PlateAllocationReportController> seriLogger, IProcedureRepository procedureRepository)
        {
            logger = seriLogger;
            _procedureRepository = procedureRepository;
        }

        [HttpPost]
        public async Task<ActionResult> PlateAllocRep([FromBody] List<RequestParameter> request)
        {
            string? user_id = ParameterExtension.GetParameterValue(request, "USER_ID");
            string? site_code = ParameterExtension.GetParameterValue(request, "SITE_CODE");
            string? agency_code = ParameterExtension.GetParameterValue(request, "AGENCY_CODE");
            string? ref_no = ParameterExtension.GetParameterValue(request, "REF_NO");
            string? item_code = ParameterExtension.GetParameterValue(request, "ITEM_CODE");
            string? status = ParameterExtension.GetParameterValue(request, "STATUS");
            DateTime date_from = ParameterExtension.GetParameterValue(request, "DATE_FROM");
            DateTime date_to = ParameterExtension.GetParameterValue(request, "DATE_TO");

            List<PlateAllocationReport>? PlateAllocRepVar;

            try
            {
                log = logger.LogFromContext(user_id);

                PlateAllocRepVar = await _procedureRepository.PlateAllocationRep(site_code, agency_code, ref_no, item_code, status, date_from, date_to );

                if (PlateAllocRepVar == null)
                {
                    log.LogInformation("Plate Allocation Report {0}", user_id);
                    log.LogInformation("Plate Allocation Report not found.");

                    return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "Plate Allocation Report not found.." });
                    
                }

                log.LogInformation("User Login Request:\r\n{0} ", PlateAllocRepVar.Dump());

            }
            catch (Exception ex)
            {
                log.LogError(ex, "Plate Allocation Report Request: {0}", user_id);

                return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "Plate Allocation Report not found." });
                //return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = ex.Message }); //From Sproc
            }

            return Json(new ResponseParameter { Code = HttpStatusCode.OK, Data = JsonConvert.SerializeObject(PlateAllocRepVar), ErrorMessage = null });

        }

    }
}
