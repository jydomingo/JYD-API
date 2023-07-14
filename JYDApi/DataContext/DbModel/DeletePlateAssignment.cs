using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace JYD.DataContext.DbModel
{
    [Keyless]
    public class DeletePlateAssignment
    {
        public string? PLATENO { get; set; }
        public string? USER_ID { get; set; }

    }
}
