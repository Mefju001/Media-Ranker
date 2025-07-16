using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Movie:Media
    {
        public TimeSpan Duration { get; set; }
        public bool IsCinemaRelease { get; set; }
    }
}