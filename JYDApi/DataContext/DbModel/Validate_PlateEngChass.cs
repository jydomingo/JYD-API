using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace JYD.DataContext.DbModel
{
    [Keyless]
    public class Validate_PlateEngChass
    {
        public string? ERROR_CODE { get; set; }
        public string? ERROR_DESC { get; set; }
    
    }
}
