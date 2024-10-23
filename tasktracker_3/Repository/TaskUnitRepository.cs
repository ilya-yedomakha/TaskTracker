using Microsoft.EntityFrameworkCore;
using tasktracker_3.Data;
using tasktracker_3.Interfaces;
using tasktracker_3.Models;


namespace tasktracker_3.Repository
{
    public class TaskUnitRepository : ITaskUnitRepository
    {
        private readonly DataContext _context;
        public TaskUnitRepository(DataContext context) {
            _context = context;
        }

        public bool AddTask(TaskUnit taskUnit)
        {
            _context.Add(taskUnit);
            return Save();
        }

        public bool DeleteTask(TaskUnit taskUnit)
        {
            _context.Remove(taskUnit);
            return Save();
        }

        public TaskUnit? GetTask(long id)
        {
            return _context.TaskUnits.Include(t => t.ChildOf).Include(t => t.ParentOf).Include(t => t.Workers).Include(t => t.Project).Where(t => t.Id == id).FirstOrDefault();
        }

        public Project? GetTaskProject(long id)
        {
            var t = _context.TaskUnits.Include(t => t.ChildOf).Include(t => t.ParentOf).Include(t => t.Project).Include(t => t.Workers).Where(t => t.Id == id).FirstOrDefault();

            if (t == null)
            {
                return null;
            }
            return t.Project;
        }

        public ICollection<TaskUnit> GetTasks(string Title)
        {
            return _context.TaskUnits.Include(t => t.ChildOf).Include(t => t.ParentOf).Include(t => t.Workers).Include(t => t.Project).OrderBy(t => t.Id).ToList();
        }

        public ICollection<TaskUnit> GetTasks()
        {
            return _context.TaskUnits.Include(t => t.ChildOf).Include(t => t.ParentOf).Include(t => t.Workers).Include(t => t.Project).OrderBy(t => t.Id).ToList();
        }

        public ICollection<Worker>? GetTaskWorkers(long id)
        {
            var t = _context.TaskUnits.Include(t => t.Workers).Where(t => t.Id == id).FirstOrDefault();

            if (t == null)
            {
                return null;
            }
            return t.Workers;
        }

        public bool TaskExists(long id)
        {
            return _context.TaskUnits.Any(e => e.Id == id);
        }

        public bool UpdateTask(TaskUnit taskUnit)
        {
            _context.Update(taskUnit);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public ICollection<TaskUnit>? GetChildrenOfTask(long id)
        {
            var t = _context.TaskUnits.Include(t => t.ChildOf).Include(t => t.ParentOf).Where(t => t.Id == id).FirstOrDefault();

            if (t == null)
            {
                return null;
            }
            return t.ParentOf;
        }

        public ICollection<TaskUnit>? GetParentsOfTask(long id)
        {
            var t = _context.TaskUnits.Include(t => t.ChildOf).Include(t => t.ParentOf).Where(t => t.Id == id).FirstOrDefault();

            if (t == null)
            {
                return null;
            }
            return t.ChildOf;
        }
    }
}
