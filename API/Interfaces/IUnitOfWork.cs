namespace API.Interfaces;
public interface IUnitOfWork
{
    ITaskRepository TaskRepository { get; }
    IUserRepository UserRepository { get; }
    ISolutionRepository SolutionRepository { get; }
    Task<bool> Complete();
    bool HasChanges();
}