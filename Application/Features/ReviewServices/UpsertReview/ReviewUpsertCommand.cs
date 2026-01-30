using Application.Common.DTO.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ReviewServices.UpsertReview
{
    public record ReviewUpsertCommand(int?id, int? mediaId, int? userId, [Range(1, 10)] int Rating, string Comment):IRequest<ReviewResponse>;
}
