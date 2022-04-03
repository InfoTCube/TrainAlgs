using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Helpers;
public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<RegisterDto, AppUser>();
        CreateMap<AppUser, MemberDto>();
        CreateMap<AlgTask, TaskDto>()
            .ForMember(user => user.AuthorUsername, opt => opt.MapFrom(src => 
                src.Author.UserName));
    }
}
