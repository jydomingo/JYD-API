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
    public class ReportbyPlateNoController : BaseController
    {
        private readonly IProcedureRepository _procedureRepository;
        private ILogger<ReportbyPlateNoController> log;
        private SeriLogger<ReportbyPlateNoController> logger;

        public ReportbyPlateNoController(SeriLogger<ReportbyPlateNoController> seriLogger, IProcedureRepository procedureRepository)
        {
            logger = seriLogger;
            _procedureRepository = procedureRepository;
        }

        [HttpPost]
        public async Task<ActionResult> RepbyPlate([FromBody] List<RequestParameter> request)
        {
            string? user_id = ParameterExtension.GetParameterValue(request, "USER_ID");
            string? plate_no = ParameterExtension.GetParameterValue(request, "PLATE_NO");
            string? site_code = ParameterExtension.GetParameterValue(request, "SITE_CODE");
            string? agency_code = ParameterExtension.GetParameterValue(request, "AGENCY_CODE");

            List<ReportbyPlateNo>? reportbyPlate;

            try
            {
                log = logger.LogFromContext(user_id);

                reportbyPlate = await _procedureRepository.ReportbyPlateNos(plate_no, site_code,agency_code);

                if (reportbyPlate == null)
                {
                    log.LogInformation("Plate Number Request: {0}", plate_no);
                    log.LogInformation("The Plate Number not found.");

                    return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "The Plate Number not found." });
                }

                log.LogInformation("User Login Request:\r\n{0} ", reportbyPlate.Dump());

            }
            catch (Exception ex)
            {
                log.LogError(ex, "Plate Number Request: {0}", plate_no);

                return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "The Plate Number not found." });
            }

            return Json(new ResponseParameter { Code = HttpStatusCode.OK, Data = JsonConvert.SerializeObject(reportbyPlate), ErrorMessage = null });

        }

    }
}
