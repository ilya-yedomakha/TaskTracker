using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using tasktracker_3.DTO;
using tasktracker_3.Interfaces.Services;
using tasktracker_3.Models;

namespace tasktracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProjectService _projectService;
        private readonly IWorkerService _workerService;
        private readonly ITaskUnitService _taskUnitService;

        public WorkersController(IWorkerService workerService, ITaskUnitService taskService, IProjectService projectService, IMapper mapper)
        {
            _projectService = projectService;
            _workerService = workerService;
            _taskUnitService = taskService;
            _mapper = mapper;
        }

        // GET: api/Workers
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Worker>), 200)]
        public IActionResult GetWorkers()
        {
            var workers = _mapper.Map<List<WorkerDTO>>(_workerService.GetWorkers());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(workers);
        }

        // GET: api/Workers/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Worker), 200)]
        public IActionResult GetTask(long id)
        {
            var worker = _workerService.GetWorker(id);
            if (worker == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(_mapper.Map<WorkerDTO>(worker));
        }

        [HttpGet("{id:long}/Projects")]
        [ProducesResponseType(typeof(IEnumerable<Worker>), 200)]
        public IActionResult GetWorkerProjects(long id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var workersprojects = _workerService.GetWorkerProjects(id);

            if(workersprojects == null)
            {
                return NotFound("There is no such task");
            }

            return Ok(_mapper.Map<List<ProjectDTO>>(workersprojects));
        }

        [HttpGet("{id:long}/Tasks")]
        [ProducesResponseType(typeof(IEnumerable<TaskUnit>), 200)]
        public IActionResult GetWorkerTasks(long id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var workertasks = _workerService.GetWorkerTasks(id);
            if (workertasks == null)
            {
                return NotFound("There is no such task");
            }

            return Ok(_mapper.Map<List<TaskUnitDTO>>(workertasks));
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult AddWorker([FromBody] CreateWorkerDTO workerCreate)
        {
            if (workerCreate == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var worker = _mapper.Map<Worker>(workerCreate);

            var result = _workerService.AddWorker(worker);

            return result;
        }

        [HttpPut("{Id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult UpdateWorker(int Id, [FromBody] CreateWorkerDTO workerUpdate)
        {
            if (workerUpdate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var worker = _mapper.Map<Worker>(workerUpdate);

            var result = _workerService.UpdateWorker(Id, worker);
            return result;
        }

        [HttpPut("{workerId}/Tasks/{taskId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult AddTaskToWorker(long workerId, long taskId)
        {
            var result = _workerService.AddTaskToWorker(workerId, taskId);
            return result;
        }

        [HttpPut("{workerId}/Projects/{projectId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult AddProjectToWorker(long workerId, long projectId)
        {
            var result = _workerService.AddProjectToWorker(workerId, projectId);
            return result;
        }

        [HttpDelete("{workerId}/Tasks/{taskId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult RemoveTaskFromWorker(long workerId, long taskId)
        {
            var result = _workerService.RemoveTaskFromWorker(workerId, taskId);
            return result;
        }

        [HttpDelete("{workerId}/Projects/{projectId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult RemoveProjectFromWorker(long workerId, long projectId)
        {
            var result = _workerService.RemoveProjectFromWorker(workerId, projectId);
            return result;
        }

        [HttpDelete("{Id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult DeleteWorker(int Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _workerService.DeleteWorker(Id);
            return result;
        }
    }
}
