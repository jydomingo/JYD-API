using System.ComponentModel.DataAnnotations;

namespace JYD.DataContext.DbModel
{
    public class Retrieve_UserdtlDB
    {
        [Key]
        public string? USER_ID { get; set; }
        public string? FIRST_NAME { get; set; }
        public string? MIDDLE_NAME { get; set; }
        public string? LAST_NAME { get; set;}
        public string? EMAIL_ADDRESS { get; set; }
        public string? AGENCY_CODE { get; set; } 
        public string? AGENCY_NAME { get; set; }
        public string? AGENCY_TYPE { get; set; }
        public string? SESSION_FLAG { get; set; }
    }
}
