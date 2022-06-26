namespace JudgeAPI.Interfaces;
public interface ICppService
{
    Task<string> CompileCpp(string code);
}