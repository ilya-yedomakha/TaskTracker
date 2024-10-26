using Microsoft.EntityFrameworkCore;
using tasktracker_3.Data;
using tasktracker_3.Interfaces;
using tasktracker_3.Models;
using tasktracker_3.Repository.Base;

namespace tasktracker_3.Repository
{
    public class WorkerRepository : BaseRepository<Worker>, IWorkerRepository
    {
        public WorkerRepository(DataContext context) : base(context)
        {
        }

        public bool AddWorker(Worker worker)
        {
            _context.Workers.Add(worker);
            return Save(_context);
        }

        public bool DeleteWorker(Worker worker)
        {
            _context.Remove(worker);
            return Save(_context);
        }

        public Worker? GetWorker(long id)
        {
            return _context.Workers.Where(t => t.Id == id).Include(w => w.Tasks).Include(w => w.Projects).FirstOrDefault();

        }

        public ICollection<Project>? GetWorkerProjects(long id)
        {
            var w = _context.Workers.Where(w => w.Id == id).Include(w => w.Projects).FirstOrDefault();

            if (w == null)
            {
                return null;
            }
            return w.Projects;
        }

        public ICollection<Worker> GetWorkers()
        {
            return _context.Workers.OrderBy(t => t.Id).Include(w => w.Tasks).Include(w => w.Projects).ToList();

        }

        public ICollection<TaskUnit>? GetWorkerTasks(long id)
        {
            var w = _context.Workers.Where(w => w.Id == id).Include(w => w.Tasks).FirstOrDefault();

            if (w == null)
            {
                return null;
            }
            return w.Tasks;
        }

        public bool UpdateWorker(Worker worker)
        {
            _context.Update(worker);
            return Save(_context);
        }

        public bool WorkerExists(long id)
        {
            return _context.Workers.Any(e => e.Id == id);
        }
    }
}
