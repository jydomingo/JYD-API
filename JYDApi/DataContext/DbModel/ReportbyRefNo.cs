using Microsoft.EntityFrameworkCore;

namespace JYD.DataContext.DbModel
{
    [Keyless]
    public class ReportbyRefNo
    {
        public string? DATE_ALLOCATED { get; set; }
        public string? DATE_RECEIVED { get; set; }
        public string? STATUS { get; set; }
        public string? REF_NO { get; set; }
        public string? ITEM_DESC { get; set; }
        public Int16 SEQ_NO { get; set; }
        public Int16 ALLOCATED { get; set; }
        public int RESERVED { get; set; }
        public int AVAILABLE { get; set; }
        public string? SERIES_START { get; set; }
        public string? SERIES_END { get; set; }

    }
      
    [Keyless]
    public class allochdr
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
        public string? ALLOCATED_BY { get; set; }
        public string? RECEIVED_BY { get; set; }

    }

    public class combineResult
    {
        public List<ReportbyRefNo>? repbyrep { get; set; }
        public allochdr? alloc { get; set; }

    }







}
