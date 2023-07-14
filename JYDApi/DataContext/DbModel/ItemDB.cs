using System.ComponentModel.DataAnnotations;

namespace JYD.DataContext.DbModel
{
    //[TABLE("USER_ACCOUNT_REF")]

    public class ItemDB
    {
        [Key]
        public string? ITEM_CODE { get; set; }
        public string? ITEM_DESC { get; set; }
        public string? CLASSIFICATION { get; set; }
        public string? TYPE { get; set; }
        public string? STATUS_CODE { get; set; }
        public string? AGENCY_CODE { get; set; }


    }
}
