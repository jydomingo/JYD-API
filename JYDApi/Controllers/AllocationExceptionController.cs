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
    public class AllocationExceptionController : BaseController
    {
        private readonly IProcedureRepository _procedureRepository;
        private ILogger<AllocationExceptionController> log;
        private SeriLogger<AllocationExceptionController> logger;
        public AllocationExceptionController(SeriLogger<AllocationExceptionController> seriLogger, IProcedureRepository procedureRepository)
        {
            logger = seriLogger;            
            _procedureRepository = procedureRepository;
        }

        [HttpPost]
        public async Task<ActionResult> GetAlloctionException([FromBody] List<RequestParameter> request)
        {
            string control_no = ParameterExtension.GetParameterValue(request, "CONTROL_NO");
            int seq_no = ParameterExtension.GetParameterValue(request, "SEQ_NO");
            string? user_id = ParameterExtension.GetParameterValue(request, "USER_ID");
            


            List<AllocationException>? alloc_excp;

            try
            {
                log = logger.LogFromContext(user_id);

                alloc_excp = await _procedureRepository.GetAlloctionException(control_no, seq_no);

                if (alloc_excp == null)
                {
                    log.LogInformation("Allocation Exception: {0}", control_no);
                    log.LogInformation("No record found");

                    return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "No record found." });
                }
                        
                log.LogInformation("Allocation Exception Request:\r\n{0} ", alloc_excp.Dump());

            }
            catch (Exception ex)
            {
                log.LogError(ex, "Allocation Exception: {0}", control_no);

                return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "No record found." });
            }

            return Json(new ResponseParameter { Code = HttpStatusCode.OK, Data = JsonConvert.SerializeObject(alloc_excp), ErrorMessage = null });

        }
    }
}
