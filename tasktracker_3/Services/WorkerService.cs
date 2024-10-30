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
    public class WorkerService : BaseService<Worker, WorkerDTO>, IWorkerService
    {
        public WorkerService(IMapper mapper, BaseRepository<Worker> baseRepository,
            BaseRepository<TaskUnit> taskRepositoryBase,
            BaseRepository<Project> projectRepositoryBase,
            BaseRepository<Worker> workerRepositoryBase,
            ITaskUnitRepository taskUnitRepository, IProjectRepository projectRepository, IWorkerRepository workerRepository)
        : base(mapper,
               baseRepository,
               taskRepositoryBase,
               projectRepositoryBase,
               workerRepositoryBase,
               taskUnitRepository,
               workerRepository,
               projectRepository) { }


        public Result<Worker, WorkerDTO> AddProjectToWorker(long workerId, long projectId)
        {
            var project_db = _projectRepositoryBase.GetById(projectId, true);
            var worker_db = _workerRepositoryBase.GetById(workerId, true);


            if (worker_db == null)
            {
                return Result<Worker, WorkerDTO>.Failure(ModelError<Worker>.NotFound(workerId));
            }
            if (project_db == null)
            {
                return Result<Worker, WorkerDTO>.Failure(ModelError<Project>.NotFound(projectId));
            }

            worker_db.Projects.Add(project_db);

            if (_workerRepositoryBase.Update(worker_db))
            {
                return Result<Worker, WorkerDTO>.Success();
            }

            return Result<Worker, WorkerDTO>.Failure(ModelError<Worker>.ServerError);
        }

        public Result<Worker, WorkerDTO> AddTaskToWorker(long workerId, long taskId)
        {
            var task_db = _taskRepositoryBase.GetById(taskId, true);
            var worker_db = _workerRepositoryBase.GetById(workerId, true);

            if (task_db == null)
            {
                return Result<Worker, WorkerDTO>.Failure(ModelError<TaskUnit>.NotFound(workerId));
            }
            if (worker_db == null)
            {
                return Result<Worker, WorkerDTO>.Failure(ModelError<Worker>.NotFound(workerId));
            }

            worker_db.Tasks.Add(task_db);


            if (_workerRepositoryBase.Update(worker_db))
            {
                return Result<Worker, WorkerDTO>.Success();
            }

            return Result<Worker, WorkerDTO>.Failure(ModelError<Worker>.ServerError);
        }

        public Result<Worker, WorkerDTO> AddWorker(CreateWorkerDTO WorkerDTO)
        {
            var Worker = _mapper.Map<Worker>(WorkerDTO);
            if (Worker == null)
            {
                return Result<Worker, WorkerDTO>.Failure(ModelError<Worker>.NullReference);
            }
            else
            {
                if (!Worker.Projects.IsNullOrEmpty())
                {
                    ICollection<Project> prj = Worker.Projects;
                    ICollection<Project> projects = new List<Project>();
                    foreach (var project in prj)
                    {
                        var db_project = _projectRepositoryBase.GetById(project.Id, true);
                        if (db_project == null)
                        {
                            return Result<Worker, WorkerDTO>.Failure(ModelError<Project>.NotFound(project.Id));

                        }
                        else
                        {
                            projects.Add(db_project);
                        }
                    }
                    Worker.Projects = projects;
                }

                if (!Worker.Tasks.IsNullOrEmpty())
                {
                    ICollection<TaskUnit> tsks_create = Worker.Tasks;
                    ICollection<TaskUnit> taskUnits_new = new List<TaskUnit>();
                    foreach (var taskUnit in tsks_create)
                    {
                        var db_task = _taskRepositoryBase.GetById(taskUnit.Id, true);
                        if (db_task == null)
                        {
                            return Result<Worker, WorkerDTO>.Failure(ModelError<TaskUnit>.NotFound(taskUnit.Id));
                        }
                        else
                        {
                            taskUnits_new.Add(db_task);
                        }
                    }
                    Worker.Tasks = taskUnits_new;
                }

                if (_workerRepositoryBase.AddModel(Worker))
                {
                    return Result<Worker, WorkerDTO>.Success();
                }

                return Result<Worker, WorkerDTO>.Failure(ModelError<Worker>.ServerError);
            }
        }

        public Result<Worker, WorkerDTO> DeleteWorker(long id)
        {
            var Worker = _workerRepositoryBase.GetById(id, true);
            if (Worker == null)
            {
                return Result<Worker, WorkerDTO>.Failure(ModelError<Worker>.NullReference);
            }

            var workerProjects = Worker.Projects;

            foreach (var project in workerProjects)
            {
                project.Workers.Remove(Worker);
            }

            var workerTasks = Worker.Tasks;

            foreach (var task in workerTasks)
            {
                task.Workers.Remove(Worker);
            }

            if (_workerRepositoryBase.DeleteModel(Worker))
            {
                return Result<Worker, WorkerDTO>.Success();
            }

            return Result<Worker, WorkerDTO>.Failure(ModelError<Worker>.ServerError);
        }

        public Result<Project, ProjectDTO> GetWorkerProjects(long id)
        {
            var projects = _workerRepository.GetWorkerProjects(id);
            if (projects != null)
            {
                var res = Result<Project, ProjectDTO>.Success();
                res.Models = projects.ToList();
                res.ModelDTOs = _mapper.Map<List<ProjectDTO>>(projects.ToList());
                return res;
            }
            else return Result<Project, ProjectDTO>.Failure(ModelError<Worker>.NotFound(id));
        }

        public Result<TaskUnit, TaskUnitDTO> GetWorkerTasks(long id)
        {
            var tasks = _workerRepository.GetWorkerTasks(id);
            if (tasks != null)
            {
                var res = Result<TaskUnit, TaskUnitDTO>.Success();
                res.Models = tasks.ToList();
                res.ModelDTOs = _mapper.Map<List<TaskUnitDTO>>(tasks.ToList());
                return res;
            }
            else return Result<TaskUnit, TaskUnitDTO>.Failure(ModelError<Worker>.NotFound(id));
        }

        public Result<Worker, WorkerDTO> GetWorkerById(long id, bool includes)
        {
            return GetById(id, includes);
        }

        public Result<Worker, WorkerDTO> GetAllWorkers(bool includes)
        {
            return GetAll(includes);
        }

        public bool WorkerExists(long id)
        {
            return ModelExists(id);
        }

        public Result<Worker, WorkerDTO> RemoveProjectFromWorker(long workerId, long projectId)
        {
            var worker_db = _workerRepositoryBase.GetById(workerId, true);
            var project_db = _projectRepositoryBase.GetById(projectId, true);


            if (project_db == null)
            {
                return Result<Worker, WorkerDTO>.Failure(ModelError<Project>.NotFound(projectId)); ;
            }
            if (worker_db == null)
            {
                return Result<Worker, WorkerDTO>.Failure(ModelError<Worker>.NotFound(workerId));
            }

            worker_db.Projects.Remove(project_db);


            if (_workerRepositoryBase.Update(worker_db))
            {
                return Result<Worker, WorkerDTO>.Success();
            }

            return Result<Worker, WorkerDTO>.Failure(ModelError<Worker>.ServerError);
        }

        public Result<Worker, WorkerDTO> RemoveTaskFromWorker(long workerId, long taskId)
        {
            var worker_db = _workerRepositoryBase.GetById(workerId, true);
            var task_db = _taskRepositoryBase.GetById(taskId, true);


            if (task_db == null)
            {
                return Result<Worker, WorkerDTO>.Failure(ModelError<TaskUnit>.NotFound(taskId));
            }
            if (worker_db == null)
            {
                return Result<Worker, WorkerDTO>.Failure(ModelError<Worker>.NotFound(workerId));
            }

            worker_db.Tasks.Remove(task_db);


            if (_workerRepositoryBase.Update(worker_db))
            {
                return Result<Worker, WorkerDTO>.Success();
            }

            return Result<Worker, WorkerDTO>.Failure(ModelError<Worker>.ServerError);
        }

        public Result<Worker, WorkerDTO> UpdateWorker(long id, CreateWorkerDTO workerDTO)
        {
            var worker = _mapper.Map<Worker>(workerDTO);
            var worker_db = _workerRepositoryBase.GetById(id, true);
            if (worker == null)
            {
                return Result<Worker, WorkerDTO>.Failure(ModelError<Worker>.NullReference);
            }
            if (worker_db == null)
            {
                return Result<Worker, WorkerDTO>.Failure(ModelError<Worker>.NotFound(id));
            }
            else
            {
                worker.Id = id;
                worker_db.Name = worker.Name;
                worker_db.Surname = worker.Surname;
                worker_db.Age = worker.Age;
                worker_db.Sex = worker.Sex;
                worker_db.Job = worker.Job;

                if (!worker.Projects.IsNullOrEmpty())
                {
                    ICollection<Project> prj = worker.Projects;
                    ICollection<Project> projects = new List<Project>();
                    foreach (var project in prj)
                    {
                        var db_project = _projectRepositoryBase.GetById(project.Id, true);
                        if (db_project == null)
                        {
                            return Result<Worker, WorkerDTO>.Failure(ModelError<Project>.NotFound(project.Id));
                        }
                        else
                        {
                            projects.Add(db_project);
                        }
                    }
                    worker_db.Projects.Clear();
                    worker_db.Projects = projects;
                }


                if (!worker.Tasks.IsNullOrEmpty())
                {
                    ICollection<TaskUnit> tsks_create = worker.Tasks;
                    ICollection<TaskUnit> taskUnits_new = new List<TaskUnit>();
                    foreach (var taskUnit in tsks_create)
                    {
                        var db_task = _taskRepositoryBase.GetById(taskUnit.Id, true);
                        if (db_task == null)
                        {
                            return Result<Worker, WorkerDTO>.Failure(ModelError<TaskUnit>.NotFound(taskUnit.Id));
                        }
                        else
                        {
                            taskUnits_new.Add(db_task);
                        }
                    }
                    worker.Tasks.Clear();
                    worker_db.Tasks = taskUnits_new;
                }

                if (_workerRepositoryBase.Update(worker_db))
                {
                    return Result<Worker, WorkerDTO>.Success();
                }

                return Result<Worker, WorkerDTO>.Failure(ModelError<Worker>.ServerError);
            }
        }
    }
}
