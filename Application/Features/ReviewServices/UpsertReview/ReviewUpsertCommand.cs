using Application.Common.DTO.Response;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.ReviewServices.UpsertReview
{
    public record ReviewUpsertCommand(int? id, int? mediaId, Guid? userId, [Range(1, 10)] int Rating, string Comment) : IRequest<ReviewResponse>;
}
