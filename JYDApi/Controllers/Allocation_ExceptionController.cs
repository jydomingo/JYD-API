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
    public class Allocation_ExceptionController : BaseController
    {

        private readonly IProcedureRepository _procedureRepository;
        private ILogger<Allocation_ExceptionController> log;
        private SeriLogger<Allocation_ExceptionController> logger;

        public Allocation_ExceptionController(SeriLogger<Allocation_ExceptionController> seriLogger, IProcedureRepository procedureRepository)
        {
            logger = seriLogger;
            _procedureRepository = procedureRepository;
        }


        [HttpPost]
        public async Task<ActionResult> AddAllocationEx([FromBody] List<RequestParameter> request)
        {
            
            string? control_no = ParameterExtension.GetParameterValue(request, "CONTROL_NO");
            int seq_no =  ParameterExtension.GetParameterValue(request, "SEQ_NO");
            string? plate_no = ParameterExtension.GetParameterValue(request, "PLATE_NO");
            string? user_id = ParameterExtension.GetParameterValue(request, "USER_ID");


            List<AllocationException>? allocationException;
            //int allocationException;
            try
            {
                log = logger.LogFromContext(user_id);

                allocationException = await _procedureRepository.allex(control_no, seq_no, plate_no, user_id);

                if (allocationException == null)
                {
                    log.LogInformation("Control Request: {0}", control_no);
                    log.LogInformation("Allocation Exception not added.");

                    return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "Allocation Exception not added." });
                }

                log.LogInformation("Allocation Exception has been successfully added. {0}", allocationException);

            }
            catch (Exception ex)
            {
                log.LogError(ex, "Control Request: {0}", control_no);

                return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "Catch Exception (Allocation_ExceptionController) Error." });
            }

            return Json(new ResponseParameter { Code = HttpStatusCode.OK, Data = JsonConvert.SerializeObject(allocationException), ErrorMessage = null });

        }


    }
}
