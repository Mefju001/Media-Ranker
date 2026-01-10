namespace WebApplication1.Application.Common.DTO.Request
{
    public class UserRequest
    {
        public required string username { get; set; }
        public required string password { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
    }
}
