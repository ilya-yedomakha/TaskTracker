using Microsoft.EntityFrameworkCore;
using tasktracker_3.DTO;

using tasktracker_3.Models;

namespace tasktracker_3.Interfaces
{
    public interface IProjectRepository
    {
        ICollection<Project> GetProjects();
        Project? GetProject(long id);
        ICollection<Project> GetProjects(string Name);
        bool ProjectExists(long id);
        bool AddProject(Project Project);
        bool UpdateProject(Project project);
        bool DeleteProject(Project project);
        ICollection<Worker>? GetProjectWorkers(long id);
        ICollection<TaskUnit>? GetProjectTasks(long id);

        public bool Save();
    }
}
