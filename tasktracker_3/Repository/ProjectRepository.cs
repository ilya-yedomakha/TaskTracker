using Microsoft.EntityFrameworkCore;
using tasktracker_3.Data;
using tasktracker_3.Interfaces;
using tasktracker_3.Models;

namespace tasktracker_3.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly DataContext _context;
        public ProjectRepository(DataContext context) {
            _context = context;
        }

        public bool AddProject(Project Project)
        {
            _context.Projects.Add(Project);
            return Save();
        }

        public bool DeleteProject(Project project)
        {
            _context.Remove(project);
            return Save();
        }

        public Project? GetProject(long id)
        {
            return _context.Projects.Where(t => t.Id == id).Include(p => p.Tasks).Include(p => p.Workers).FirstOrDefault();
        }

        public ICollection<Project> GetProjects()
        {
            return _context.Projects.OrderBy(t => t.Id).Include(p => p.Tasks).Include(p => p.Workers).ToList();

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

        public bool ProjectExists(long id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }

        public bool UpdateProject(Project Project)
        {
            _context.Update(Project);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
