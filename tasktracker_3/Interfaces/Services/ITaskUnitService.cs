using tasktracker_3.DTO;
using tasktracker_3.Help.Result;
using tasktracker_3.Models;

namespace tasktracker_3.Interfaces.Services
{
    public interface ITaskUnitService
    {
        Result<TaskUnit, TaskUnitDTO> GetTasks(string Title);
        Result<TaskUnit, TaskUnitDTO> AddTask(CreateTaskUnitDTO taskUnitDTO);
        Result<TaskUnit, TaskUnitDTO> UpdateTask(long id, CreateTaskUnitDTO taskUnitDTO);
        Result<TaskUnit, TaskUnitDTO> AddWorkerToTask(long taskId, long workerId);
        Result<TaskUnit, TaskUnitDTO> AddChildTaskToTask(long parentTaskId, long childTaskId);
        Result<TaskUnit, TaskUnitDTO> RemoveChildTaskFromTask(long parentTaskId, long childTaskId);
        Result<TaskUnit, TaskUnitDTO> GetTaskUnitById(long id, bool includes);
        Result<TaskUnit, TaskUnitDTO> GetAllTasks(bool includes);
        bool TaskExists(long id);
        Result<TaskUnit, TaskUnitDTO> GetChildrenOfTask(long id);
        Result<TaskUnit, TaskUnitDTO> GetParentsOfTask(long id);
        Result<TaskUnit, TaskUnitDTO> RemoveWorkerFromTask(long taskId, long workerId);
        Result<TaskUnit, TaskUnitDTO> DeleteTask(long id);
        Result<Worker, WorkerDTO> GetTaskWorkers(long id);
        Result<Project, ProjectDTO> GetTaskProject(long id);
    }
}
