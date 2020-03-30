using System.Linq;
using AutoMapper;
using DiplomaApp.API.Dtos;
using DiplomaApp.API.Models;

namespace DiplomaApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>()
                //photo url in list of users
                .ForMember(dest => dest.PhotoUrl, opt => 
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
                    //age
                .ForMember(dest => dest.Age, opt => 
                    opt.MapFrom(sourceMember=> sourceMember.DateOfBirth.CalculateAge()));


            CreateMap<User, UserForDetailedDto>()
            .ForMember(dest => dest.PhotoUrl, opt => 
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
            .ForMember(dest => dest.Age, opt => 
                    opt.MapFrom(sourceMember=> sourceMember.DateOfBirth.CalculateAge()));


            CreateMap<Photo, PhotosForDetailedDto>();
            CreateMap<UserForUpdateDto, User>();
            CreateMap<Photo, PhotoForReturnDto>();
            CreateMap<PhotoForCreationDto, Photo>();
            CreateMap<UserForRegisterDto, User>();
        }
    }
}