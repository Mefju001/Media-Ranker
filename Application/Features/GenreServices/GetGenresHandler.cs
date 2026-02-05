using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.GenreServices
{
    public class GetGenresHandler:IRequestHandler<GetGenresQuery, List<GenreResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetGenresHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<List<GenreResponse>> Handle(GetGenresQuery request, CancellationToken cancellationToken)
        {
            var genres = await unitOfWork.GenreRepository.GetAllAsync(cancellationToken);
            return genres.Select(GenreMapper.ToResponse).ToList();
        }
    }
}
