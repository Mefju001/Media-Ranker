using Application.Common.DTO.Request;
using Application.Common.Interfaces;
using Domain.Aggregate;


namespace Application.Common.Services
{
    public class DirectorHelperService:IDirectorHelperService
    {
        private readonly IDirectorRepository directorRepository;
        public DirectorHelperService(IDirectorRepository directorRepository)
        {
            this.directorRepository = directorRepository;
        }
        public async Task<Director> GetOrCreateDirectorAsync(DirectorRequest directorRequest, CancellationToken cancellationToken)
        {
            var Director = await directorRepository.FirstOrDefaultForNameAndSurnameAsync(directorRequest.Name, directorRequest.Surname, cancellationToken);
            if (Director is not null) return Director;
            Director = Director.Create(directorRequest.Name, directorRequest.Surname);
            var result = await directorRepository.AddAsync(Director, cancellationToken);
            return result;
        }
        public async Task<Dictionary<(string, string), Director>> EnsureDirectorsExistAsync(List<DirectorRequest> directors, CancellationToken cancellationToken)
        {
            var uniquePairs = directors
                .Select(d => (Name: d.Name.Trim(), Surname: d.Surname.Trim()))
                .Distinct()
                .ToList();
            var existingDirectors = await directorRepository.findByNames(uniquePairs, cancellationToken);
            var directorMap = existingDirectors.ToDictionary(
               d => (d.Name.Trim(), d.Surname.Trim()));
            foreach (var pair in uniquePairs)
            {
                if (!directorMap.ContainsKey(pair))
                {
                    var newDirector = Director.Create(pair.Name, pair.Surname);
                    await directorRepository.AddAsync(newDirector, cancellationToken);
                    directorMap.Add(pair, newDirector);
                }
            }
            return directorMap;
        }
    }
}
