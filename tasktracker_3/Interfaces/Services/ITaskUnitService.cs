using tasktracker_3.Help.Result;
using tasktracker_3.Models;

namespace tasktracker_3.Interfaces.Services
{
    public interface ITaskUnitService
    {
        Result<TaskUnit> GetTasks(string Title);
        Result<TaskUnit> AddTask(TaskUnit taskUnit);
        Result<TaskUnit> UpdateTask(long id, TaskUnit taskUnit);
        Result<TaskUnit> AddWorkerToTask(long taskId, long workerId);
        Result<TaskUnit> AddChildTaskToTask(long parentTaskId, long childTaskId);
        Result<TaskUnit> RemoveChildTaskFromTask(long parentTaskId, long childTaskId);

        Result<TaskUnit> GetChildrenOfTask(long id);
        Result<TaskUnit> GetParentsOfTask(long id);
        Result<TaskUnit> RemoveWorkerFromTask(long taskId, long workerId);
        Result<TaskUnit> DeleteTask(long id);
        Result<Worker> GetTaskWorkers(long id);
        Result<Project> GetTaskProject(long id);
    }
}
