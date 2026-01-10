namespace WebApplication1.Application.Common.DTO.Response
{
    public record ReviewResponse(int id, int MediaId, string username, int rating, string comment, DateTime CreatedAt, DateTime? LastModifiedAt);

}
