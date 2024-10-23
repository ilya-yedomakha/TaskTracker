using Microsoft.AspNetCore.Mvc;
using tasktracker_3.Models;

namespace tasktracker_3.Interfaces.Services
{
    public interface ITaskUnitService
    {
        ICollection<TaskUnit> GetTasks();
        TaskUnit? GetTask(long id);
        ICollection<TaskUnit> GetTasks(string Title);
        bool TaskExists(long id);
        IActionResult AddTask(TaskUnit taskUnit);
        IActionResult UpdateTask(long id, TaskUnit taskUnit);
        IActionResult AddWorkerToTask(long taskId, long workerId);
        IActionResult AddChildTaskToTask(long parentTaskId, long childTaskId);
        IActionResult RemoveChildTaskFromTask(long parentTaskId, long childTaskId);

        //IActionResult AddParentTaskToTask(long TaskId, long parentTaskId);
        //IActionResult RemoveParentTaskFromTask(long TaskId, long parentTaskId);

        ICollection<TaskUnit>? GetChildrenOfTask(long id);
        ICollection<TaskUnit>? GetParentsOfTask(long id);
        IActionResult RemoveWorkerFromTask(long taskId, long workerId);
        IActionResult DeleteTask(long id);
        ICollection<Worker>? GetTaskWorkers(long id);
        Project? GetTaskProject(long id);
    }
}
