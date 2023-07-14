using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JYD.DataContext.DbModel
{

    public class Status_Ref
    {
        [Key, Column("TABLE_NAME", Order = 0)]
        public string? TABLE_NAME { get; set; }
        [Key, Column("STATUS_CODE", Order = 1)]
        public string? STATUS_CODE{ get; set; }
        public string? STATUS_DESCRIPTION { get; set; }
        public DateTime DATE_CREATED { get; set; }
        

    }
}
