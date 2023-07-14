using Microsoft.EntityFrameworkCore;

namespace JYD.DataContext.DbModel
{

    [Keyless]
    public class ReportbyPlateNo
    {
        public string? STATUS_CODE { get; set; }
        public string? STATUS { get; set; }
        public string? DEALER { get; set; }
        public string? PLATE_NO { get; set; }
        public string? ENGINE_NO { get; set; }
        public string? CHASSIS_NO { get; set; }
        public string? DATE_RESERVED { get; set; }
    }
}
