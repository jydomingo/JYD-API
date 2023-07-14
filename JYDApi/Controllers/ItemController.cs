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
    public class ItemController : BaseController
    {
        private readonly IProcedureRepository _procedureRepository;
        private ILogger<ItemController> log;
        private SeriLogger<ItemController> logger;
        public ItemController(SeriLogger<ItemController> seriLogger, IProcedureRepository procedureRepository)
        {
            logger = seriLogger;            
            _procedureRepository = procedureRepository;
        }

        [HttpPost]
        public async Task<ActionResult> GetItemPerDealer([FromBody] List<RequestParameter> request)
        {
            string? dealer_code = ParameterExtension.GetParameterValue(request, "AGENCY_CODE");
            string? user_id = ParameterExtension.GetParameterValue(request, "USER_ID");

            List<ItemDB>? item;

            try
            {
                log = logger.LogFromContext(user_id);

                item = await _procedureRepository.GetListOfItem(dealer_code);

                if (item == null)
                {
                    log.LogInformation("Agency Code Request: {0}", dealer_code);
                    log.LogInformation("The agency code is incorrect.");

                    return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "The agency code is incorrect." });
                }
                        
                log.LogInformation("Agency Code Request:\r\n{0} ", item.Dump());

            }
            catch (Exception ex)
            {
                log.LogError(ex, "Agency Code Request: {0}", dealer_code);

                return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "The agency code is incorrect." });
            }

            return Json(new ResponseParameter { Code = HttpStatusCode.OK, Data = JsonConvert.SerializeObject(item), ErrorMessage = null });

        }
    }
}
