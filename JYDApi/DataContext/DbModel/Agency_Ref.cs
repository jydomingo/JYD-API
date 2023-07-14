using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace JYD.DataContext.DbModel
{
    public class Agency_Ref
    {
        [Key]
        public string? AGENCY_CODE { get; set; }
        public string? AGENCY_NAME { get; set; }
        public string? ADDRESS { get; set; }
        public string? AGENCY_TYPE { get; set; }
        public string? REGION_CODE { get; set; }
        public string? ACCREDITATION_ID { get; set; }
        public string? STATUS_CODE { get; set; }
        public DateTime DATE_CREATED { get; set; }
        public string? MODIFYPLATE_SWITCH { get; set; }
    }
}
