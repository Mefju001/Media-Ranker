using Application.Common.DTO.Response;
using MediatR;


namespace Application.Features.GenreServices
{
    public record GetGenresQuery: IRequest<List<GenreResponse>>
    {
    }
}
