using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using tasktracker_3.DTO;
using tasktracker_3.Interfaces.Services;
using tasktracker_3.Models;
using tasktracker_3.Services.Base;

namespace tasktracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProjectService _projectService;
        private readonly IWorkerService _workerService;
        private readonly BaseService<Worker> _workerServiceBase;
        private readonly ITaskUnitService _taskUnitService;

        public WorkersController(BaseService<Worker> workerServiceBase, IWorkerService workerService, ITaskUnitService taskService, IProjectService projectService, IMapper mapper)
        {
            _workerServiceBase = workerServiceBase;
            _projectService = projectService;
            _workerService = workerService;
            _taskUnitService = taskService;
            _mapper = mapper;
        }

        // GET: api/Workers
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Worker>), 200)]
        public IActionResult GetAllWorkers()
        {
            var result = _workerServiceBase.GetAll(false);
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<List<WorkerDTO>>(result.Models));
            }
            else return BadRequest(result.Error);

        }

        // GET: api/Workers/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Worker), 200)]
        public IActionResult GetWorkerById(long id)
        {
            var result = _workerServiceBase.GetById(id, false);
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<WorkerDTO>(result.Model));

            }
            else return BadRequest(result.Error);
        }

        [HttpGet("{id:long}/Projects")]
        [ProducesResponseType(typeof(IEnumerable<Worker>), 200)]
        public IActionResult GetWorkerProjects(long id)
        {
            var result = _workerService.GetWorkerProjects(id);
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<List<ProjectDTO>>(result.Models));
            }
            else return BadRequest(result.Error);
        }

        [HttpGet("{id:long}/Tasks")]
        [ProducesResponseType(typeof(IEnumerable<TaskUnit>), 200)]
        public IActionResult GetWorkerTasks(long id)
        {
            var result = _workerService.GetWorkerTasks(id);
            if (result.IsSuccess)
            {

                return Ok(_mapper.Map<List<TaskUnitDTO>>(result.Models));
            }
            else return BadRequest(result.Error);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult AddWorker([FromBody] CreateWorkerDTO workerCreate)
        {

            var worker = _mapper.Map<Worker>(workerCreate);

            var result = _workerService.AddWorker(worker);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok("Success!");
        }

        [HttpPut("{Id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult UpdateWorker(int Id, [FromBody] CreateWorkerDTO workerUpdate)
        {
            var worker = _mapper.Map<Worker>(workerUpdate);

            var result = _workerService.UpdateWorker(Id, worker);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok("Success!");
        }

        [HttpPut("{workerId}/Tasks/{taskId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult AddTaskToWorker(long workerId, long taskId)
        {
            var result = _workerService.AddTaskToWorker(workerId, taskId);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok("Success!");
        }

        [HttpPut("{workerId}/Projects/{projectId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult AddProjectToWorker(long workerId, long projectId)
        {
            var result = _workerService.AddProjectToWorker(workerId, projectId);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok("Success!");
        }

        [HttpDelete("{workerId}/Tasks/{taskId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult RemoveTaskFromWorker(long workerId, long taskId)
        {
            var result = _workerService.RemoveTaskFromWorker(workerId, taskId);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok("Success!");
        }

        [HttpDelete("{workerId}/Projects/{projectId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult RemoveProjectFromWorker(long workerId, long projectId)
        {
            var result = _workerService.RemoveProjectFromWorker(workerId, projectId);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok("Success!");
        }

        [HttpDelete("{Id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult DeleteWorker(int Id)
        {
            var result = _workerService.DeleteWorker(Id);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok("Success!");
        }
    }
}
