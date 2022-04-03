namespace API.Interfaces;
public interface IUnitOfWork
{
    ITaskRepository TaskRepository { get; }
    IUserRepository UserRepository { get; }
    Task<bool> Complete();
    bool HasChanges();
}