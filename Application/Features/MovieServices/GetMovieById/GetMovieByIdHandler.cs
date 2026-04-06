using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Aggregate;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Features.MovieServices.GetMovieById
{
    public class GetMovieByIdHandler : IRequestHandler<GetMovieByIdQuery, MovieResponse?>
    {
        private readonly IAppDbContext appDbContext;

        public GetMovieByIdHandler(IAppDbContext appDbContext)
        {
            
            this.appDbContext = appDbContext;
        }

        public async Task<MovieResponse?> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
        {
            return await appDbContext.Set<Movie>()
                .AsNoTracking()
                .AsSplitQuery()
                .Select()
                .firstOrDefaultAsync(m => m.Id == request.id, cancellationToken);
        }
    }
}
