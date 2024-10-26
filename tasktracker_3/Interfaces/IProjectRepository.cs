using tasktracker_3.Models;

namespace tasktracker_3.Interfaces
{
    public interface IProjectRepository
    {
        ICollection<Project> GetProjects(string Name);
        ICollection<Worker>? GetProjectWorkers(long id);
        ICollection<TaskUnit>? GetProjectTasks(long id);
    }
}
