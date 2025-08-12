using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Movie:Media
    {
        public int directorId { get; set; }
        public virtual required Director director { get; set; }
        public TimeSpan Duration { get; set; }
        public bool IsCinemaRelease { get; set; }
    }
}