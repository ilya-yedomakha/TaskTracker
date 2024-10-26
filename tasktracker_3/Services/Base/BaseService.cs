using tasktracker_3.Help.Result;
using tasktracker_3.Help.Result.ModelErrors;
using tasktracker_3.Interfaces;
using tasktracker_3.Models;
using tasktracker_3.Repository.Base;

namespace tasktracker_3.Services.Base
{
    public class BaseService<T>
        where T : BaseModel
    {
        protected readonly IProjectRepository _projectRepository;
        protected readonly IWorkerRepository _workerRepository;
        protected readonly ITaskUnitRepository _taskUnitRepository;
        protected readonly BaseRepository<TaskUnit> _taskRepositoryBase;
        protected readonly BaseRepository<Worker> _workerRepositoryBase;
        protected readonly BaseRepository<Project> _projectRepositoryBase;
        protected readonly BaseRepository<T> _baseRepository;
        public BaseService(BaseRepository<T> baseRepository, BaseRepository<TaskUnit> taskRepositoryBase, BaseRepository<Project> projectRepositoryBase, BaseRepository<Worker> workerRepositoryBase, ITaskUnitRepository taskUnitRepository, IWorkerRepository workerRepository, IProjectRepository projectRepository)
        {
            _projectRepositoryBase = projectRepositoryBase;
            _workerRepositoryBase = workerRepositoryBase;
            _taskRepositoryBase = taskRepositoryBase;
            _projectRepository = projectRepository;
            _workerRepository = workerRepository;
            _taskUnitRepository = taskUnitRepository;
            _baseRepository = baseRepository;
        }

        public Result<T> GetById(long id, bool includes)
        {
            var model = _baseRepository.GetById(id, includes);
            if (model != null)
            {
                var res = Result<T>.Success();
                res.Model = model;
                return res;
            }
            else return Result<T>.Failure(ModelError<T>.NotFound(id));
        }

        public Result<T> GetAll(bool includes)
        {
            var models = _baseRepository.GetAll(includes);
            var res = Result<T>.Success();
            res.Models = models.ToList();
            return res;
        }

        public bool ModelExists(long id)
        {
            return _baseRepository.ModelExists(id);
        }
    }
}
