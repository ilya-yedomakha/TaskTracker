using tasktracker_3.Help.Result;
using tasktracker_3.Models;

namespace tasktracker_3.Interfaces.Services
{
    public interface IProjectService
    {
        Result<Project> GetProjects(string Name);
        Result<Project> AddProject(Project Project);
        Result<Project> UpdateProject(long id, Project Project);
        Result<Project> AddWorkerToProject(long projectId, long workerId);
        Result<Project> RemoveWorkerFromProject(long projectId, long workerId);

        Result<Project> AddTaskToProject(long projectId, long taskId);
        Result<Project> RemoveTaskFromProject(long projectId, long taskId);
        Result<Project> DeleteProject(long id);
        Result<Worker> GetProjectWorkers(long id);
        Result<TaskUnit> GetProjectTasks(long id);
    }
}
