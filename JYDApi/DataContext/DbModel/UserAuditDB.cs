using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JYD.DataContext.DbModel
{
   

    public class UserAuditDB

    {
        [Key, Column("TIME_STAMP", Order = 1)]
        public DateTime TIME_STAMP { get; set; }

        [Key, Column("USER_ID", Order = 0)]
        public string USER_ID { get; set; }
        public string FIRST_NAME { get; set; }
        public string MIDDLE_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string AGENCY_CODE { get; set; }
        public string EMAIL_ADDRESS { get; set; }
        public string? PASSWORD_HASH { get; set; }
        public string? TEMP_PASSWORD { get; set; }
        public string USER_STATUS { get; set; }
        public string ACTION_BY { get; set; }
        public string ACTION_TAKEN { get; set; }
        public DateTime DATE_LAST_MODIFIED { get; set; }

    }
}
