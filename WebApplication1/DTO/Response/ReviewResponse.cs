namespace WebApplication1.DTO.Response
{
    public record ReviewResponse
    {
        // Publiczny konstruktor bez argumentów
        public ReviewResponse() { }
        public ReviewResponse(string mediaName, string? username, int Rating, string Comment) { }
        public string? mediaName { get; init; }
        public string? username { get; init; }
        public int rating { get; init; }
        public string? comment { get; init; }
    }
}
