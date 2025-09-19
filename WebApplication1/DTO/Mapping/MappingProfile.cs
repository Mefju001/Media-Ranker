using AutoMapper;
using WebApplication1.DTO.Response;
using WebApplication1.Models;

namespace WebApplication1.DTO.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Review, ReviewResponse>()
                .ForMember(
                    dest => dest.username,
                    opt => opt.MapFrom(src => src.User != null ? src.User.name : "Anonimowy")
                )
                .ForMember(
                    dest => dest.mediaName,
                    opt => opt.MapFrom(src => src.Media != null ? src.Media.title : "Nieznane medium")
                );


            CreateMap<Media, MediaResponse>();



            CreateMap<Movie, MovieResponse>();

            CreateMap<Genre, GenreResponse>();
            CreateMap<Director, DirectorResponse>();
        }
    }
}
