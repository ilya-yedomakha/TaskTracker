using tasktracker_3.Models;

namespace tasktracker_3.Interfaces
{
    public interface ITaskUnitRepository
    {
        ICollection<TaskUnit> GetTasks(string Title);
        ICollection<Worker>? GetTaskWorkers(long id);
        Project? GetTaskProject(long id);

        ICollection<TaskUnit>? GetChildrenOfTask(long id);
        ICollection<TaskUnit>? GetParentsOfTask(long id);
    }
}
