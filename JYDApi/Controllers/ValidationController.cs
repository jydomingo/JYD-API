using Microsoft.AspNetCore.Mvc;
using Logging;
using JYD.DataContext.Repository;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using ServiceStack.Text;
using JYD.DataContext.DbModel;
using JYD.Helper;
using System.Net;
using System.Text.RegularExpressions;
using JYD.Controllers.Base;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using ServiceStack;

namespace JYD.Controllers
{
    public class ValidationController : BaseController
    {
        private readonly IProcedureRepository _procedureRepository;
        private ILogger<ValidationController> log;
        private SeriLogger<ValidationController> logger;

        public ValidationController(SeriLogger<ValidationController> seriLogger, IProcedureRepository procedureRepository)
        {
            logger = seriLogger;
            _procedureRepository = procedureRepository;
        }
        string? _continue = string.Empty;

        [HttpPost]
        public async Task<ActionResult> GetValidate([FromBody] List<RequestParameter> request)
        {
            string user_id = ParameterExtension.GetParameterValue(request, "USER_ID");
            string item_code = ParameterExtension.GetParameterValue(request, "ITEM_CODE");
            string range_fr = ParameterExtension.GetParameterValue(request, "RANGE_FR");
            string range_to = ParameterExtension.GetParameterValue(request, "RANGE_TO");
            string control_no = ParameterExtension.GetParameterValue(request, "CONTROL_NO");
                   control_no = control_no.Replace("-", "");
            string ref_no = string.Empty;
            string site_code = ParameterExtension.GetParameterValue(request, "SITE_CODE");
            string agency_code = ParameterExtension.GetParameterValue(request, "AGENCY_CODE");
            string status_code = "TM";
            string all_params = string.Empty;

            Validate? Validates;
            All_Result all_Result = new All_Result();
            List<CheckDuplicate>? checkDuplicate;
            Exploded? exploded_save;
            string ls_error_msg = string.Empty;
            try
            {
                log = logger.LogFromContext(user_id);

                #region Check the format of the plates
                
                Validates = await _procedureRepository.GetValidate(item_code, range_fr, range_to);
                all_params = item_code + " / " + range_fr + " / " + range_to;
                if (Validates == null)
                {
                    log.LogInformation("Validation Code Request: {0}", all_params);
                    log.LogInformation("Null object return from sproc_pmf_ValidatePlate");

                    return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "Null object return from sproc_pmf_ValidatePlate." });
                }
                ls_error_msg = string.IsNullOrEmpty(Validates.ERROR_MSG)?string.Empty:Validates.ERROR_MSG.Trim();
                #endregion

                if (ls_error_msg.Trim() == "")
                {
                    #region Check for duplicate/Overlapping of plates 
                   
                    checkDuplicate = await _procedureRepository.GetCheckDuplicate(item_code, range_fr, range_to);
                    
                    if (checkDuplicate == null || checkDuplicate.Count == 0) 
                    {
                        log.LogInformation("Validation Code Request: {0}", all_params);
                        log.LogInformation("No duplicate/Overlapping found.");
                    #endregion

                        #region Saving of plate
                        exploded_save = await _procedureRepository.GetExploded(control_no, ref_no, site_code, agency_code, item_code, range_fr, range_to, status_code, user_id);

                        if (exploded_save == null)
                        {
                            log.LogInformation("Save Header and Details: {0}", all_params);
                            log.LogInformation("No return from sproc_pmf_createallocationhdr");

                            return Json(new ResponseParameter { Code = HttpStatusCode.BadRequest, Data = null, ErrorMessage = "Error in Saving Header and Details." });
                        }
                        #endregion

                        all_Result.TotalSeriesResult = Validates;
                        all_Result.ExplodedSeriesResult = exploded_save;
                        return Json(new ResponseParameter { Code = HttpStatusCode.OK, Data = JsonConvert.SerializeObject(all_Result), ErrorMessage = null });
                    }

                    all_Result.TotalSeriesResult = Validates;
                    all_Result.DuplicateSeriesResult = checkDuplicate;

                    // return if duplicate/Overlapping found
                    return Json(new ResponseParameter { Code = HttpStatusCode.BadRequest, Data = JsonConvert.SerializeObject(all_Result), ErrorMessage = "" });
                }
                log.LogInformation("User Login Request:\r\n{0} ", Validates.Dump());
            }

            catch (Exception ex)
            {
                log.LogError(ex, "Validation Code Request: {0}", all_params);
                return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = ex.Message });
            }
          

            /*Returns if there's an error in VAlidation of plate format*/
            return Json(new ResponseParameter { Code = HttpStatusCode.BadRequest, Data = JsonConvert.SerializeObject(Validates), ErrorMessage = ls_error_msg });

            


        }
    }
}


