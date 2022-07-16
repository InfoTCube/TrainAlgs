using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CliWrap;
using CliWrap.Buffered;
using JudgeAPI.DTOs;
using JudgeAPI.Interfaces;

namespace JudgeAPI.Services;

public class PythonService : IPythonService
{
    public async Task<SolutionDto> RunPython(AlgTaskDto algTask)
    {
        var ticks = DateTime.Now.Ticks;
        var guid = Guid.NewGuid().ToString();
        var uniqueId = ticks.ToString() +'-'+ guid.ToString();
        string path = $"./Runners/Python/{uniqueId}";

        using (StreamWriter sw = File.CreateText($"{path}.py"))
        {
            await sw.WriteAsync(algTask.Code);
            sw.Close();
        }

        SolutionDto solution = new SolutionDto();

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
                        Console.WriteLine(t.Input);
                        var result = await (t.Input.Replace(' ', '\n') | Cli.Wrap("python3") | outputBuilder)
                            .WithArguments($"{path}.py")
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

        File.Delete($"{path}.py"); // clean trash

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
