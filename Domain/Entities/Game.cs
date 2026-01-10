using WebApplication1.Domain.ValueObjects;

namespace WebApplication1.Domain.Entities
{
    public class Game : Media
    {
        public string? Developer { get; set; }
        public required EPlatform Platform { get; set; }
    }
}
