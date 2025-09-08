using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class LikedMedia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int userId { get; set; }
        public User User { get; set; }

        public int mediaId { get; set; }
        public Media Media { get; set; }
        public MediaType mediaType { get; set; }

        public DateTime LikedDate { get; set; } = DateTime.UtcNow;
    }
}
