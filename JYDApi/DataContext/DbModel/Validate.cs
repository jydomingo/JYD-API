using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace JYD.DataContext.DbModel
{
    [Keyless]
    public class Validate
    
    {
        public decimal TOTAL { get; set; }
        public string? ERROR_MSG { get; set; }
    }

    [Keyless]
    public class CheckDuplicate
    {
        public string? CONTROL_NO { get; set; }
        public Int16 SEQ_NO { get; set; }
        public string? REF_NO { get; set; }
        public string? SERIES_START { get; set; }
        public string? SERIES_END { get; set; }
        public string? STATUS_CODE { get; set; }
        public string? ERR_MSG { get; set; }
    }

    [Keyless]
    public class Exploded
    {
        public string? CONTROL_NO { get; set; }
        public Int16 SEQ_NO { get; set; }
        public string? SERIES_START { get; set; }
        public string? SERIES_END { get; set; }
        public Int16 QTY_ALLOCATED { get; set; }
        public Int16 QTY_EXCLUDED { get; set; }
        public string? PLATE_EXCEPTION { get; set; }
    }

    public class All_Result
    {
        public Validate TotalSeriesResult { get; set; }
        public List<CheckDuplicate>? DuplicateSeriesResult { get; set; }
        public Exploded? ExplodedSeriesResult { get; set; }
    }




}
