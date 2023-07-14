using System.ComponentModel.DataAnnotations;

namespace JYD.DataContext.DbModel
{
  

    public class NewAllocationDB
    {
        [Key]
        public string? REF_NO { get; set; }
        public string? CONTROL_NO { get; set; }
     
    }
}
