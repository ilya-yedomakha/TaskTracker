using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using tasktracker_3.DTO;
using tasktracker_3.Interfaces.Services;
using tasktracker_3.Models;

namespace tasktracker_3.Controllers
{
    [Route("api/Tasks")]
    [ApiController]
    public class TasksUnitController : Controller
    {
        //private readonly DataContext _context;
        private readonly ITaskUnitService _taskUnitService;
        private readonly IWorkerService _workerService;
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;

        public TasksUnitController(IProjectService projectService, IWorkerService workerService, ITaskUnitService
            taskUnitService, IMapper mapper)
        {
            _workerService = workerService;
            _taskUnitService = taskUnitService;
            _projectService = projectService;
            _mapper = mapper;
        }

        // GET: api/Tasks
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TaskUnit>), 200)]
        public IActionResult GetTasks()
        {
            var taskUnits = _taskUnitService.GetTasks();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(_mapper.Map<List<TaskUnitDTO>>(taskUnits));
        }

        // GET: api/Tasks/5
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(TaskUnit), 200)]
        public IActionResult GetTask(long id)
        {
            var taskUnit = _taskUnitService.GetTask(id);

            if (taskUnit == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            return Ok(_mapper.Map<TaskUnitDTO>(taskUnit));
        }

        // GET: api/Tasks/5
        [HttpGet("{id:long}/Workers")]
        [ProducesResponseType(typeof(IEnumerable<Worker>), 200)]
        public IActionResult GetTaskWorkers(long id)
        {
            var taskUnit = _taskUnitService.TaskExists(id);

            if (!taskUnit)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(_mapper.Map<List<WorkerDTO>>(_taskUnitService.GetTaskWorkers(id)));
        }

        // GET: api/Tasks/5
        [HttpGet("{id:long}/ParentsTasks")]
        [ProducesResponseType(typeof(IEnumerable<Worker>), 200)]
        public IActionResult GetParentsTasks(long id)
        {
            var taskUnit = _taskUnitService.TaskExists(id);

            if (!taskUnit)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(_mapper.Map<List<TaskUnitDTO>>(_taskUnitService.GetParentsOfTask(id)));
        }

        // GET: api/Tasks/5
        [HttpGet("{id:long}/ChildrenTasks")]
        [ProducesResponseType(typeof(IEnumerable<Worker>), 200)]
        public IActionResult GetChildrenTasks(long id)
        {
            var taskUnit = _taskUnitService.TaskExists(id);

            if (!taskUnit)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(_mapper.Map<List<TaskUnitDTO>>(_taskUnitService.GetChildrenOfTask(id)));
        }

        [HttpGet("{id:long}/Project")]
        [ProducesResponseType(typeof(Project), 200)]
        public IActionResult GetTaskProject(long id)
        {
            var taskUnit = _taskUnitService.TaskExists(id);

            if (!taskUnit)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(_mapper.Map<ProjectDTO>(_taskUnitService.GetTaskProject(id)));
        }

        [HttpGet("title/{title}")]
        [ProducesResponseType(typeof(IEnumerable<TaskUnit>), 200)]
        public IActionResult GetTasks(string title)
        {
            var taskUnits = _taskUnitService.GetTasks(title);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(_mapper.Map<List<TaskUnitDTO>>(taskUnits));
        }


        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult AddTask([FromBody] CreateTaskUnitDTO taskUnitCreate)
        {
            if (taskUnitCreate == null)
            {
                return BadRequest(ModelState);
            }


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var taskUnit = _mapper.Map<TaskUnit>(taskUnitCreate);

            var result = _taskUnitService.AddTask(taskUnit);

            return result;
        }

        [HttpPut("{Id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult UpdateTask(long Id, [FromBody] CreateTaskUnitDTO taskUnitUpdate)
        {
            if (taskUnitUpdate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var taskUnit = _mapper.Map<TaskUnit>(taskUnitUpdate);

            var result = _taskUnitService.UpdateTask(Id, taskUnit);

            return result;
        }

        [HttpPut("{taskId}/Workers/{workerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult AddWorkerToTask(long taskId, long workerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _taskUnitService.AddWorkerToTask(taskId, workerId);

            return result;
        }

        [HttpPut("{taskId}/Tasks/{childTaskId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult AddChildTaskToTask(long taskId, long childTaskId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _taskUnitService.AddChildTaskToTask(taskId,childTaskId);

            return result;
        }


        [HttpDelete("{taskId}/Tasks/{childTaskId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult RemoveChildTaskFromTask(long taskId, long childTaskId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _taskUnitService.RemoveChildTaskFromTask(taskId, childTaskId);

            return result;
        }

        [HttpDelete("{taskId}/Workers/{workerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult RemoveWorkerFromTask(long taskId, long workerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _taskUnitService.RemoveWorkerFromTask(taskId, workerId);

            return result;
        }

        [HttpDelete("{Id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult DeleteTask(int Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _taskUnitService.DeleteTask(Id);

            return result;
        }
    }
}
