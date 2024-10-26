using tasktracker_3.Help.Result;
using tasktracker_3.Models;

namespace tasktracker_3.Interfaces.Services
{
    public interface IWorkerService
    {
        Result<Worker> AddWorker(Worker worker);
        Result<Worker> UpdateWorker(long id, Worker worker);
        Result<Worker> DeleteWorker(long id);

        Result<Worker> AddProjectToWorker(long workerId, long projectId);
        Result<Worker> RemoveProjectFromWorker(long workerId, long projectId);

        Result<Worker> AddTaskToWorker(long workerId, long projectId);
        Result<Worker> RemoveTaskFromWorker(long workerId, long taskId);
        Result<TaskUnit> GetWorkerTasks(long id);
        Result<Project> GetWorkerProjects(long id);
    }
}
