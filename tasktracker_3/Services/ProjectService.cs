using Microsoft.IdentityModel.Tokens;
using tasktracker_3.Help.Result;
using tasktracker_3.Help.Result.ModelErrors;
using tasktracker_3.Interfaces;
using tasktracker_3.Interfaces.Services;
using tasktracker_3.Models;
using tasktracker_3.Repository.Base;
using tasktracker_3.Services.Base;

namespace tasktracker_3.Services
{
    public class ProjectService : BaseService<Project>, IProjectService
    {

        public ProjectService(
            BaseRepository<Project> baseRepository,
            BaseRepository<TaskUnit> taskRepositoryBase,
            BaseRepository<Project> projectRepositoryBase,
            BaseRepository<Worker> workerRepositoryBase,
            ITaskUnitRepository taskUnitRepository, IProjectRepository projectRepository, IWorkerRepository workerRepository)
        : base(baseRepository, taskRepositoryBase, projectRepositoryBase, workerRepositoryBase, taskUnitRepository, workerRepository, projectRepository) { }

        public Result<Project> AddProject(Project Project)
        {
            if (Project == null)
            {
                return Result<Project>.Failure(ModelError<Project>.NullReference);
            }
            else
            {
                var ProjectName = _projectRepositoryBase.GetAll(false).Where(p => p.Name.Trim().ToUpper() == Project.Name.Trim().ToUpper()).FirstOrDefault();

                if (ProjectName != null)
                {
                    return Result<Project>.Failure(ModelError<Project>.SameTitle(Project.Name));
                }

                if (!Project.Workers.IsNullOrEmpty())
                {
                    ICollection<Worker> wkr = Project.Workers;
                    ICollection<Worker> workers = new List<Worker>();
                    foreach (var worker in wkr)
                    {
                        var db_worker = _workerRepositoryBase.GetById(worker.Id, true);
                        if (db_worker == null)
                        {
                            return Result<Project>.Failure(ModelError<Worker>.NotFound(worker.Id));
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
                        var db_task = _taskRepositoryBase.GetById(taskUnit.Id, true);
                        if (db_task == null)
                        {
                            return Result<Project>.Failure(ModelError<TaskUnit>.NotFound(taskUnit.Id));

                        }
                        else
                        {
                            taskUnits_new.Add(db_task);
                        }
                    }
                    Project.Tasks = taskUnits_new;
                }
                if (_projectRepositoryBase.AddModel(Project))
                {
                    return Result<Project>.Success();
                }

                return Result<Project>.Failure(ModelError<Project>.ServerError);
            }
        }

        public Result<Project> AddTaskToProject(long projectId, long taskId)
        {
            var task_db = _taskRepositoryBase.GetById(taskId, false);
            var project_db = _projectRepositoryBase.GetById(projectId, false);


            if (task_db == null)
            {
                return Result<Project>.Failure(ModelError<TaskUnit>.NotFound(taskId));
            }
            if (project_db == null)
            {
                return Result<Project>.Failure(ModelError<Project>.NotFound(projectId));
            }

            project_db.Tasks.Add(task_db);


            if (_projectRepositoryBase.Update(project_db))
            {
                return Result<Project>.Success();
            }

            return Result<Project>.Failure(ModelError<Project>.ServerError);
        }

        public Result<Project> AddWorkerToProject(long projectId, long workerId)
        {
            var project_db = _projectRepositoryBase.GetById(projectId, true);
            var worker_db = _workerRepositoryBase.GetById(workerId, true);

            if (worker_db == null)
            {
                return Result<Project>.Failure(ModelError<Worker>.NotFound(workerId));
            }
            if (project_db == null)
            {
                return Result<Project>.Failure(ModelError<Project>.NotFound(projectId));
            }

            project_db.Workers.Add(worker_db);


            if (_projectRepositoryBase.Update(project_db))
            {
                return Result<Project>.Success();
            }

            return Result<Project>.Failure(ModelError<Project>.ServerError);
        }

        public Result<Project> DeleteProject(long id)
        {
            var Project = _projectRepositoryBase.GetById(id, true);
            if (Project == null)
            {
                return Result<Project>.Failure(ModelError<Project>.NotFound(id));
            }

            var projectWorkers = Project.Workers;

            foreach (var worker in projectWorkers)
            {
                worker.Projects.Remove(Project);
            }

            //Cascade deleting tasks
            if (_projectRepositoryBase.DeleteModel(Project))
            {
                return Result<Project>.Success();
            }

            return Result<Project>.Failure(ModelError<Project>.ServerError);
        }


        public Result<Project> GetProjects(string Name)
        {
            var projects = _projectRepository.GetProjects(Name);
            var res = Result<Project>.Success();
            res.Models = projects.ToList();
            return res;
        }

        public Result<TaskUnit> GetProjectTasks(long id)
        {
            var tasks = _projectRepository.GetProjectTasks(id);
            if (tasks != null)
            {
                var res = Result<TaskUnit>.Success();
                res.Models = tasks.ToList();
                return res;
            }
            else return Result<TaskUnit>.Failure(ModelError<Project>.NotFound(id));
        }

        public Result<Worker> GetProjectWorkers(long id)
        {
            var workers = _projectRepository.GetProjectWorkers(id);
            if (workers != null)
            {
                var res = Result<Worker>.Success();
                res.Models = workers.ToList();
                return res;
            }
            else return Result<Worker>.Failure(ModelError<Project>.NotFound(id));
        }


        public Result<Project> RemoveTaskFromProject(long projectId, long taskId)
        {
            var project_db = _projectRepositoryBase.GetById(projectId, true);
            var task_db = _taskRepositoryBase.GetById(taskId, true);


            if (task_db == null)
            {
                return Result<Project>.Failure(ModelError<TaskUnit>.NotFound(taskId));
            }
            if (project_db == null)
            {
                return Result<Project>.Failure(ModelError<Project>.NotFound(projectId));
            }

            project_db.Tasks.Remove(task_db);


            if (_projectRepositoryBase.Update(project_db))
            {
                return Result<Project>.Success();
            }

            return Result<Project>.Failure(ModelError<Project>.ServerError);
        }

        public Result<Project> RemoveWorkerFromProject(long projectId, long workerId)
        {
            var project_db = _projectRepositoryBase.GetById(projectId, true);
            var worker_db = _workerRepositoryBase.GetById(workerId, true);


            if (worker_db == null)
            {
                return Result<Project>.Failure(ModelError<Worker>.NotFound(workerId));
            }
            if (project_db == null)
            {
                return Result<Project>.Failure(ModelError<Project>.NotFound(projectId));
            }

            project_db.Workers.Remove(worker_db);


            if (_projectRepositoryBase.Update(project_db))
            {
                return Result<Project>.Success();
            }

            return Result<Project>.Failure(ModelError<Project>.ServerError);
        }

        public Result<Project> UpdateProject(long id, Project project)
        {
            var project_db = _projectRepositoryBase.GetById(id, true);

            if (project == null)
            {
                return Result<Project>.Failure(ModelError<Project>.NullReference);
            }
            if (project_db == null)
            {
                return Result<Project>.Failure(ModelError<Project>.NotFound(id));
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
                        var db_worker = _workerRepositoryBase.GetById(worker.Id, true);
                        if (db_worker == null)
                        {
                            return Result<Project>.Failure(ModelError<Worker>.NotFound(worker.Id));
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
                        var db_task = _taskRepositoryBase.GetById(taskUnit.Id, false);
                        if (db_task == null)
                        {
                            return Result<Project>.Failure(ModelError<TaskUnit>.NotFound(taskUnit.Id));
                        }
                        else
                        {
                            taskUnits_new.Add(db_task);
                        }
                    }
                    project.Tasks.Clear();
                    project_db.Tasks = taskUnits_new;
                }

                if (_projectRepositoryBase.Update(project_db))
                {
                    return Result<Project>.Success();
                }

                return Result<Project>.Failure(ModelError<Project>.ServerError);
            }
        }
    }
}
