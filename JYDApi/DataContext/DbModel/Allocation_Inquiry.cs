using Microsoft.EntityFrameworkCore;

namespace JYD.DataContext.DbModel
{
    [Keyless]
    public class Allocation_Inquiry
    {
        public DateTime? DATE_ALLOCATED { get; set; }
        public DateTime? DATE_RECEIVED { get; set; }
        public string? STATUS_CODE { get; set; }
        public string? STATUS_DESCRIPTION { get; set; }
        public string? CONTROL_NO { get; set; }
        public string? REFERENCE_NO { get; set; }
        public string? DEALER_CODE { get; set; }
        public string? DEALER_NAME { get; set; }
        public string? ITEM_CODE { get; set; }
        public string? ITEM_DESC { get; set; }
        public int TOTAL_ALLOCATED { get; set; }
        public int TOTAL_AVAILABLE { get; set; }
        public int TOTAL_RESERVED { get; set; }
        public DateTime? DATE_COMPLETED { get; set; }   


    }
}
