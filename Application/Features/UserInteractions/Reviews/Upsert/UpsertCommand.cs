using Application.Features.Common.Interfaces;
using Application.Features.UserInteractions.Reviews.Common;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.UserInteractions.Reviews.Upsert
{
    public record UpsertCommand(Guid? id, Guid? mediaId, Guid? userId, [Range(1, 10)] int Rating, string Comment) : ICommand<ReviewResponse>;
}
