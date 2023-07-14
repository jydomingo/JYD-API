using System.ComponentModel.DataAnnotations;

namespace JYD.DataContext.DbModel
{
  

    public class AllocationDtlDB
    {
        [Key]
        public string? CONTROL_NO { get; set; }
        public int? SEQ_NO { get; set; }
        public string? REF_NO { get; set; }
        public string? PLATE_NO { get; set; }
        public string? STATUS_CODE { get; set; }
        public string? USER_ID { get; set; }
        public DateTime? DATE_CREATED { get; set; }


    }
}
