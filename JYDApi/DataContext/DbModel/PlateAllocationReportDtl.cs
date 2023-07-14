using Microsoft.EntityFrameworkCore;

namespace JYD.DataContext.DbModel
{
    [Keyless]
    public class PlateAllocationReportDtl
    {
        public DateTime? DATE_ALLOCATED { get; set; }
        public DateTime? DATE_RECEIVED { get; set; }
        public string? STATUS_HDR { get; set; }
        public string? STATUS_DESCRIPTION { get; set; }
        public string? CONTROL_NO { get; set; }
        public string? REFERENCE_NO { get; set; }
        public string? DEALER_CODE { get; set; }
        public string? DEALER_NAME { get; set; }
        public string? SITE_CODE { get; set; }
        public string? SITE_NAME { get; set; }
        public string? ITEM_CODE { get; set; }
        public string? ITEM_DESC { get; set; }
        public int? TOTAL_ALLOCATED { get; set; } 
        public int? TOTAL_AVAILABLE { get; set; }
        public int? TOTAL_RESERVED { get; set; }
        public int? TOTAL_EXCEPTION { get; set; } 
        public DateTime? DATE_COMPLETED { get; set; }
        public string? ALLOCATED_BY { get; set; }
        public string? PLATE_NO { get; set; }
        public string? STATUS_PLATE { get; set; }
        public string? ENGINE_NO { get; set; }
        public string? CHASSIS_NO { get; set; }
        public DateTime? DATE_RESERVED { get; set; }
        public Int16? SEQ_NO { get; set; }

    }
}
