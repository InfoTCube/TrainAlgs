using System.Text;
using System.Text.RegularExpressions;
using CliWrap;
using CliWrap.Buffered;
using JudgeAPI.DTOs;
using JudgeAPI.Interfaces;

namespace JudgeAPI.Services;
public class CppService : ICppService
{
    public async Task<SolutionDto> RunCpp(AlgTaskDto algTask)
    {
        var ticks = DateTime.Now.Ticks;
        var guid = Guid.NewGuid().ToString();
        var uniqueId = ticks.ToString() +'-'+ guid.ToString();
        string path = $"./Runners/Cpp/{uniqueId}";

        using (StreamWriter sw = File.CreateText($"{path}.cpp"))
        {
            await sw.WriteAsync(algTask.Code);
            sw.Close();
        }

        SolutionDto solution = new SolutionDto();

        // compiling
        try
        {
            var result = await Cli.Wrap("g++")
            .WithArguments($"{path}.cpp -o {path}")
            .ExecuteBufferedAsync();
            Console.WriteLine($"{uniqueId} - {result.ExitCode} -- {result.ExitTime} -- {result.RunTime}");
        }
        catch(Exception e) // compiling error
        {
            File.Delete($"{path}.cpp");
            solution.Status = "Compiling Error";
            foreach (Match match in Regex.Matches(e.Message, @"(?<=cpp:)(.*)(?=\n)", RegexOptions.None))
                if(int.TryParse(match.Value.First().ToString(), out _)) // check if first char of match.Value is digit
                    solution.ErrorMessage += match.Value + '\n';

            solution.Points = 0;
            return solution;
        }

        // running code goes here
        try
        {
            solution.TestGroups = new List<TestGroupSolutionDto>();
            foreach(var tg in algTask.TestGroups) 
            {
                var testGroup = new TestGroupSolutionDto{ Number = tg.Number };
                testGroup.Tests = new List<TestSolutionDto>();

                foreach(var t in tg.Tests) 
                {
                    var outputBuilder = new StringBuilder();
                    var result = await (t.Input | Cli.Wrap($"{path}") | outputBuilder)
                    .ExecuteBufferedAsync();
                    string output = outputBuilder.ToString();
                    Console.WriteLine($"{uniqueId} - {output} -- {result.ExitCode} -- {result.ExitTime} -- {result.RunTime}");
                    var test = new TestSolutionDto { Number = t.Number };
                    if(output == t.Output) 
                    {
                        test.Status = "Ok";
                    }
                    else
                    {
                        test.Status = "Wrong Answer";
                        test.Error = $"{tg.Number}:{t.Number}: received: {output}, expected: '{t.Output}'";
                    }
                    testGroup.Tests?.Add(test);
                }

                if(testGroup.Tests.Count() == tg.Tests.Count())
                {
                    testGroup.Points = tg.Points;
                }

                solution.TestGroups.Add(testGroup);
            }
        }
        catch(Exception e)
        {
            System.Console.WriteLine(e.Message);
        }

        File.Delete(path); File.Delete($"{path}.cpp"); // clean trash

        return solution;
    }
}