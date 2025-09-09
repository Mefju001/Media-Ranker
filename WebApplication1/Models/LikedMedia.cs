using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class LikedMedia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int? MovieId { get; set; }
        public Movie? Movie { get; set; }

        public int? TvSeriesId { get; set; }
        public TvSeries? TvSeries { get; set; }

        public int? GameId { get; set; }
        public Game? Game { get; set; }

        public string Type { get; set; }

        public DateTime LikedDate { get; set; } = DateTime.UtcNow;
    }
}
