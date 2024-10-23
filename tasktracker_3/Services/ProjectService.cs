using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using tasktracker_3.Interfaces;
using tasktracker_3.Interfaces.Services;
using tasktracker_3.Models;

namespace tasktracker_3.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IWorkerRepository _workerRepository;
        private readonly ITaskUnitRepository _taskUnitRepository;
        public ProjectService(ITaskUnitRepository taskUnitRepository, IWorkerRepository workerRepository, IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
            _workerRepository = workerRepository;
            _taskUnitRepository = taskUnitRepository;
        }

        public IActionResult AddProject(Project Project)
        {
            var ProjectName = _projectRepository.GetProjects().Where(p => p.Name.Trim().ToUpper() == Project.Name.Trim().ToUpper())
                .FirstOrDefault();

            if (ProjectName != null)
            {
                return new UnprocessableEntityObjectResult("Name " + Project.Name + " already exists");
            }
            if (Project == null)
            {
                return new NotFoundObjectResult("Project was not found");
            }
            else
            {
                if (!Project.Workers.IsNullOrEmpty())
                {
                    ICollection<Worker> wkr = Project.Workers;
                    ICollection<Worker> workers = new List<Worker>();
                    foreach (var worker in wkr)
                    {
                        var db_worker = _workerRepository.GetWorker(worker.Id);
                        if (db_worker == null)
                        {
                            return new NotFoundObjectResult("Worker with Id: " + worker.Id + " was not found");
                        }
                        else
                        {
                            workers.Add(db_worker);
                        }
                    }
                    Project.Workers = workers;
                }

                if (!Project.Tasks.IsNullOrEmpty())
                {
                    ICollection<TaskUnit> tsks_create = Project.Tasks;
                    ICollection<TaskUnit> taskUnits_new = new List<TaskUnit>();
                    foreach (var taskUnit in tsks_create)
                    {
                        var db_task = _taskUnitRepository.GetTask(taskUnit.Id);
                        if (db_task == null)
                        {
                            return new NotFoundObjectResult("Task with Id: " + taskUnit.Id + " was not found");

                        }
                        else
                        {
                            taskUnits_new.Add(db_task);
                        }
                    }
                    Project.Tasks = taskUnits_new;
                }
                if (_projectRepository.AddProject(Project))
                {
                    return new OkObjectResult("Success! Project added!");
                }

                return new BadRequestObjectResult("Something went wrong!");
            }
        }

        public IActionResult AddTaskToProject(long projectId, long taskId)
        {
            var task_db = _taskUnitRepository.GetTask(taskId);
            var project_db = _projectRepository.GetProject(projectId);


            if (task_db == null)
            {
                return new NotFoundObjectResult("Task was not found");
            }
            if (project_db == null)
            {
                return new NotFoundObjectResult("Project was not found");
            }

            project_db.Tasks.Add(task_db);


            if (_projectRepository.UpdateProject(project_db))
            {
                return new OkObjectResult("Success! Task was added to project!");
            }

            return new BadRequestObjectResult("Something went wrong!");
        }

        public IActionResult AddWorkerToProject(long projectId, long workerId)
        {
            var project_db = _projectRepository.GetProject(projectId);
            var worker_db = _workerRepository.GetWorker(workerId);

            if (worker_db == null)
            {
                return new NotFoundObjectResult("Worker was not found");
            }
            if (project_db == null)
            {
                return new NotFoundObjectResult("Project was not found");
            }

            project_db.Workers.Add(worker_db);


            if (_projectRepository.UpdateProject(project_db))
            {
                return new OkObjectResult("Success! Worker was added to project!");
            }

            return new BadRequestObjectResult("Something went wrong!");
        }

        public IActionResult DeleteProject(long id)
        {
            var Project = _projectRepository.GetProject(id);
            if (Project == null)
            {
                return new NotFoundObjectResult("Project was not found");
            }

            var projectWorkers = Project.Workers;

            foreach (var worker in projectWorkers)
            {
                worker.Projects.Remove(Project);
            }

            //Cascade deleting tasks
            if (_projectRepository.DeleteProject(Project))
            {
                return new OkObjectResult("Success! Project was deleted!");
            }

            return new BadRequestObjectResult("Something went wrong!");
        }

        public Project? GetProject(long id)
        {
            return _projectRepository.GetProject(id);
        }

        public ICollection<Project> GetProjects()
        {
            return _projectRepository.GetProjects();
        }

        public ICollection<Project> GetProjects(string Name)
        {
            return _projectRepository.GetProjects(Name);
        }

        public ICollection<TaskUnit>? GetProjectTasks(long id)
        {
            if (_projectRepository.ProjectExists(id))
            {
                return _projectRepository.GetProjectTasks(id);
            }
            return null;
        }

        public ICollection<Worker>? GetProjectWorkers(long id)
        {
            if (_projectRepository.ProjectExists(id))
            {
                return _projectRepository.GetProjectWorkers(id);
            }
            return null;
        }

        public bool ProjectExists(long id)
        {
            return _projectRepository.ProjectExists(id);
        }

        public IActionResult RemoveTaskFromProject(long projectId, long taskId)
        {
            var project_db = _projectRepository.GetProject(projectId);
            var task_db = _taskUnitRepository.GetTask(taskId);


            if (task_db == null)
            {
                return new NotFoundObjectResult("Task was not found");
            }
            if (project_db == null)
            {
                return new NotFoundObjectResult("Project was not found");
            }

            project_db.Tasks.Remove(task_db);


            if (_projectRepository.UpdateProject(project_db))
            {
                return new OkObjectResult("Success! Task was removed from project!");
            }

            return new BadRequestObjectResult("Something went wrong!");
        }

        public IActionResult RemoveWorkerFromProject(long projectId, long workerId)
        {
            var project_db = _projectRepository.GetProject(projectId);
            var worker_db = _workerRepository.GetWorker(workerId);


            if (worker_db == null)
            {
                return new NotFoundObjectResult("Worker was not found");
            }
            if (project_db == null)
            {
                return new NotFoundObjectResult("Project was not found");
            }

            project_db.Workers.Remove(worker_db);


            if (_projectRepository.UpdateProject(project_db))
            {
                return new OkObjectResult("Success! Task was removed from project!");
            }

            return new BadRequestObjectResult("Something went wrong!");
        }

        public IActionResult UpdateProject(long id, Project project)
        {
            var project_db = _projectRepository.GetProject(id);

            if (project == null || project_db == null)
            {
                return new NotFoundObjectResult("Project was not found");
            }
            else
            {
                project.Id = id;
                project_db.Name = project.Name;
                project_db.Description = project.Description;

                if (!project.Workers.IsNullOrEmpty())
                {
                    ICollection<Worker> wkr = project.Workers;
                    ICollection<Worker> workers = new List<Worker>();
                    foreach (var worker in wkr)
                    {
                        var db_worker = _workerRepository.GetWorker(worker.Id);
                        if (db_worker == null)
                        {
                            return new NotFoundObjectResult("Worker was not found");
                        }
                        else
                        {
                            workers.Add(db_worker);
                        }
                    }
                    project_db.Workers.Clear();
                    project_db.Workers = workers;
                }


                if (!project.Tasks.IsNullOrEmpty())
                {
                    ICollection<TaskUnit> tsks_create = project.Tasks;
                    ICollection<TaskUnit> taskUnits_new = new List<TaskUnit>();
                    foreach (var taskUnit in tsks_create)
                    {
                        var db_task = _taskUnitRepository.GetTask(taskUnit.Id);
                        if (db_task == null)
                        {
                            return new NotFoundObjectResult("Task was not found");
                        }
                        else
                        {
                            taskUnits_new.Add(db_task);
                        }
                    }
                    project.Tasks.Clear();
                    project_db.Tasks = taskUnits_new;
                }

                if (_projectRepository.UpdateProject(project_db))
                {
                    return new OkObjectResult("Success! Task was removed from project!");
                }

                return new BadRequestObjectResult("Something went wrong!");
            }
        }
    }
}
