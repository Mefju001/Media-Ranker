using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required int Rating { get; set; }
        public required string Comment { get; set; }
        public int MediaId { get; set; }
        public virtual Media Media { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public void SetReview(Review review)
        {
            this.Rating = review.Rating;
            this.Comment = review.Comment;
            this.MediaId = review.MediaId;
            this.UserId = review.UserId;
        }
    }
}