using Microsoft.EntityFrameworkCore;
using tasktracker_3.Data;
using tasktracker_3.Interfaces;
using tasktracker_3.Models;
using tasktracker_3.Repository.Base;


namespace tasktracker_3.Repository
{
    public class TaskUnitRepository : BaseRepository<TaskUnit>, ITaskUnitRepository
    {
        public TaskUnitRepository(DataContext context) : base(context) {
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

        public ICollection<Worker>? GetTaskWorkers(long id)
        {
            var t = _context.TaskUnits.Include(t => t.Workers).Where(t => t.Id == id).FirstOrDefault();

            if (t == null)
            {
                return null;
            }
            return t.Workers;
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
