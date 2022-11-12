using AutoMapper;
using MovieAPIs.DTOs;

namespace MovieAPIs.Helper
{
    public class MappingProfile : Profile
    {
        // syntax of mapping between classes

        public MappingProfile() : base()    
        {
            // <base class  , class which we need map class to it >
           // by default its map each property to property with the same name in destnation file

            CreateMap<Movie, MovieDeatialsDto>();
        }

    }
}
