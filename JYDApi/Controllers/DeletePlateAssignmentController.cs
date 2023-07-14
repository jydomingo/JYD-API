using Logging;
using Microsoft.AspNetCore.Mvc;
using JYD.Controllers.Base;
using JYD.DataContext.Repository;
using JYD.Helper;
using System.Net;

namespace JYD.Controllers
{
    public class DeletePlateAssignmentController : BaseController
    {
        private readonly IProcedureRepository _procedureRepository;
        private ILogger<DeletePlateAssignmentController> log;
        private SeriLogger<DeletePlateAssignmentController> logger;
        public DeletePlateAssignmentController(SeriLogger<DeletePlateAssignmentController> seriLogger, IProcedureRepository procedureRepository)
        {
            logger = seriLogger;
            _procedureRepository = procedureRepository;
        }

        [HttpPost]
        public async Task<ActionResult> DeletePlateAssignment([FromBody] List<RequestParameter> request)
        {
            string? plate_no = ParameterExtension.GetParameterValue(request, "PLATE_NO");
            string? user_id = ParameterExtension.GetParameterValue(request, "USER_ID");
            
            int affectedRows;

            try
            {
                log = logger.LogFromContext(user_id);

                affectedRows = await _procedureRepository.DeletePlateAssignment(plate_no, user_id);

                if (affectedRows <= 0)
                {
                    log.LogInformation("Delete Plate Assignment {0}", user_id);
                    log.LogInformation("Delete Plate Assignment not found.");

                    return Json(new ResponseParameter { Code = HttpStatusCode.NotModified, Data = null, ErrorMessage = "Delete Plate Assignment not found" });
                }

                log.LogInformation("User {0}  Plate Assign has been successfully deleted ", user_id);

            }
            catch (Exception ex)
            {
                log.LogError(ex, "Delete Plate Assignment: {0}", user_id);

                return Json(new ResponseParameter { Code = HttpStatusCode.NotModified, Data = null, ErrorMessage = "Error in (Delete Plate Assignment Controller) catch Exception." });
            }

            return Json(new ResponseParameter { Code = HttpStatusCode.OK, Data = null, ErrorMessage = null });
            
        }
    }
}



