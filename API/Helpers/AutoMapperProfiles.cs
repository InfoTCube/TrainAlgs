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
        CreateMap<NewTestDto, Test>();
        CreateMap<Test, TestDto>();
        CreateMap<NewTestGroupDto, TestGroup>();
        CreateMap<TestGroup, TestGroupDto>();
        CreateMap<AlgTask, TaskDto>()
            .ForMember(user => user.AuthorUsername, opt => opt.MapFrom(src => 
                src.Author.UserName))
            .ForMember(task => task.ExampleTestGroup, opt => opt.MapFrom(src => 
                src.TestGroups.FirstOrDefault(tg => tg.Number == 0)));
        CreateMap<AlgTask, ListedTaskDto>()
            .ForMember(user => user.AuthorUsername, opt => opt.MapFrom(src => 
                src.Author.UserName));
        CreateMap<NewTaskDto, AlgTask>();
        CreateMap<TestSolution, TestSolutionDto>()
            .ForMember(test => test.MemoryLimit, opt => opt.MapFrom(src => 
                src.TestGroup.Solution.Task.TestGroups.FirstOrDefault(x => x.Number == src.TestGroup.Number)
                .Tests.FirstOrDefault(x => x.Number == src.Number).MemoryLimit))
            .ForMember(test => test.TimeLimit, opt => opt.MapFrom(src => 
                src.TestGroup.Solution.Task.TestGroups.FirstOrDefault(x => x.Number == src.TestGroup.Number)
                .Tests.FirstOrDefault(x => x.Number == src.Number).TimeLimit));
        CreateMap<TestGroupSolution, TestGroupSolutionDto>();
        CreateMap<Solution, SolutionDto>()
            .ForMember(solution => solution.AuthorUsername, opt => opt.MapFrom(src => 
                src.Author.UserName))
            .ForMember(solution => solution.AlgTaskTag, opt => opt.MapFrom(src => 
                src.Task.NameTag))
            .ForMember(solution => solution.AlgTaskName, opt => opt.MapFrom(src => 
                src.Task.Name));
        CreateMap<AlgTask, AlgTaskToTestDto>();
        CreateMap<TestGroup, TestGroupToTestDto>();
        CreateMap<Test, TestToTestDto>();
        CreateMap<Solution, ListedSolutionDto>()
            .ForMember(user => user.AuthorUsername, opt => opt.MapFrom(src => 
                src.Author.UserName))
            .ForMember(solution => solution.AlgTaskTag, opt => opt.MapFrom(src => 
                src.Task.NameTag));
    }
}
