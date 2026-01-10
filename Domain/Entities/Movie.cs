namespace WebApplication1.Domain.Entities
{
    public class Movie : Media
    {
        public int directorId { get; set; }
        public virtual Director director { get; set; }
        public TimeSpan Duration { get; set; }
        public bool IsCinemaRelease { get; set; }
    }
}