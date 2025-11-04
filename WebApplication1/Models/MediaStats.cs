using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class MediaStats
    {
        [Key]
        public int MediaId { get; set; }
        public required virtual Media Media { get; set; }

        public double? AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public DateTime? LastCalculated { get; set; }
    }
}
