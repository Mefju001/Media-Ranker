using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Application.Common.DTO.Request
{
    public record ReviewRequest([Range(1, 10)] int Rating, string Comment);
}
