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
    public class ReportbyRefNoController : BaseController
    {
        private readonly IProcedureRepository _procedureRepository;
        private ILogger<ReportbyRefNoController> log;
        private SeriLogger<ReportbyRefNoController> logger;

        public ReportbyRefNoController(SeriLogger<ReportbyRefNoController> seriLogger, IProcedureRepository procedureRepository)
        {
            logger = seriLogger;
            _procedureRepository = procedureRepository;
        }

         
        [HttpPost]
        public async Task<ActionResult> Alloc([FromBody] List<RequestParameter> request)
        {
            string? user_id = ParameterExtension.GetParameterValue(request, "USER_ID");
            string? dealer = ParameterExtension.GetParameterValue(request, "DEALER");
            string? refno = ParameterExtension.GetParameterValue(request, "REFNO");
            

            List<ReportbyRefNo>? reportbyRef;
            combineResult combores = new combineResult();
            allochdr? allocattionhdr;

            try
            {
                log = logger.LogFromContext(user_id);
                reportbyRef = await _procedureRepository.ReportbyRefNo(dealer, refno); 

                if (reportbyRef == null)
                {
                    log.LogInformation("Reference Number {0}", user_id);
                    log.LogInformation("The Reference inquiry not found.");

                    return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "The Reference inquiry not found." });
                }
                combores.repbyrep = reportbyRef;

                allocattionhdr = await _procedureRepository.allochdr(refno);

                if (allocattionhdr == null)
                {
                    log.LogInformation("Reference Number {0}", user_id);
                    log.LogInformation("The Reference inquiry not found.");

                    return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "The Reference/Allocation header inquiry not found." });
                }
                combores.alloc = allocattionhdr;

                log.LogInformation("User Login Request:\r\n{0} ", allocattionhdr.Dump());

            }
            catch (Exception ex)
            {
                log.LogError(ex, "Reference Inquiry Request: {0}", user_id);

                return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "The Reference inquiry not found." });
            }

            return Json(new ResponseParameter { Code = HttpStatusCode.OK, Data = JsonConvert.SerializeObject(combores), ErrorMessage = null });

        }





    }
}
