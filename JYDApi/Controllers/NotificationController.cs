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
    public class NotificationController : BaseController
    {
        private readonly IProcedureRepository _procedureRepository;
        private ILogger<NotificationController> log;
        private SeriLogger<NotificationController> logger;

        public NotificationController(SeriLogger<NotificationController> seriLogger, IProcedureRepository procedureRepository)
        {
            logger = seriLogger;
            _procedureRepository = procedureRepository;
        }


        [HttpPost]
        public async Task<ActionResult> GetNotification([FromBody] List<RequestParameter> request)
        {
            string user_id = ParameterExtension.GetParameterValue(request, "USER_ID");

            List<Notification>? notification;

            try
            {
                log = logger.LogFromContext(user_id);

                notification = await _procedureRepository.GetNotification(user_id);

                if (notification == null)
                {
                    return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "No message notification." });
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Notification Exception");
                return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "No message notification." });
            }

            return Json(new ResponseParameter { Code = HttpStatusCode.OK, Data = JsonConvert.SerializeObject(notification), ErrorMessage = null });
        }

        [HttpPost]
        public async Task<ActionResult> GetNotificationById([FromBody] List<RequestParameter> request)
        {
            string id = ParameterExtension.GetParameterValue(request, "ID");
            string user_id = ParameterExtension.GetParameterValue(request, "USER_ID");

            Notification? notification;

            try
            {
                log = logger.LogFromContext(user_id);

                notification = await _procedureRepository.GetNotificationById(id);

                if (notification == null)
                {
                    return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "No message notification." });
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Notification Exception");
                return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "No message notification." });
            }

            return Json(new ResponseParameter { Code = HttpStatusCode.OK, Data = JsonConvert.SerializeObject(notification), ErrorMessage = null });
        }

        [HttpPost]
        public async Task<ActionResult> ReadNotification([FromBody] List<RequestParameter> request)
        {
            string id = ParameterExtension.GetParameterValue(request, "ID");
            string user_id = ParameterExtension.GetParameterValue(request, "USER_ID");

            try
            {
                log = logger.LogFromContext(user_id);

                int affectedRows = await _procedureRepository.ReadNotification(id, user_id);

                if (affectedRows <= 0)
                {
                    return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "No record found." });
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Notification Exception");
                return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "No record found." });
            }

            return Json(new ResponseParameter { Code = HttpStatusCode.OK, Data = null, ErrorMessage = null });
        }

        [HttpPost]
        public async Task<ActionResult> SaveNotification([FromBody] List<RequestParameter> request)
        {
            string id = ParameterExtension.GetParameterValue(request, "ID");
            string user_id = ParameterExtension.GetParameterValue(request, "USER_ID");
            string agency_code = ParameterExtension.GetParameterValue(request, "AGENCY_CODE");
            string ref_no = ParameterExtension.GetParameterValue(request, "REF_NO");
            string messasge = ParameterExtension.GetParameterValue(request, "MESSAGE");

            try
            {
                log = logger.LogFromContext(user_id);

                int affectedRows = await _procedureRepository.SaveNotification(id, user_id, agency_code, ref_no, messasge);

                if (affectedRows <= 0)
                {
                    return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "Record not save." });
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Notification Exception");
                return Json(new ResponseParameter { Code = HttpStatusCode.NotFound, Data = null, ErrorMessage = "Record not save." });
            }

            return Json(new ResponseParameter { Code = HttpStatusCode.OK, Data = null, ErrorMessage = null });
        }

    }
}
