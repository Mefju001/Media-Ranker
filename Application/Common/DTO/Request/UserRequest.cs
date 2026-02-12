namespace Application.Common.DTO.Request
{
    public class UserRequest
    {
        public required string username { get; set; }
        public required string password { get; set; }
        public required string name { get; set; }
        public required string surname { get; set; }
        public required string email { get; set; }
    }
}
