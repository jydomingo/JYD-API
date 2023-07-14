using Microsoft.EntityFrameworkCore;

namespace JYD.DataContext.DbModel
{
    [Keyless]
    public class RevertPlateAllocation
    {
        public Int16? SEQ_NO { get; set; }
        public string? SITE_CODE { get; set; }
        public string? AGENCY_CODE { get; set; }
        public string? ITEM_CODE { get; set; }
        public string? SERIES_START { get; set; }
        public string? SERIES_END { get; set; }
        public Int16? QTY { get; set; }
        public string? STATUS_CODE { get; set; }
        public Int16? EXCEPTION_QTY { get; set; }
        public string? REF_NO { get; set; }
        public string? CONTROL_NO { get; set; }
        
    }
}
