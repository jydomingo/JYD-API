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
    public class PlateAllocationInquiryController : BaseController
    {
        private readonly IProcedureRepository _procedureRepository;
        private ILogger<PlateAllocationInquiryController> log;
        private SeriLogger<PlateAllocationInquiryController> logger;

        public PlateAllocationInquiryController(SeriLogger<PlateAllocationInquiryController> seriLogger, IProcedureRepository procedureRepository)
        {
            logger = seriLogger;
            _procedureRepository = procedureRepository;
        }

        [HttpPost]
        public async Task<ActionResult> Alloc([FromBody] List<RequestParameter> request)
        {
            string user_id = ParameterExtension.GetParameterValue(request, "USER_ID");
            string? site_code = ParameterExtension.GetParameterValue(request, "SITE_CODE");
            string? agency_code = ParameterExtension.GetParameterValue(request, "AGENCY_CODE");
            string? ref_no = ParameterExtension.GetParameterValue(request, "REF_NO");
            string? item_code = ParameterExtension.GetParameterValue(request, "ITEM_CODE");
            string? status = ParameterExtension.GetParameterValue(request, "STATUS");
            DateTime date_from = ParameterExtension.GetParameterValue(request, "DATE_FROM");
            DateTime date_to = ParameterExtension.GetParameterValue(request, "DATE_TO");
            // @p_site_code,@p_agency_code,@p_ref_no,@p_item_code,@p_status ,@p_date_from,@p_date_to

            List<Allocation_Inquiry>? Alloc_ref;

            try
            {
                log = logger.LogFromContext(user_id);
                
                Alloc_ref = await _procedureRepository.Allocation_Inquiries(site_code, agency_code, ref_no, item_code, status, date_from, date_to );

                if (Alloc_ref == null)
                {
                    log.LogInformation("Plate Allocation Inquiry {0}", user_id);
                    log.LogInformation("The plate inquiry not found.");

                    return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "The Plate inquiry not found.." });
                }

                log.LogInformation("User Login Request:\r\n{0} ", Alloc_ref.Dump());

            }
            catch (Exception ex)
            {
                log.LogError(ex, "Plate Inquiry Request: {0}", user_id);

                return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "The Plate inquiry not found." });
            }

            return Json(new ResponseParameter { Code = HttpStatusCode.OK, Data = JsonConvert.SerializeObject(Alloc_ref), ErrorMessage = null });

        }

    }
}
