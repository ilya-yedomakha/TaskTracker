
using Microsoft.EntityFrameworkCore;
using tasktracker_3.Models;

namespace tasktracker_3.Interfaces
{
    public interface ITaskUnitRepository
    {
        ICollection<TaskUnit> GetTasks();
        TaskUnit? GetTask(long id);
        ICollection<TaskUnit> GetTasks(string Title);
        bool TaskExists(long id);
        bool AddTask(TaskUnit taskUnit);
        bool UpdateTask(TaskUnit taskUnit);
        bool DeleteTask(TaskUnit taskUnit);
        ICollection<Worker>? GetTaskWorkers(long id);
        Project? GetTaskProject(long id);

        ICollection<TaskUnit>? GetChildrenOfTask(long id);
        ICollection<TaskUnit>? GetParentsOfTask(long id);

        public bool Save();
    }
}
