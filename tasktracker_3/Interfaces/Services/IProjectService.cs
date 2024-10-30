using tasktracker_3.DTO;
using tasktracker_3.Help.Result;
using tasktracker_3.Models;

namespace tasktracker_3.Interfaces.Services
{
    public interface IProjectService
    {
        Result<Project, ProjectDTO> GetProjects(string Name);
        Result<Project, ProjectDTO> AddProject(CreateProjectDTO projectDTO);
        Result<Project, ProjectDTO> UpdateProject(long id, CreateProjectDTO projectDTO);
        Result<Project, ProjectDTO> AddWorkerToProject(long projectId, long workerId);
        Result<Project, ProjectDTO> RemoveWorkerFromProject(long projectId, long workerId);
        Result<Project, ProjectDTO> GetProjectById(long id, bool includes);
        Result<Project, ProjectDTO> GetAllProjects(bool includes);
        bool ProjectExists(long id);
        Result<Project, ProjectDTO> AddTaskToProject(long projectId, long taskId);
        Result<Project, ProjectDTO> RemoveTaskFromProject(long projectId, long taskId);
        Result<Project, ProjectDTO> DeleteProject(long id);
        Result<Worker, WorkerDTO> GetProjectWorkers(long id);
        Result<TaskUnit, TaskUnitDTO> GetProjectTasks(long id);
    }
}
