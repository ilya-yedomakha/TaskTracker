using tasktracker_3.DTO;
using tasktracker_3.Help.Result;
using tasktracker_3.Models;

namespace tasktracker_3.Interfaces.Services
{
    public interface IWorkerService
    {
        Result<Worker, WorkerDTO> AddWorker(CreateWorkerDTO workerDTO);
        Result<Worker, WorkerDTO> UpdateWorker(long id, CreateWorkerDTO workerDTO);
        Result<Worker, WorkerDTO> DeleteWorker(long id);

        Result<Worker, WorkerDTO> AddProjectToWorker(long workerId, long projectId);
        Result<Worker, WorkerDTO> RemoveProjectFromWorker(long workerId, long projectId);

        Result<Worker, WorkerDTO> AddTaskToWorker(long workerId, long projectId);
        Result<Worker, WorkerDTO> RemoveTaskFromWorker(long workerId, long taskId);
        Result<TaskUnit, TaskUnitDTO> GetWorkerTasks(long id);
        Result<Project, ProjectDTO> GetWorkerProjects(long id);
    }
}
