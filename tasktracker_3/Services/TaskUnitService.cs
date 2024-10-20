using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.EntityFrameworkCore;
using tasktracker_3.Interfaces;
using tasktracker_3.Interfaces.Services;
using tasktracker_3.Models;
using tasktracker_3.Repository;
using Microsoft.IdentityModel.Tokens;
using tasktracker_3.Data;
using tasktracker_3.Models.Enums;
using System.Threading.Tasks;

namespace tasktracker_3.Services
{
    public class TaskUnitService : ITaskUnitService
    {
        private readonly ITaskUnitRepository _taskUnitRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IWorkerRepository _workerRepository;
        public TaskUnitService(ITaskUnitRepository taskUnitRepository, IProjectRepository projectRepository, IWorkerRepository workerRepository)
        {
            _projectRepository = projectRepository;
            _workerRepository = workerRepository;
            _taskUnitRepository = taskUnitRepository;

        }

        public IActionResult AddChildTaskToTask(long parentTaskId, long childTaskId)
        {
            var parent_task_db = _taskUnitRepository.GetTask(parentTaskId);
            var child_task_db = _taskUnitRepository.GetTask(childTaskId);


            if (parent_task_db == null)
            {
                return new NotFoundObjectResult("Task was not found");
            }
            if (child_task_db == null)
            {
                return new NotFoundObjectResult("Child task was not found");
            }


            parent_task_db.ParentOf.Add(child_task_db);


            if (_taskUnitRepository.UpdateTask(parent_task_db))
            {
                return new OkObjectResult("Success! Child task was added to task!");
            }

            return new BadRequestObjectResult("Something went wrong!");
        }

        public IActionResult AddTask(TaskUnit taskUnit)
        {
            var TaskUnits = _taskUnitRepository.GetTasks().Where(t => t.Title.Trim().ToUpper() == taskUnit.Title.Trim().ToUpper()).FirstOrDefault();

            if (TaskUnits != null)
            {
                return new UnprocessableEntityObjectResult("Task " + taskUnit.Title + " already exists");
            }

            if (taskUnit == null)
            {
                return new NotFoundObjectResult("Task was not found");
            }

            ICollection<Worker> wkr = taskUnit.Workers;
            ICollection<Worker> workers = new List<Worker>();

            ICollection<TaskUnit> tsk = taskUnit.ParentOf;
            ICollection<TaskUnit> childrenTasks = new List<TaskUnit>();

            if (!taskUnit.Workers.IsNullOrEmpty())
            {
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
                taskUnit.Workers = workers;
            }

            if (!taskUnit.ParentOf.IsNullOrEmpty())
            {
                foreach (var childTask in tsk)
                {
                    var db_child = _taskUnitRepository.GetTask(childTask.Id);
                    if (db_child == null)
                    {
                        return new NotFoundObjectResult("Task with Id: " + childTask.Id + " was not found");
                    }
                    else
                    {
                        childrenTasks.Add(db_child);
                    }
                }
                taskUnit.ParentOf = childrenTasks;
            }

            ICollection<TaskUnit> tsk2 = taskUnit.ChildOf;
            ICollection<TaskUnit> parentTasks = new List<TaskUnit>();

            //children
            if (!taskUnit.ChildOf.IsNullOrEmpty())
            {
                foreach (var parentTask in tsk2)
                {
                    var db_parent = _taskUnitRepository.GetTask(parentTask.Id);
                    if (db_parent == null)
                    {
                        return new NotFoundObjectResult("Task with Id: " + parentTask.Id + " was not found");
                    }
                    else
                    {
                        parentTasks.Add(db_parent);
                    }
                }
                taskUnit.ChildOf = parentTasks;
            }

            if (taskUnit.Project != null)
            {
                if (!_projectRepository.ProjectExists(taskUnit.Project.Id))
                {
                    return new NotFoundObjectResult("Project with " + taskUnit.Project.Id + "was not found");
                }

                taskUnit.Project = _projectRepository.GetProject(taskUnit.Project.Id);
            }
            else
            {
                return new BadRequestObjectResult("Project reference is required");
            }


            if (_taskUnitRepository.AddTask(taskUnit))
            {
                return new OkObjectResult("Success! Task added!");
            }

            return new BadRequestObjectResult("Something went wrong!");
        }

        public IActionResult AddWorkerToTask(long taskId, long workerId)
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

            task_db.Workers.Add(worker_db);


            if (_taskUnitRepository.UpdateTask(task_db))
            {
                return new OkObjectResult("Success! Worker added to task!");
            }

            return new BadRequestObjectResult("Something went wrong!");

        }

        public IActionResult DeleteTask(long Id)
        {
            var taskUnit = _taskUnitRepository.GetTask(Id);
            if (taskUnit == null)
            {
                return new NotFoundObjectResult("Task was not found");
            }

            var taskWorkers = taskUnit.Workers;

            foreach (var worker in taskWorkers)
            {
                worker.Tasks.Remove(taskUnit);
            }
            //think
            var taskParent = taskUnit.ParentOf;
            var taskChild = taskUnit.ChildOf;

            foreach (var child in taskParent)
            {
                child.ChildOf.Remove(taskUnit);
            }
            foreach (var parent in taskChild)
            {
                parent.ChildOf.Remove(taskUnit);
            }

            taskUnit.ParentOf.Clear();
            taskUnit.ChildOf.Clear();
            //
            var taskProject = taskUnit.Project;

            if (taskProject != null)
            {
                taskProject.Tasks.Remove(taskUnit);
            }

            if (_taskUnitRepository.DeleteTask(taskUnit))
            {
                return new OkObjectResult("Success! Task was deleted!");
            }

            return new BadRequestObjectResult("Something went wrong!");
        }

        public ICollection<TaskUnit>? GetChildrenOfTask(long id)
        {
            if (_taskUnitRepository.TaskExists(id))
            {
                return _taskUnitRepository.GetChildrenOfTask(id);
            }
            else return null;
        }

        public ICollection<TaskUnit>? GetParentsOfTask(long id)
        {
            if (_taskUnitRepository.TaskExists(id))
            {
                return _taskUnitRepository.GetParentsOfTask(id);
            }
            else return null;
        }

        public TaskUnit? GetTask(long id)
        {
            return _taskUnitRepository.GetTask(id);
        }

        public Project? GetTaskProject(long id)
        {
            if (_taskUnitRepository.TaskExists(id))
            {
                return _taskUnitRepository.GetTaskProject(id);
            }
            return null;
        }

        public ICollection<TaskUnit> GetTasks(string Title)
        {
            return _taskUnitRepository.GetTasks(Title);
        }

        public ICollection<TaskUnit> GetTasks()
        {
            return _taskUnitRepository.GetTasks();
        }

        public ICollection<Worker>? GetTaskWorkers(long id)
        {
            if (_taskUnitRepository.TaskExists(id))
            {
                return _taskUnitRepository.GetTaskWorkers(id);
            }
            else return null;
        }

        public IActionResult RemoveChildTaskFromTask(long parentTaskId, long childTaskId)
        {
            var parent_task_db = _taskUnitRepository.GetTask(parentTaskId);
            var child_task_db = _taskUnitRepository.GetTask(childTaskId);


            if (parent_task_db == null)
            {
                return new NotFoundObjectResult("Task was not found");
            }
            if (child_task_db == null)
            {
                return new NotFoundObjectResult("Child Task was not found");
            }

            parent_task_db.ParentOf.Remove(child_task_db);

            if (_taskUnitRepository.UpdateTask(parent_task_db))
            {
                return new OkObjectResult("Success! Child task from task was removed!");
            }

            return new BadRequestObjectResult("Something went wrong!");
        }

        public IActionResult RemoveWorkerFromTask(long taskId, long workerId)
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

            task_db.Workers.Remove(worker_db);

            if (_taskUnitRepository.UpdateTask(task_db))
            {
                return new OkObjectResult("Success! Worker from task was removed!");
            }

            return new BadRequestObjectResult("Something went wrong!");

        }

        public bool TaskExists(long id)
        {
            return _taskUnitRepository.TaskExists(id);
        }

        public IActionResult UpdateTask(long id, TaskUnit taskUnit)
        {
            var task_db = _taskUnitRepository.GetTask(id);
            if (taskUnit == null || task_db == null)
            {
                return new BadRequestObjectResult("Task was not found");
            }
            else
            {
                taskUnit.Id = id;

                task_db.Title = taskUnit.Title;
                task_db.Description = taskUnit.Description;
                task_db.UpdatedDate = taskUnit.UpdatedDate;
                task_db.CreatedDate = taskUnit.CreatedDate;
                task_db.Status = taskUnit.Status;
                task_db.EndDate = taskUnit.EndDate;

                if (!taskUnit.Workers.IsNullOrEmpty())
                {
                    ICollection<Worker> wkr = taskUnit.Workers;
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
                    task_db.Workers.Clear();
                    task_db.Workers = workers;
                }

                ICollection<TaskUnit> tsk = taskUnit.ParentOf;
                ICollection<TaskUnit> childrenTasks = new List<TaskUnit>();

                if (!taskUnit.ParentOf.IsNullOrEmpty())
                {
                    foreach (var childTask in tsk)
                    {
                        var db_child = _taskUnitRepository.GetTask(childTask.Id);
                        if (db_child == null)
                        {
                            return new NotFoundObjectResult("Task with Id: " + childTask.Id + " was not found");
                        }
                        else
                        {
                            childrenTasks.Add(db_child);
                        }
                    }
                    taskUnit.ParentOf.Clear();
                    taskUnit.ParentOf = childrenTasks;
                }

                ICollection<TaskUnit> tsk2 = taskUnit.ChildOf;
                ICollection<TaskUnit> parentTasks = new List<TaskUnit>();

                //children
                if (!taskUnit.ChildOf.IsNullOrEmpty())
                {
                    foreach (var parentTask in tsk2)
                    {
                        var db_parent = _taskUnitRepository.GetTask(parentTask.Id);
                        if (db_parent == null)
                        {
                            return new NotFoundObjectResult("Task with Id: " + parentTask.Id + " was not found");
                        }
                        else
                        {
                            parentTasks.Add(db_parent);
                        }
                    }
                    taskUnit.ChildOf.Clear();
                    taskUnit.ChildOf = parentTasks;
                }


                if (taskUnit.Project != null)
                {
                    if (!_projectRepository.ProjectExists(taskUnit.Project.Id))
                    {
                        return new NotFoundObjectResult("Project was not found");
                    }
                    task_db.Project = _projectRepository.GetProject(taskUnit.Project.Id);
                }
                else
                {
                    return new BadRequestObjectResult("Project reference is required");
                }

                if (_taskUnitRepository.UpdateTask(task_db))
                {
                    return new OkObjectResult("Success! Task was updated!");
                }

                return new BadRequestObjectResult("Something went wrong!");

            }
        }


    }
}
