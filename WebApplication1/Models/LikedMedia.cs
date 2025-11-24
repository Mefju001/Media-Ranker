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
        public virtual User User { get; set; }

        public int MediaId { get; set; }
        public virtual Media Media { get; set; }

        public DateTime LikedDate { get; set; } = DateTime.UtcNow;
    }
}
