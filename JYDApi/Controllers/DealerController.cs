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
    public class DealerController : BaseController
    {
        private readonly IProcedureRepository _procedureRepository;
        private ILogger<DealerController> log;
        private SeriLogger<DealerController> logger;
        public DealerController(SeriLogger<DealerController> seriLogger, IProcedureRepository procedureRepository)
        {
            logger = seriLogger;            
            _procedureRepository = procedureRepository;
        }

        [HttpPost]
        public async Task<ActionResult> GetDealerPerDO([FromBody] List<RequestParameter> request)
        {
            string? site_code = ParameterExtension.GetParameterValue(request, "SITE_CODE");
            string? user_id = ParameterExtension.GetParameterValue(request, "USER_ID");
            


            List<DealerDB>? dealer;

            try
            {
                log = logger.LogFromContext(user_id);

                dealer = await _procedureRepository.GetListOfDealer(site_code);

                if (dealer == null)
                {
                    log.LogInformation("Site Code Request: {0}", site_code);
                    log.LogInformation("The site code is incorrect.");

                    return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "The site code is incorrect." });
                }
                        
                log.LogInformation("Site Code Request:\r\n{0} ", dealer.Dump());

            }
            catch (Exception ex)
            {
                log.LogError(ex, "Site Code Request: {0}", site_code);

                return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "The site code is incorrect." });
            }

            return Json(new ResponseParameter { Code = HttpStatusCode.OK, Data = JsonConvert.SerializeObject(dealer), ErrorMessage = null });

        }
    }
}
