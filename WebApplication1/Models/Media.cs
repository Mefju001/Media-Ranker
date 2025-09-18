using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public abstract class Media
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string title { get; set; }
        public required string description { get; set; }
        public int genreId { get; set; }
        public virtual required Genre genre { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string? Language { get; set; }
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
