using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTO.Request
{
    public record ReviewRequest([Range(1, 10)] int Rating, string Comment);
}
