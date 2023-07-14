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
    public class AgencyController : BaseController
    {
        private readonly IProcedureRepository _procedureRepository;
        private ILogger<AgencyController> log;
        private SeriLogger<AgencyController> logger;

        public AgencyController(SeriLogger<AgencyController> seriLogger, IProcedureRepository procedureRepository)
        {
            logger = seriLogger;
            _procedureRepository = procedureRepository;
        }


        [HttpPost]
        public async Task<ActionResult> GetAgency([FromBody] List<RequestParameter> request)
        {
            string? agency_code = ParameterExtension.GetParameterValue(request, "AGENCY_CODE");
            string user_id = ParameterExtension.GetParameterValue(request, "USER_ID");


            Agency_Ref? Agencyref;

            try
            {
                log = logger.LogFromContext(user_id);

                Agencyref = await _procedureRepository.GetAgency_Ref(agency_code);

                if (Agencyref == null)
                {
                    log.LogInformation("Agency Code Request: {0}", agency_code);
                    log.LogInformation("The agency code not found.");

                    return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "The agency code not found." });
                }

                log.LogInformation("User Login Request:\r\n{0} ", Agencyref.Dump());

            }
            catch (Exception ex)
            {
                log.LogError(ex, "Agency Code Request: {0}", agency_code);

                return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "The agency code not found." });
            }

            return Json(new ResponseParameter { Code = HttpStatusCode.OK, Data = JsonConvert.SerializeObject(Agencyref), ErrorMessage = null });

        }
    }
}
