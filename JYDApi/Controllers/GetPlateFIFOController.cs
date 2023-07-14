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
    public class GetPlateFIFOController : BaseController
    {
        private readonly IProcedureRepository _procedureRepository;
        private ILogger<GetPlateFIFOController> log;
        private SeriLogger<GetPlateFIFOController> logger;

        public GetPlateFIFOController(SeriLogger<GetPlateFIFOController> seriLogger, IProcedureRepository procedureRepository)
        {
            logger = seriLogger;
            _procedureRepository = procedureRepository;
        }

        [HttpPost]
        public async Task<ActionResult> FIFO([FromBody] List<RequestParameter> request)
        {
            string? agency_code = ParameterExtension.GetParameterValue(request, "AGENCY_CODE");
            string? item_code = ParameterExtension.GetParameterValue(request, "ITEM_CODE");
            string? user_id = ParameterExtension.GetParameterValue(request, "USER_ID");

            List<GetPlateFIFO>? _plateFIFOs;
            
            try
            {
                log = logger.LogFromContext(user_id);

                _plateFIFOs = await _procedureRepository.plateFIFOs(agency_code, item_code);

                if (_plateFIFOs == null)
                {
                    log.LogInformation("Plate Request: {0}", user_id);
                    log.LogInformation("Retrieve Error.");

                    return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "Retrieve for fifo plate returns null." });
                }

               log.LogInformation("Retrieved FIFO Plate Request:\r\n{0} ", _plateFIFOs.Dump());
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Plate Request: {0}", user_id);

                return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "Retrieve for fifo plate error." });
            }

            return Json(new ResponseParameter { Code = HttpStatusCode.OK, Data = JsonConvert.SerializeObject(_plateFIFOs), ErrorMessage = null });

        }

    }
}
