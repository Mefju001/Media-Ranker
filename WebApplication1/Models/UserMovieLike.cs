using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class UserMovieLike
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int userId { get; set; }
        public User user { get; set; }

        public int movieId { get; set; }
        public Movie movie { get; set; }

        public DateTime LikedDate { get; set; } = DateTime.UtcNow;
    }
}
