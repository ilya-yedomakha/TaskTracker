
using Microsoft.EntityFrameworkCore;
using tasktracker_3.Models;

namespace tasktracker_3.Interfaces
{
    public interface IWorkerRepository
    {
        ICollection<Worker> GetWorkers();
        Worker? GetWorker(long id);
        bool WorkerExists(long id);
        bool AddWorker(Worker worker);
        bool UpdateWorker(Worker worker);
        bool DeleteWorker(Worker worker);
        ICollection<TaskUnit>? GetWorkerTasks(long id);
        ICollection<Project>? GetWorkerProjects(long id);

        public bool Save();
    }
}
