using System;
using AutoMapper;
using MovieMaster.Data.Dto;
using MovieMaster.Service.Model;
using MovieMaster.Service.ViewModel;

namespace MovieMaster.Service
{
    public sealed class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<MovieDto, Movie>();
            CreateMap<MovieDto, Movie>()
            .ForMember(d => d.MovieTitle, opt => opt.MapFrom(s => s.Title));

            CreateMap<Movie, MovieDto>();
            CreateMap<MovieViewModel, MovieDto>();


        }
    }
}