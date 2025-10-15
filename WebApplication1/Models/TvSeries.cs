namespace WebApplication1.Models
{
    public class TvSeries : Media
    {
        public int Seasons { get; set; }
        public int Episodes { get; set; }
        public string? Network { get; set; }
        public EStatus Status { get; set; }
    }
}
