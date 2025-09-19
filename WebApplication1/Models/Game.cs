namespace WebApplication1.Models
{
    public class Game : Media
    {
        public string? Developer { get; set; }
        public required string Platform { get; set; }
    }
}
