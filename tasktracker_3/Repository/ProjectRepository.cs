using Microsoft.EntityFrameworkCore;
using tasktracker_3.Data;
using tasktracker_3.Interfaces;
using tasktracker_3.Models;
using tasktracker_3.Repository.Base;

namespace tasktracker_3.Repository
{
    public class ProjectRepository : BaseRepository<Project>, IProjectRepository
    {
        public ProjectRepository(DataContext context) : base(context)
        {
        }


        public ICollection<Project> GetProjects(string Name)
        {
            return _context.Projects.Where(p => p.Name == Name).Include(p => p.Tasks).Include(p => p.Workers).OrderBy(t => t.Id).ToList();

        }

        public ICollection<TaskUnit>? GetProjectTasks(long id)
        {
            var p = _context.Projects.Where(t => t.Id == id).Include(p => p.Tasks).FirstOrDefault();

            if (p == null)
            {
                return null;
            }
            return p.Tasks;
        }

        public ICollection<Worker>? GetProjectWorkers(long id)
        {
            var p = _context.Projects.Where(t => t.Id == id).Include(p=>p.Workers).FirstOrDefault();

            if (p == null)
            {
                return null;
            }
            return p.Workers;
        }
    }
}
