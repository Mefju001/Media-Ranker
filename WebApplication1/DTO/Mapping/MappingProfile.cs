using AutoMapper;
using WebApplication1.DTO.Response;
using WebApplication1.Models;

namespace WebApplication1.DTO.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Movie, MovieResponse>()
            .ConstructUsing(src => new MovieResponse(
                src.title,
                src.description,
                new GenreResponse(src.genre.name),
                new DirectorResponse(src.director.name, src.director.surname),
                src.ReleaseDate,
                src.Language,
                src.Reviews.Select(r => new ReviewResponse(
                    src.title,             // mediaName
                    r.User.username,      // username
                    r.Rating,
                    r.Comment
                )).ToList(),
                src.Duration,
                src.IsCinemaRelease
            ));

            // Mapowanie Review -> ReviewResponse (opcjonalne, jeśli chcesz też osobno)
            CreateMap<Review, ReviewResponse>()
                .ConstructUsing(r => new ReviewResponse(
                    r.Media.title,
                    r.User.username,
                    r.Rating,
                    r.Comment
                ));
            CreateMap<Media, MediaResponse>();
            CreateMap<Genre, GenreResponse>();
            CreateMap<Director, DirectorResponse>();
        }
    }
}
