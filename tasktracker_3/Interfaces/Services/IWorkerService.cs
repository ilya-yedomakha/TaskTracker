using Microsoft.AspNetCore.Mvc;
using tasktracker_3.Models;

namespace tasktracker_3.Interfaces.Services
{
    public interface IWorkerService
    {
        ICollection<Worker> GetWorkers();
        Worker? GetWorker(long id);
        bool WorkerExists(long id);
        IActionResult AddWorker(Worker worker);
        IActionResult UpdateWorker(long id, Worker worker);
        IActionResult DeleteWorker(long id);

        IActionResult AddProjectToWorker(long workerId, long projectId);
        IActionResult RemoveProjectFromWorker(long workerId, long projectId);

        IActionResult AddTaskToWorker(long workerId, long projectId);
        IActionResult RemoveTaskFromWorker(long workerId, long taskId);
        ICollection<TaskUnit>? GetWorkerTasks(long id);
        ICollection<Project>? GetWorkerProjects(long id);
    }
}
