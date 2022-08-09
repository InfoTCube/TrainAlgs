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
                src.Author.UserName))
            .ForMember(task => task.Submissions, opt => opt.MapFrom(src => src.Solutions.Count))
            .ForMember(task => task.AverageResult, opt => opt.MapFrom(src => CalculateAverageResult(src.Solutions)))
            .ForMember(task => task.CorrectPercent, opt => opt.MapFrom(src => CalculatePercentOfCorrect(src.Solutions)));
        CreateMap<NewTaskDto, AlgTask>();
        CreateMap<TestSolution, TestSolutionDto>()
            .ForMember(test => test.MemoryLimit, opt => opt.MapFrom(src => 
                src.TestGroup.Solution.Task.TestGroups.FirstOrDefault(x => x.Number == src.TestGroup.Number)
                .Tests.FirstOrDefault(x => x.Number == src.Number).MemoryLimit))
            .ForMember(test => test.TimeLimit, opt => opt.MapFrom(src => 
                src.TestGroup.Solution.Task.TestGroups.FirstOrDefault(x => x.Number == src.TestGroup.Number)
                .Tests.FirstOrDefault(x => x.Number == src.Number).TimeLimit));
        CreateMap<TestGroupSolution, TestGroupSolutionDto>()
            .ForMember(testGroup => testGroup.MaxPoints, opt => opt.MapFrom(src => 
                src.Solution.Task.TestGroups.FirstOrDefault(x => x.Number == src.Number).Points));
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

    private static short CalculateAverageResult(IEnumerable<Solution> solutions)
    {
        long sum = solutions.Sum(s => s.Points);
        if(sum == 0) return 0;
        return (short)(sum / solutions.Count());
    }

    private static short CalculatePercentOfCorrect(IEnumerable<Solution> solutions)
    {
        int correct = solutions.Where(s => s.Points == 100).Count();
        if(correct == 0) return 0;
        return (short)((correct*100 / solutions.Count()));
    }
}
