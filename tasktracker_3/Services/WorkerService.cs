using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using tasktracker_3.Interfaces;
using tasktracker_3.Interfaces.Services;
using tasktracker_3.Models;

namespace tasktracker_3.Services
{
    public class WorkerService : IWorkerService
    {
        private readonly ITaskUnitRepository _taskUnitRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IWorkerRepository _workerRepository;

        public WorkerService(ITaskUnitRepository taskUnitRepository, IProjectRepository projectRepository, IWorkerRepository workerRepository)
        {
            _projectRepository = projectRepository;
            _workerRepository = workerRepository;
            _taskUnitRepository = taskUnitRepository;

        }

        public IActionResult AddProjectToWorker(long workerId, long projectId)
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

            worker_db.Projects.Add(project_db);

            if (_workerRepository.UpdateWorker(worker_db))
            {
                return new OkObjectResult("Success! Project was added to worker!");
            }

            return new BadRequestObjectResult("Something went wrong!");
        }

        public IActionResult AddTaskToWorker(long workerId, long taskId)
        {
            var task_db = _taskUnitRepository.GetTask(taskId);
            var worker_db = _workerRepository.GetWorker(workerId);

            if (task_db == null)
            {
                return new NotFoundObjectResult("Task was not found");
            }
            if (worker_db == null)
            {
                return new NotFoundObjectResult("Worker was not found");
            }

            worker_db.Tasks.Add(task_db);


            if (_workerRepository.UpdateWorker(worker_db))
            {
                return new OkObjectResult("Success! Task was added to worker!");
            }

            return new BadRequestObjectResult("Something went wrong!");
        }

        public IActionResult AddWorker(Worker Worker)
        {
            if (Worker == null)
            {
                return new NotFoundObjectResult("Worker was not found");
            }
            else
            {
                if (!Worker.Projects.IsNullOrEmpty())
                {
                    ICollection<Project> prj = Worker.Projects;
                    ICollection<Project> projects = new List<Project>();
                    foreach (var project in prj)
                    {
                        var db_project = _projectRepository.GetProject(project.Id);
                        if (db_project == null)
                        {
                            return new NotFoundObjectResult("Project with Id: " + project.Id + " was not found");

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
                        var db_task = _taskUnitRepository.GetTask(taskUnit.Id);
                        if (db_task == null)
                        {
                            new NotFoundObjectResult("Task with Id: " + taskUnit.Id + " was not found");
                        }
                        else
                        {
                            taskUnits_new.Add(db_task);
                        }
                    }
                    Worker.Tasks = taskUnits_new;
                }

                if (_workerRepository.AddWorker(Worker))
                {
                    return new OkObjectResult("Success! Worker added!");
                }

                return new BadRequestObjectResult("Something went wrong!");
            }
        }

        public IActionResult DeleteWorker(long id)
        {
            var Worker = _workerRepository.GetWorker(id);
            if (Worker == null)
            {
                return new NotFoundObjectResult("Worker was not found");
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

            if (_workerRepository.DeleteWorker(Worker))
            {
                return new OkObjectResult("Success! Worker was deleted!");
            }

            return new BadRequestObjectResult("Something went wrong!");
        }

        public Worker? GetWorker(long id)
        {
            return _workerRepository.GetWorker(id);
        }

        public ICollection<Project>? GetWorkerProjects(long id)
        {
            if (_workerRepository.WorkerExists(id))
            {
                return _workerRepository.GetWorkerProjects(id);
            }
            return null;
        }

        public ICollection<Worker> GetWorkers()
        {
            return _workerRepository.GetWorkers();
        }

        public ICollection<TaskUnit>? GetWorkerTasks(long id)
        {
            if (_workerRepository.WorkerExists(id))
            {
                return _workerRepository.GetWorkerTasks(id);
            }
            return null;
        }

        public IActionResult RemoveProjectFromWorker(long workerId, long projectId)
        {
            var worker_db = _workerRepository.GetWorker(workerId);
            var project_db = _projectRepository.GetProject(projectId);


            if (project_db == null)
            {
                return new NotFoundObjectResult("Project was not found");
            }
            if (worker_db == null)
            {
                return new NotFoundObjectResult("Worker was not found");
            }

            worker_db.Projects.Remove(project_db);


            if (_workerRepository.UpdateWorker(worker_db))
            {
                return new OkObjectResult("Success! Project was removed from Worker!");
            }

            return new BadRequestObjectResult("Something went wrong!");
        }

        public IActionResult RemoveTaskFromWorker(long workerId, long taskId)
        {
            var worker_db = _workerRepository.GetWorker(workerId);
            var task_db = _taskUnitRepository.GetTask(taskId);


            if (task_db == null)
            {
                return new NotFoundObjectResult("Task was not found");
            }
            if (worker_db == null)
            {
                return new NotFoundObjectResult("Worker was not found");
            }

            worker_db.Tasks.Remove(task_db);


            if (_workerRepository.UpdateWorker(worker_db))
            {
                return new OkObjectResult("Success! Task was removed from worker!");
            }

            return new BadRequestObjectResult("Something went wrong!");
        }

        public IActionResult UpdateWorker(long id, Worker worker)
        {
            var worker_db = _workerRepository.GetWorker(id);
            if (worker == null || worker_db == null)
            {
                return new NotFoundObjectResult("Work was not found");
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
                        var db_project = _projectRepository.GetProject(project.Id);
                        if (db_project == null)
                        {
                            return new NotFoundObjectResult("Project was not found");
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
                    worker.Tasks.Clear();
                    worker_db.Tasks = taskUnits_new;
                }

                if (_workerRepository.UpdateWorker(worker_db))
                {
                    return new OkObjectResult("Success! Worker was updated!");
                }

                return new BadRequestObjectResult("Something went wrong!");
            }
        }

        public bool WorkerExists(long id)
        {
            return _workerRepository.WorkerExists(id);
        }
    }
}
