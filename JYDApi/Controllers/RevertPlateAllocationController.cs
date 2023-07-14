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
    public class RevertPlateAllocationController : BaseController
    {
        private readonly IProcedureRepository _procedureRepository;
        private ILogger<RevertPlateAllocationController> log;
        private SeriLogger<RevertPlateAllocationController> logger;

        public RevertPlateAllocationController(SeriLogger<RevertPlateAllocationController> seriLogger, IProcedureRepository procedureRepository)
        {
            logger = seriLogger;
            _procedureRepository = procedureRepository;
        }

        [HttpPost]
        public async Task<ActionResult> RevertPlateAllocation([FromBody] List<RequestParameter> request)
        {
            string? ref_no = ParameterExtension.GetParameterValue(request, "REF_NO");
            string? user_id = ParameterExtension.GetParameterValue(request, "USER_ID");
            
            List<RevertPlateAllocation>? RevertPlateAllocationVar;

            try
            {
                log = logger.LogFromContext(user_id);

                RevertPlateAllocationVar = await _procedureRepository.RevertPlateAlloc(ref_no, user_id );

                if (RevertPlateAllocationVar == null)
                {
                    log.LogInformation("Revert Plate Allocation {0}", user_id);
                    log.LogInformation("Revert Plate Allocation not found.");

                    return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "Revert Plate Allocation not found.." });
                    
                }

                log.LogInformation("User Login Request:\r\n{0} ", RevertPlateAllocationVar.Dump());

            }
            catch (Exception ex)
            {
                log.LogError(ex, "Revert Plate Allocation Request: {0}", user_id);

                return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "Revert Plate Allocation not found." });
               
            }

            return Json(new ResponseParameter { Code = HttpStatusCode.OK, Data = JsonConvert.SerializeObject(RevertPlateAllocationVar), ErrorMessage = null });

        }

    }
}
