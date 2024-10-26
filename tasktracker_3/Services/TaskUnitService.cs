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
    public class TaskUnitService : BaseService<TaskUnit>, ITaskUnitService
    {

        public TaskUnitService(BaseRepository<TaskUnit> baseRepository,
            BaseRepository<TaskUnit> taskRepositoryBase,
            BaseRepository<Project> projectRepositoryBase,
            BaseRepository<Worker> workerRepositoryBase,
            ITaskUnitRepository taskUnitRepository, IProjectRepository projectRepository, IWorkerRepository workerRepository)
        : base(baseRepository, taskRepositoryBase, projectRepositoryBase, workerRepositoryBase, taskUnitRepository, workerRepository, projectRepository) { }


        public Result<TaskUnit> AddChildTaskToTask(long parentTaskId, long childTaskId)
        {
            var parent_task_db = _taskRepositoryBase.GetById(parentTaskId, true);
            var child_task_db = _taskRepositoryBase.GetById(childTaskId, true);


            if (parent_task_db == null)
            {
                return Result<TaskUnit>.Failure(ModelError<TaskUnit>.NotFound(parentTaskId));
            }
            if (child_task_db == null)
            {
                return Result<TaskUnit>.Failure(ModelError<TaskUnit>.NotFound(childTaskId));
            }


            parent_task_db.ParentOf.Add(child_task_db);


            if (_taskRepositoryBase.Update(parent_task_db))
            {
                return Result<TaskUnit>.Success();
            }

            return Result<TaskUnit>.Failure(ModelError<TaskUnit>.ServerError);
        }

        public Result<TaskUnit> AddTask(TaskUnit taskUnit)
        {

            if (taskUnit == null)
            {
                return Result<TaskUnit>.Failure(ModelError<TaskUnit>.NullReference);
            }

            var TaskUnits = _taskRepositoryBase.GetAll(false).Where(t => t.Title.Trim().ToUpper() == taskUnit.Title.Trim().ToUpper()).FirstOrDefault();

            if (TaskUnits != null)
            {
                return Result<TaskUnit>.Failure(ModelError<TaskUnit>.SameTitle(taskUnit.Title));
            }

            ICollection<Worker> wkr = taskUnit.Workers;
            ICollection<Worker> workers = new List<Worker>();

            ICollection<TaskUnit> tsk = taskUnit.ParentOf;
            ICollection<TaskUnit> childrenTasks = new List<TaskUnit>();

            if (!taskUnit.Workers.IsNullOrEmpty())
            {
                foreach (var worker in wkr)
                {
                    var db_worker = _workerRepositoryBase.GetById(worker.Id, true);
                    if (db_worker == null)
                    {
                        return Result<TaskUnit>.Failure(ModelError<Worker>.NotFound(worker.Id));
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
                    var db_child = _taskRepositoryBase.GetById(childTask.Id, true);
                    if (db_child == null)
                    {
                        return Result<TaskUnit>.Failure(ModelError<TaskUnit>.NotFound(childTask.Id));
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
                    var db_parent = _taskRepositoryBase.GetById(parentTask.Id, true);
                    if (db_parent == null)
                    {
                        return Result<TaskUnit>.Failure(ModelError<TaskUnit>.NotFound(parentTask.Id));
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
                var project = _projectRepositoryBase.GetById(taskUnit.Project.Id, true);
                if (project == null)
                {
                    return Result<TaskUnit>.Failure(ModelError<Project>.NotFound(taskUnit.Project.Id));
                }

                taskUnit.Project = project;
            }
            else
            {
                return Result<TaskUnit>.Failure(ModelError<Project>.NullReference);
            }


            if (_taskRepositoryBase.AddModel(taskUnit))
            {
                return Result<TaskUnit>.Success();
            }

            return Result<TaskUnit>.Failure(ModelError<TaskUnit>.ServerError);
        }

        public Result<TaskUnit> AddWorkerToTask(long taskId, long workerId)
        {
            var task_db = _taskRepositoryBase.GetById(taskId, true);
            var worker_db = _workerRepositoryBase.GetById(workerId, true);


            if (task_db == null)
            {
                return Result<TaskUnit>.Failure(ModelError<TaskUnit>.NotFound(taskId));
            }
            if (worker_db == null)
            {
                return Result<TaskUnit>.Failure(ModelError<Worker>.NotFound(workerId));
            }

            task_db.Workers.Add(worker_db);


            if (_taskRepositoryBase.Update(task_db))
            {
                return Result<TaskUnit>.Success();
            }

            return Result<TaskUnit>.Failure(ModelError<TaskUnit>.ServerError);

        }

        public Result<TaskUnit> DeleteTask(long Id)
        {
            var taskUnit = _taskRepositoryBase.GetById(Id, true);
            if (taskUnit == null)
            {
                return Result<TaskUnit>.Failure(ModelError<TaskUnit>.NotFound(Id));
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

            if (_taskRepositoryBase.DeleteModel(taskUnit))
            {
                return Result<TaskUnit>.Success();
            }

            return Result<TaskUnit>.Failure(ModelError<TaskUnit>.ServerError);
        }

        public Result<TaskUnit> GetChildrenOfTask(long id)
        {
            var children = _taskUnitRepository.GetChildrenOfTask(id);
            if (children!=null) {
                var res = Result<TaskUnit>.Success();
                res.Models = children.ToList();
                return res;
            }
            else return Result<TaskUnit>.Failure(ModelError<TaskUnit>.NotFound(id));
        }

        public Result<TaskUnit> GetParentsOfTask(long id)
        {
            var parents = _taskUnitRepository.GetParentsOfTask(id);
            if (parents != null)
            {
                var res = Result<TaskUnit>.Success();
                res.Models = parents.ToList();
                return res;
            }
            else return Result<TaskUnit>.Failure(ModelError<TaskUnit>.NotFound(id));
        }

        public Result<Project> GetTaskProject(long id)
        {
            var project = _taskUnitRepository.GetTaskProject(id);
            if (project != null)
            {
                var res = Result<Project>.Success();
                res.Model = project;
                return res;
            }
            else return Result<Project>.Failure(ModelError<TaskUnit>.NotFound(id));
        }

        public Result<TaskUnit> GetTasks(string Title)
        {
            var res = Result<TaskUnit>.Success();
            res.Models = _taskUnitRepository.GetTasks(Title).ToList();

            return res;
        }

        public Result<Worker> GetTaskWorkers(long id)
        {
            var workers = _taskUnitRepository.GetTaskWorkers(id);
            if (workers != null)
            {
                var res = Result<Worker>.Success();
                res.Models = workers.ToList();
                return res;
            }
            else return Result<Worker>.Failure(ModelError<TaskUnit>.NotFound(id));
        }

        public Result<TaskUnit> RemoveChildTaskFromTask(long parentTaskId, long childTaskId)
        {
            var parent_task_db = _taskRepositoryBase.GetById(parentTaskId, true);
            var child_task_db = _taskRepositoryBase.GetById(childTaskId, true);


            if (parent_task_db == null)
            {
                return Result<TaskUnit>.Failure(ModelError<TaskUnit>.NotFound(parentTaskId));
            }
            if (child_task_db == null)
            {
                return Result<TaskUnit>.Failure(ModelError<TaskUnit>.NotFound(childTaskId));
            }

            parent_task_db.ParentOf.Remove(child_task_db);

            if (_taskRepositoryBase.Update(parent_task_db))
            {
                return Result<TaskUnit>.Success();
            }

            return Result<TaskUnit>.Failure(ModelError<TaskUnit>.ServerError);
        }

        public Result<TaskUnit> RemoveWorkerFromTask(long taskId, long workerId)
        {
            var task_db = _taskRepositoryBase.GetById(taskId, true);
            var worker_db = _workerRepositoryBase.GetById(workerId, true);


            if (task_db == null)
            {
                return Result<TaskUnit>.Failure(ModelError<TaskUnit>.NotFound(taskId));
            }
            if (worker_db == null)
            {
                return Result<TaskUnit>.Failure(ModelError<Worker>.NotFound(workerId));
            }

            task_db.Workers.Remove(worker_db);

            if (_taskRepositoryBase.Update(task_db))
            {
                return Result<TaskUnit>.Success();
            }

            return Result<TaskUnit>.Failure(ModelError<TaskUnit>.ServerError);

        }

        public Result<TaskUnit> UpdateTask(long id, TaskUnit taskUnit)
        {
            var task_db = _taskRepositoryBase.GetById(id, true);
            if (taskUnit == null)
            {
                return Result<TaskUnit>.Failure(ModelError<TaskUnit>.NullReference);
            }
            if (task_db == null)
            {
                return Result<TaskUnit>.Failure(ModelError<TaskUnit>.NotFound(id));
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
                task_db.ParentOf = taskUnit.ParentOf;
                task_db.ChildOf = taskUnit.ChildOf;

                if (!taskUnit.Workers.IsNullOrEmpty())
                {
                    ICollection<Worker> wkr = taskUnit.Workers;
                    ICollection<Worker> workers = new List<Worker>();
                    foreach (var worker in wkr)
                    {
                        var db_worker = _workerRepositoryBase.GetById(worker.Id, true);
                        if (db_worker == null)
                        {
                            return Result<TaskUnit>.Failure(ModelError<Worker>.NotFound(worker.Id));
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
                        var db_child = _taskRepositoryBase.GetById(childTask.Id, true);
                        if (db_child == null)
                        {
                            return Result<TaskUnit>.Failure(ModelError<TaskUnit>.NotFound(childTask.Id));
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
                        var db_parent = _taskRepositoryBase.GetById(parentTask.Id, true);
                        if (db_parent == null)
                        {
                            return Result<TaskUnit>.Failure(ModelError<TaskUnit>.NotFound(parentTask.Id));
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
                    var proj = _projectRepositoryBase.GetById(taskUnit.Project.Id, true);
                    if (proj == null)
                    {
                        return Result<TaskUnit>.Failure(ModelError<Project>.NotFound(taskUnit.Project.Id));
                    }
                    task_db.Project = proj;
                }
                else
                {
                    return Result<TaskUnit>.Failure(ModelError<Project>.NullReference);
                }

                if (_taskRepositoryBase.Update(task_db))
                {
                    return Result<TaskUnit>.Success();
                }

                return Result<TaskUnit>.Failure(ModelError<TaskUnit>.ServerError);

            }
        }



    }
}
