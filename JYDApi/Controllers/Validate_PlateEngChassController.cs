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
    public class Validate_PlateEngChassController : BaseController
    {

        private readonly IProcedureRepository _procedureRepository;
        private ILogger<Validate_PlateEngChassController> log;
        private SeriLogger<Validate_PlateEngChassController> logger;

        public Validate_PlateEngChassController(SeriLogger<Validate_PlateEngChassController> seriLogger, IProcedureRepository procedureRepository)
        {
            logger = seriLogger;
            _procedureRepository = procedureRepository;
        }

        [HttpPost]
        public async Task<ActionResult> ValidatePEC([FromBody] List<RequestParameter> request)
        {
           string? agency_code = ParameterExtension.GetParameterValue(request, "AGENCY_CODE");
           string? plate_no = ParameterExtension.GetParameterValue(request, "PLATE_NO");
           string? status_code = ParameterExtension.GetParameterValue(request, "STATUS_CODE");
           string? engine_no = ParameterExtension.GetParameterValue(request, "ENGINE_NO");
           string? chassis_no = ParameterExtension.GetParameterValue(request, "CHASSIS_NO");
           DateTime date_reserved = ParameterExtension.GetParameterValue(request, "DATE_RESERVED");
           string? item_code = ParameterExtension.GetParameterValue(request, "ITEM_CODE");
           string? user_id = ParameterExtension.GetParameterValue(request, "USER_ID");
           string? old_values = ParameterExtension.GetParameterValue(request, "OLD_VALUES");

            List<Validate_PlateEngChass>? _PlateEngChass;
            string? description = "";
            try
            {
                log = logger.LogFromContext(user_id);

                _PlateEngChass = await _procedureRepository.validate_PEC(agency_code, plate_no, status_code, engine_no, chassis_no, date_reserved,item_code, old_values, user_id);

                if (_PlateEngChass == null)
                {
                    log.LogInformation("Plate Request: {0}", plate_no);
                    log.LogInformation("Validation Error.");

                    return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "Validation for plate returns null." });
                }
                                
                //switch (_PlateEngChass.REMARKS)
                //{
                //    case "101":
                //        description = "Plate does not exists.";
                //        break;
                //    case "102":
                //         description = "Plate is assigned to another dealer";
                //        break;
                //    case "301":
                //        description = "Plate not yet assigned (TM status)";
                //        break;
                //    case "302":
                //        description = "Plate not yet assigned (AL status)";
                //        break;
                //    case "401":
                //        description = "Plate is already issued.";
                //        break;
                //    case "501":
                //        description = "Engine number is already assigned to another MV";
                //        break;
                //    case "601":
                //        description = "Chassis number is already assigned to another MV";
                //        break;
                //    case "701":
                //        description = "Update of plate status failed.";
                //        break;
                //    default:
                //         description = "Successfully updated plate status";
                //        break;
                //}
                 
                log.LogInformation(_PlateEngChass + " {0}");

            }
            catch (Exception ex)
            {
                log.LogError(ex, "Plate Request: {0}", plate_no);

                return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "Validation for plate error." });
            }

            return Json(new ResponseParameter { Code = HttpStatusCode.OK, Data = JsonConvert.SerializeObject(_PlateEngChass), ErrorMessage = null });

        }











    }
}
