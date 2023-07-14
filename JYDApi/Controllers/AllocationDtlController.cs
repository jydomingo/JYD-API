using Logging;
using Microsoft.AspNetCore.Mvc;
using JYD.DataContext.Repository;
using Newtonsoft.Json;
using ServiceStack.Text;
using JYD.DataContext.DbModel;
using JYD.Helper;
using System.Net;
using JYD.Controllers.Base;

namespace JYD.Controllers
{
    public class AllocationDtlController : BaseController
    {
        private readonly IProcedureRepository _procedureRepository;
        private ILogger<AllocationDtlController> log;
        private SeriLogger<AllocationDtlController> logger;

        public AllocationDtlController(SeriLogger<AllocationDtlController> seriLogger, IProcedureRepository procedureRepository)
        {
            logger = seriLogger;
            _procedureRepository = procedureRepository;
        }

        [HttpPost]
        public async Task<ActionResult> GetAllocationDtl([FromBody] List<RequestParameter> request)
        {
            string? control_no = ParameterExtension.GetParameterValue(request, "CONTROL_NO");
            string? user_id = ParameterExtension.GetParameterValue(request, "USER_ID");

            List<AllocationDtlDB>? AllocationDtl;
           
            try
            {
                log = logger.LogFromContext(user_id);

                AllocationDtl = await _procedureRepository.GetAllocationDtlperNo(control_no);

                if (AllocationDtl == null)
                {
                    log.LogInformation("Allocation Details Request: {0}", control_no);
                    log.LogInformation("The Control Number not found.");

                    return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "The Control Number not found." });
                }

                log.LogInformation("Allocation Details Request:\r\n{0} ", AllocationDtl.Dump());

            }
            catch (Exception ex)
            {
                log.LogError(ex, "Allocation Details Request: {0}", control_no);

                return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "The Control Number not found." });
            }

            return Json(new ResponseParameter { Code = HttpStatusCode.OK, Data = JsonConvert.SerializeObject(AllocationDtl), ErrorMessage = null });

        }

        [HttpPost]
        public async Task<ActionResult> DeleteAllocationDtl([FromBody] List<RequestParameter> request)
        {
            string control_no = ParameterExtension.GetParameterValue(request, "CONTROL_NO");
            int seq_no = ParameterExtension.GetParameterValue(request, "SEQ_NO");
            string? user_id = ParameterExtension.GetParameterValue(request, "USER_ID");

            int affectedRows;

            try
            {
                log = logger.LogFromContext(user_id);

                affectedRows = await _procedureRepository.DeleteAllocationDtl(control_no, seq_no, user_id);

                if (affectedRows <= 0)
                {
                    log.LogInformation("Delete Allocation Details Request: {0}", control_no);
                    log.LogInformation("Unable to Delete Allocation.");

                    return Json(new ResponseParameter { Code = HttpStatusCode.NotModified, Data = null, ErrorMessage = "Unable to Delete Allocation." });
                }

                log.LogInformation("Allocation Details has been successfully Deleted {0}", control_no);

            }
            catch (Exception ex)
            {
                log.LogError(ex, "Delete Allocation Details Request: {0}", control_no);

                return Json(new ResponseParameter { Code = HttpStatusCode.NotModified, Data = null, ErrorMessage = "Unable to Delete Allocation." });
            }

            return Json(new ResponseParameter { Code = HttpStatusCode.OK, Data = null, ErrorMessage = null });

        }

        [HttpPost]
        public async Task<ActionResult> UpdateAllocationDtl([FromBody] List<RequestParameter> request)
        {
            string? user_id = ParameterExtension.GetParameterValue(request, "USER_ID");
            string? plate_no = ParameterExtension.GetParameterValue(request, "PLATE_NO");
            string? status_code = ParameterExtension.GetParameterValue(request, "STATUS_CODE");
            string? engine_no = ParameterExtension.GetParameterValue(request, "ENGINE_NO");
            string? chassis_no = ParameterExtension.GetParameterValue(request, "CHASSIS_NO");
            DateTime date_reserved = ParameterExtension.GetParameterValue(request, "DATE_RESERVED");

            int affectedRows;

            try
            {
                log = logger.LogFromContext(user_id);

                affectedRows = await _procedureRepository.UpdateAllocationDtl(plate_no, status_code, engine_no, chassis_no, date_reserved, user_id);

                if (affectedRows <= 0)
                {
                    log.LogInformation("Update Allocation Details Request: {0}", plate_no);
                    log.LogInformation("Unable to Update Allocation.");

                    return Json(new ResponseParameter { Code = HttpStatusCode.NotModified, Data = null, ErrorMessage = "Unable to Update Allocation." });
                }

                log.LogInformation("Allocation Details has been successfully Updated {0}", plate_no);

            }
            catch (Exception ex)
            {
                log.LogError(ex, "Update Allocation Details Request: {0}", plate_no);

                return Json(new ResponseParameter { Code = HttpStatusCode.NotModified, Data = null, ErrorMessage = "Unable to Update Allocation." });
            }

            return Json(new ResponseParameter { Code = HttpStatusCode.OK, Data = null, ErrorMessage = null });

        }
    }
}
