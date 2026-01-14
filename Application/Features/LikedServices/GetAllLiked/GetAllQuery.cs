using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Application.Common.DTO.Response;

namespace Application.Features.LikedServices.GetAllLiked
{
    public record GetAllQuery : IRequest<List<LikedMediaResponse>>;
}
