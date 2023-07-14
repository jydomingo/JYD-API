using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace JYD.DataContext.DbModel
{
    //[TABLE("USER_ACCOUNT_REF")]
    [Keyless]
    public class DealerDB
    {
        public string SITE_CODE { get; set; }
        public string AGENCY_CODE { get; set; }
        public string AGENCY_NAME { get; set; }
        public string ADDRESS { get; set; }
        public string AGENCY_TYPE { get; set; }
        public string REGION_CODE { get; set; }
        public string ACCREDITATION_ID { get; set; }
        public string? STATUS_CODE { get; set; }
        public DateTime? DATE_CREATED { get; set; }
       
        
    }
}
