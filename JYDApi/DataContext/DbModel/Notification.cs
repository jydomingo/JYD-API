using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace JYD.DataContext.DbModel
{
    [Keyless]
    public class Notification
    {
        public string ID { get; set; }
        public string USER_ID { get; set; }
        public string AGENCY_CODE { get; set; }
        public string REF_NO { get; set; }
        public string MESSAGE { get; set; }
        public int STATUS { get; set; }
        public DateTime? DATE_CREATED { get; set; }
    }
}
