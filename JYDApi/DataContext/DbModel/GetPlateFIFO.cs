using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace JYD.DataContext.DbModel
{
    [Keyless]
    public class GetPlateFIFO
    {
        public string? PLATE_NO { get; set; }
        public string? STATUS_CODE { get; set; }

    }
}
