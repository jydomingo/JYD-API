using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace JYD.DataContext.DbModel
{
    [Keyless]
    public class AllocationException
    {
        public string? CONTROL_NO { get; set; }
        public Int16 SEQ_NO { get; set; }
        public string? PLATE_NO { get; set; }
        public string? USER_ID { get; set; }
        public DateTime? DATE_CREATED { get; set; }

    }
}
