namespace API.Interfaces;
public interface IUnitOfWork
{
    ITaskRepository TaskRepository { get; }
    IUserRepository UserRepository { get; }
    ISolutionRepository SolutionRepository { get; }
    ICompetitionRepository CompetitionRepository { get; }
    Task<bool> Complete();
    bool HasChanges();
}