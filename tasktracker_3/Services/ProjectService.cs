using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using tasktracker_3.DTO;
using tasktracker_3.Help.Result;
using tasktracker_3.Help.Result.ModelErrors;
using tasktracker_3.Interfaces;
using tasktracker_3.Interfaces.Services;
using tasktracker_3.Models;
using tasktracker_3.Repository.Base;
using tasktracker_3.Services.Base;

namespace tasktracker_3.Services
{
    public class ProjectService : BaseService<Project, ProjectDTO>, IProjectService
    {

        public ProjectService(IMapper mapper,
            BaseRepository<Project> baseRepository,
            BaseRepository<TaskUnit> taskRepositoryBase,
            BaseRepository<Project> projectRepositoryBase,
            BaseRepository<Worker> workerRepositoryBase,
            ITaskUnitRepository taskUnitRepository, IProjectRepository projectRepository, IWorkerRepository workerRepository)
        : base(mapper, baseRepository, taskRepositoryBase, projectRepositoryBase, workerRepositoryBase, taskUnitRepository, workerRepository, projectRepository) { }

        public Result<Project, ProjectDTO> AddProject(CreateProjectDTO ProjectDTO)
        {
            var Project = _mapper.Map<Project>(ProjectDTO);
            if (Project == null)
            {
                return Result<Project, ProjectDTO>.Failure(ModelError<Project>.NullReference);
            }
            else
            {
                var ProjectName = _projectRepositoryBase.GetAll(false).Where(p => p.Name.Trim().ToUpper() == Project.Name.Trim().ToUpper()).FirstOrDefault();

                if (ProjectName != null)
                {
                    return Result<Project, ProjectDTO>.Failure(ModelError<Project>.SameTitle(Project.Name));
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
                            return Result<Project, ProjectDTO>.Failure(ModelError<Worker>.NotFound(worker.Id));
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
                            return Result<Project, ProjectDTO>.Failure(ModelError<TaskUnit>.NotFound(taskUnit.Id));

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
                    return Result<Project, ProjectDTO>.Success();
                }

                return Result<Project, ProjectDTO>.Failure(ModelError<Project>.ServerError);
            }
        }

        public Result<Project, ProjectDTO> AddTaskToProject(long projectId, long taskId)
        {
            var task_db = _taskRepositoryBase.GetById(taskId, false);
            var project_db = _projectRepositoryBase.GetById(projectId, false);


            if (task_db == null)
            {
                return Result<Project, ProjectDTO>.Failure(ModelError<TaskUnit>.NotFound(taskId));
            }
            if (project_db == null)
            {
                return Result<Project, ProjectDTO>.Failure(ModelError<Project>.NotFound(projectId));
            }

            project_db.Tasks.Add(task_db);


            if (_projectRepositoryBase.Update(project_db))
            {
                return Result<Project, ProjectDTO>.Success();
            }

            return Result<Project, ProjectDTO>.Failure(ModelError<Project>.ServerError);
        }

        public Result<Project, ProjectDTO> AddWorkerToProject(long projectId, long workerId)
        {
            var project_db = _projectRepositoryBase.GetById(projectId, true);
            var worker_db = _workerRepositoryBase.GetById(workerId, true);

            if (worker_db == null)
            {
                return Result<Project, ProjectDTO>.Failure(ModelError<Worker>.NotFound(workerId));
            }
            if (project_db == null)
            {
                return Result<Project, ProjectDTO>.Failure(ModelError<Project>.NotFound(projectId));
            }

            project_db.Workers.Add(worker_db);


            if (_projectRepositoryBase.Update(project_db))
            {
                return Result<Project, ProjectDTO>.Success();
            }

            return Result<Project, ProjectDTO>.Failure(ModelError<Project>.ServerError);
        }

        public Result<Project, ProjectDTO> DeleteProject(long id)
        {
            var Project = _projectRepositoryBase.GetById(id, true);
            if (Project == null)
            {
                return Result<Project, ProjectDTO>.Failure(ModelError<Project>.NotFound(id));
            }

            var projectWorkers = Project.Workers;

            foreach (var worker in projectWorkers)
            {
                worker.Projects.Remove(Project);
            }

            //Cascade deleting tasks
            if (_projectRepositoryBase.DeleteModel(Project))
            {
                return Result<Project, ProjectDTO>.Success();
            }

            return Result<Project, ProjectDTO>.Failure(ModelError<Project>.ServerError);
        }


        public Result<Project, ProjectDTO> GetProjects(string Name)
        {
            var projects = _projectRepository.GetProjects(Name);
            var res = Result<Project, ProjectDTO>.Success();
            res.Models = projects.ToList();
            res.ModelDTOs = _mapper.Map<List<ProjectDTO>>(projects.ToList());
            return res;
        }

        public Result<TaskUnit, TaskUnitDTO> GetProjectTasks(long id)
        {
            var tasks = _projectRepository.GetProjectTasks(id);
            if (tasks != null)
            {
                var res = Result<TaskUnit, TaskUnitDTO>.Success();
                res.Models = tasks.ToList();
                res.ModelDTOs = _mapper.Map<List<TaskUnitDTO>>(tasks.ToList());
                return res;
            }
            else return Result<TaskUnit, TaskUnitDTO>.Failure(ModelError<Project>.NotFound(id));
        }

        public Result<Worker, WorkerDTO> GetProjectWorkers(long id)
        {
            var workers = _projectRepository.GetProjectWorkers(id);
            if (workers != null)
            {
                var res = Result<Worker, WorkerDTO>.Success();
                res.Models = workers.ToList();
                res.ModelDTOs = _mapper.Map<List<WorkerDTO>>(workers.ToList());
                return res;
            }
            else return Result<Worker, WorkerDTO>.Failure(ModelError<Project>.NotFound(id));
        }


        public Result<Project, ProjectDTO> RemoveTaskFromProject(long projectId, long taskId)
        {
            var project_db = _projectRepositoryBase.GetById(projectId, true);
            var task_db = _taskRepositoryBase.GetById(taskId, true);


            if (task_db == null)
            {
                return Result<Project, ProjectDTO>.Failure(ModelError<TaskUnit>.NotFound(taskId));
            }
            if (project_db == null)
            {
                return Result<Project, ProjectDTO>.Failure(ModelError<Project>.NotFound(projectId));
            }

            project_db.Tasks.Remove(task_db);


            if (_projectRepositoryBase.Update(project_db))
            {
                return Result<Project, ProjectDTO>.Success();
            }

            return Result<Project, ProjectDTO>.Failure(ModelError<Project>.ServerError);
        }

        public Result<Project, ProjectDTO> RemoveWorkerFromProject(long projectId, long workerId)
        {
            var project_db = _projectRepositoryBase.GetById(projectId, true);
            var worker_db = _workerRepositoryBase.GetById(workerId, true);


            if (worker_db == null)
            {
                return Result<Project, ProjectDTO>.Failure(ModelError<Worker>.NotFound(workerId));
            }
            if (project_db == null)
            {
                return Result<Project, ProjectDTO>.Failure(ModelError<Project>.NotFound(projectId));
            }

            project_db.Workers.Remove(worker_db);


            if (_projectRepositoryBase.Update(project_db))
            {
                return Result<Project, ProjectDTO>.Success();
            }

            return Result<Project, ProjectDTO>.Failure(ModelError<Project>.ServerError);
        }

        public Result<Project, ProjectDTO> UpdateProject(long id, CreateProjectDTO projectDTO)
        {
            var project = _mapper.Map<Project>(projectDTO);
            var project_db = _projectRepositoryBase.GetById(id, true);

            if (project == null)
            {
                return Result<Project, ProjectDTO>.Failure(ModelError<Project>.NullReference);
            }
            if (project_db == null)
            {
                return Result<Project, ProjectDTO>.Failure(ModelError<Project>.NotFound(id));
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
                            return Result<Project, ProjectDTO>.Failure(ModelError<Worker>.NotFound(worker.Id));
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
                            return Result<Project, ProjectDTO>.Failure(ModelError<TaskUnit>.NotFound(taskUnit.Id));
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
                    return Result<Project, ProjectDTO>.Success();
                }

                return Result<Project, ProjectDTO>.Failure(ModelError<Project>.ServerError);
            }
        }
    }
}
