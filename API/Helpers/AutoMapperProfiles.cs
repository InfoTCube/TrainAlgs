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
        CreateMap<TestDto, Test>();
        CreateMap<Test, TestDto>();
        CreateMap<TestGroupDto, TestGroup>();
        CreateMap<TestGroup, TestGroupDto>();
            //.ForMember(t => t.Tests, opt => opt.MapFrom(td => td.Tests));
        CreateMap<AlgTask, TaskDto>()
            .ForMember(user => user.AuthorUsername, opt => opt.MapFrom(src => 
                src.Author.UserName));
        CreateMap<TaskDto, AlgTask>();
    }
}
