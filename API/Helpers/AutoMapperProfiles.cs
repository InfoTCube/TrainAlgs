using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using System.Security.Claims;
using static API.Helpers.TaskParams;

namespace API.Helpers;
public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<RegisterDto, AppUser>();
        CreateMap<AppUser, MemberDto>()
            .ForMember(user => user.GraphChart, opt => opt.MapFrom(src => GetSolutions(src.Solutions, src.UserName)));
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

    private static GraphChartDto? GetSolutions(IEnumerable<Solution> solutions, string username)
    {
        GraphChartDto graphChart = new GraphChartDto();
        List<Tuple<string, int>>? solutionsCount = new List<Tuple<string, int>>();
        int solvedAllTime = 0;
        int solvedLastYear = 0;
        int solvedLastMonth = 0;
        int solvedInRowAllTime = 0;
        int solvedInRowLastYear = 0;
        int solvedInRowLastMonth = 0;

        solutions = solutions.Where(s => s.Author?.UserName == username).Where(s => s.Points == 100);
        DateTime day = DateTime.Today;
        int currentRow = 0;
        while(solutions.Count() != solvedAllTime)
        {
            int count = solutions
                .Where(s => s.Date.ToString("dd/MM/yyyy") == day.ToString("dd/MM/yyyy"))
                .Count();

            if(count > 0) solutionsCount.Add(new Tuple<string, int>(day.ToString("dd/MM/yyyy"), count));

            if(DateTime.Today.AddDays(-30) < day) solvedLastMonth += count;
            if(DateTime.Today.AddDays(-365) < day) solvedLastYear += count;

            if(DateTime.Today.AddDays(-30).ToString("dd/MM/yyyy") == day.ToString("dd/MM/yyyy")) solvedInRowLastMonth = currentRow > solvedInRowLastMonth ? currentRow : solvedInRowLastMonth;
            if(DateTime.Today.AddDays(-365).ToString("dd/MM/yyyy") == day.ToString("dd/MM/yyyy")) solvedInRowLastYear = currentRow > solvedInRowLastYear ? currentRow : solvedInRowLastYear;

            if(count > 0) ++currentRow;
            else
            {
                if(DateTime.Today.AddDays(-30) < day) solvedInRowLastMonth = currentRow > solvedInRowLastMonth ? currentRow : solvedInRowLastMonth;
                if(DateTime.Today.AddDays(-365) < day) solvedInRowLastYear = currentRow > solvedInRowLastYear ? currentRow : solvedInRowLastYear;
                solvedInRowAllTime = currentRow > solvedInRowAllTime ? currentRow : solvedInRowAllTime;
                currentRow = 0;
            }

            solvedAllTime += count;
            day = day.AddDays(-1);
        }

        graphChart = new GraphChartDto
        {
            Solutions = solutionsCount,
            SolvedAllTime = solvedAllTime,
            SolvedLastYear = solvedLastYear,
            SolvedLastMonth = solvedLastMonth,
            SolvedInRowAllTime = solvedInRowAllTime,
            SolvedInRowLastYear = solvedInRowLastYear,
            SolvedInRowLastMonth = solvedInRowLastMonth
        };

        return graphChart;
    }
}
