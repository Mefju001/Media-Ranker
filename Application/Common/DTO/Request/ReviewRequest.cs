using System.ComponentModel.DataAnnotations;

namespace Application.Common.DTO.Request
{
    public record ReviewRequest(int Rating, string Comment);
}
