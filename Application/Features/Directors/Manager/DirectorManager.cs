using Application.Common.Interfaces;
using Application.Features.Directors.Common;
using Domain.Aggregate;

namespace Application.Features.Directors.Manager
{
    internal class DirectorManager : IDirectorManager
    {
        private readonly IDirectorRepository directorRepository;
        public DirectorManager(IDirectorRepository directorRepository)
        {
            this.directorRepository = directorRepository;
        }
        public async Task<IEnumerable<DirectorResponse>> GetOrCreateBatchAsync(List<DirectorRequest> directors, CancellationToken cancellationToken)
        {
            var uniquePairs = directors
            .Select(d => (Name: d.Name.Trim(), Surname: d.Surname.Trim()))
            .Distinct()
            .ToList();
            var existingDirectors = await directorRepository.FindByNamesAsync(uniquePairs, cancellationToken);
            var existingPairs = existingDirectors.Select(d => (d.fullname.Name, d.fullname.Surname)).ToHashSet();
            var newDirectors = new List<Director>();
            foreach (var pair in uniquePairs)
            {
                if (!existingPairs.Contains(pair))
                {
                    var newDirector = Director.Create(pair.Name, pair.Surname);
                    newDirectors.Add(newDirector);
                }
            }
            await directorRepository.AddRangeAsync(newDirectors, cancellationToken);
            return existingDirectors.Select(DirectorMapper.ToResponse).Concat(newDirectors.Select(DirectorMapper.ToResponse));
        }

        public async Task<DirectorResponse> GetOrCreateAsync(DirectorRequest directorRequest, CancellationToken cancellationToken)
        {
            var Director = await directorRepository.FirstOrDefaultForNameAndSurnameAsync(directorRequest.Name, directorRequest.Surname, cancellationToken);
            if (Director is not null) return DirectorMapper.ToResponse(Director);
            Director = Director.Create(directorRequest.Name, directorRequest.Surname);
            var result = await directorRepository.AddAsync(Director, cancellationToken);
            return DirectorMapper.ToResponse(result);
        }
    }
}
