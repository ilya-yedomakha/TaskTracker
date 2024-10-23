using Microsoft.AspNetCore.Mvc;
using tasktracker_3.Models;

namespace tasktracker_3.Interfaces.Services
{
    public interface IProjectService
    {
        ICollection<Project> GetProjects();
        Project? GetProject(long id);
        ICollection<Project> GetProjects(string Name);
        bool ProjectExists(long id);
        IActionResult AddProject(Project Project);
        IActionResult UpdateProject(long id, Project Project);
        IActionResult AddWorkerToProject(long projectId, long workerId);
        IActionResult RemoveWorkerFromProject(long projectId, long workerId);

        IActionResult AddTaskToProject(long projectId, long taskId);
        IActionResult RemoveTaskFromProject(long projectId, long taskId);
        IActionResult DeleteProject(long id);
        ICollection<Worker>? GetProjectWorkers(long id);
        ICollection<TaskUnit>? GetProjectTasks(long id);
    }
}
