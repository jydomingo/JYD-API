using System.ComponentModel.DataAnnotations;

namespace JYD.DataContext.DbModel
{
    //[TABLE("USER_ACCOUNT_REF")]
     
    public class UserDB
    {
        [Key]
        public string USER_ID { get; set; }
        public string FIRST_NAME { get; set; }
        public string MIDDLE_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string AGENCY_CODE { get; set; }
        public string EMAIL_ADDRESS { get; set; }
        public string? PASSWORD_HASH { get; set; }
        public string? TEMP_PASSWORD { get; set; }
        public string USER_STATUS { get; set; }
        public DateTime DATE_CREATED { get; set; }
        public string AGENCY_NAME { get; set; }
        public string AGENCY_TYPE { get; set; }
        public string? SESSION_FLAG { get; set; }

    }
}
