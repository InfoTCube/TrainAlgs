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
            solution.Status = "Compilation error";
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
                    string error = string.Empty;
                    double time = 0;
                    using var cts = new CancellationTokenSource();
                    var outputBuilder = new StringBuilder();
                    cts.CancelAfter(TimeSpan.FromMilliseconds(t.TimeLimit));
                    try
                    {
                        var result = await (t.Input | Cli.Wrap($"{path}") | outputBuilder)
                            .ExecuteBufferedAsync(cts.Token);
                        Console.WriteLine($"{uniqueId} - {outputBuilder.ToString()} -- {result.ExitCode} -- {result.ExitTime} -- {result.RunTime}");
                        time = result.RunTime.TotalMilliseconds;
                    }
                    catch(Exception e)
                    {
                        if(e.Message.Substring(Math.Max(0, e.Message.Length - 27)) == "was killed by user request.") // check for TLE
                            error = "TLE";
                        else
                            error = "RE";
                        
                        System.Console.WriteLine(e.Message);
                    }
                    
                    string output = outputBuilder.ToString().Trim();

                    var test = new TestSolutionDto { Number = t.Number };
                    test.Time = (int)time;
                    if(output == t.Output) 
                    {
                        test.Status = "Ok";
                    }
                    else if(error == "TLE")
                    {
                        test.Status = "Time limit exceeded";
                    }
                    else if(error == "RE")
                    {
                        test.Status = "Runtime error";
                    }
                    else
                    {
                        test.Status = "Wrong answer";
                        test.Error = $"{tg.Number}.{t.Number}: received: '{output}', expected: '{t.Output}'";
                    }
                    testGroup.Tests?.Add(test);
                }

                if(testGroup.Tests.Where(s => s.Status == "Ok").Count() == tg.Tests.Count()) // give points if all tests from group succeed
                    testGroup.Points = tg.Points;

                solution.TestGroups.Add(testGroup);
            }
        }
        catch(Exception e)
        {
            System.Console.WriteLine(e.Message);
        }

        File.Delete(path); File.Delete($"{path}.cpp"); // clean trash

        solution.Points = solution.TestGroups.Sum(x => x.Points);
        solution.Status = "Preliminary checking: ";
        if(solution.TestGroups.Where(n => n.Number == 0)
            .FirstOrDefault().Tests
            .Where(s => s.Status == "Ok").Count() == solution.TestGroups.Where(n => n.Number == 0).FirstOrDefault().Tests.Count())
        {
            solution.Status += "Ok";
        }
        else
        {
            solution.Status += "Error";
        }

        return solution;
    }
}