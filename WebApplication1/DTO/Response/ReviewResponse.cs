namespace WebApplication1.DTO.Response
{
    public record ReviewResponse(int id,int MediaId, string username, int rating, string comment, DateTime CreatedAt, DateTime? LastModifiedAt);

}
