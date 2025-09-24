using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Token
    {
        [Key]
        [Required]
        [StringLength(128)]
        public string Jti { get; set; } = string.Empty;

        [Required]
        [StringLength(512)]
        public string refreshToken { get; set; } = string.Empty;

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [Required]
        public DateTime IssuedAt { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        public bool IsRevoked { get; set; } = false;

        public DateTime? RevokedAt { get; set; }

        [StringLength(50)]
        public string? CreatedByIp { get; set; }

        [StringLength(256)]
        public string? UserAgent { get; set; }
    }
}
