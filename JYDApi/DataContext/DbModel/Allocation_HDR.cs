using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace JYD.DataContext.DbModel
{
    public class Allocation_HDR
    {
        public string CONTROL_NO { get; set; }
        public int SEQ_NO { get; set; }
        public string REF_NO { get; set; }
        public string SITE_CODE { get; set; }
        public string AGENCY_CODE { get; set; }
        public string ITEM_CODE { get; set; }
        public string SERIES_START { get; set; }
        public string SERIES_END { get; set; }
        public int QTY { get; set; }
        public string STATUS_CODE {get; set; }
        public string USER_ID { get; set; }
        public DateTime DATE_CREATED { get; set; }

    }
}
