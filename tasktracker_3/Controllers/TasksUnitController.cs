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

        public TasksUnitController(
            ITaskUnitService taskUnitService)
        {
            _taskUnitService = taskUnitService;
        }

        // GET: api/Tasks
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TaskUnit>), 200)]
        public IActionResult GetAllTasks()
        {
            var result = _taskUnitService.GetAllTasks(false);
            if (result.IsSuccess)
            {
                return Ok(result.ModelDTOs);
            }
            else return BadRequest(result.Error);
        }

        // GET: api/Tasks/5
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(TaskUnit), 200)]
        public IActionResult GetTaskById(long id)
        {
            var result = _taskUnitService.GetTaskUnitById(id, false);
            if (result.IsSuccess)
            {
                return Ok(result.ModelDTO);
            }
            else return BadRequest(result.Error);
        }

        // GET: api/Tasks/5
        [HttpGet("{id:long}/Workers")]
        [ProducesResponseType(typeof(IEnumerable<Worker>), 200)]
        public IActionResult GetTaskWorkers(long id)
        {
            var result = _taskUnitService.GetTaskWorkers(id);
            if (result.IsSuccess)
            {
                return Ok(result.ModelDTOs);
            }
            else return BadRequest(result.Error);
        }

        // GET: api/Tasks/5
        [HttpGet("{id:long}/ParentsTasks")]
        [ProducesResponseType(typeof(IEnumerable<Worker>), 200)]
        public IActionResult GetParentsTasks(long id)
        {
            var result = _taskUnitService.GetParentsOfTask(id);
            if (result.IsSuccess)
            {
                return Ok(result.ModelDTOs);
            }
            else return BadRequest(result.Error);
        }

        // GET: api/Tasks/5
        [HttpGet("{id:long}/ChildrenTasks")]
        [ProducesResponseType(typeof(IEnumerable<Worker>), 200)]
        public IActionResult GetChildrenTasks(long id)
        {
            var result = _taskUnitService.GetChildrenOfTask(id);
            if (result.IsSuccess)
            {
                return Ok(result.ModelDTOs);
            }
            else return BadRequest(result.Error);
        }

        [HttpGet("{id:long}/Project")]
        [ProducesResponseType(typeof(Project), 200)]
        public IActionResult GetTaskProject(long id)
        {
            var result = _taskUnitService.GetTaskProject(id);

            if (result.IsSuccess)
            {
                return Ok(result.ModelDTO);
            }
            else return BadRequest(result.Error);
        }

        [HttpGet("title/{title}")]
        [ProducesResponseType(typeof(IEnumerable<TaskUnit>), 200)]
        public IActionResult GetAllTasks(string title)
        {
            var result = _taskUnitService.GetTasks(title);

            return Ok(result.ModelDTOs);
        }


        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult AddTask([FromBody] CreateTaskUnitDTO taskUnitCreate)
        {

            var result = _taskUnitService.AddTask(taskUnitCreate);
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
        public IActionResult UpdateTask(long Id, [FromBody] CreateTaskUnitDTO taskUnitUpdate)
        {
            //var taskUnit = _mapper.Map<TaskUnit>(taskUnitUpdate);

            var result = _taskUnitService.UpdateTask(Id, taskUnitUpdate);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok("Success!");
        }

        [HttpPut("{taskId}/Workers/{workerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult AddWorkerToTask(long taskId, long workerId)
        {
            var result = _taskUnitService.AddWorkerToTask(taskId, workerId);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok("Success!");
        }

        [HttpPut("{taskId}/Tasks/{childTaskId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult AddChildTaskToTask(long taskId, long childTaskId)
        {
            var result = _taskUnitService.AddChildTaskToTask(taskId, childTaskId);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok("Success!");
        }


        [HttpDelete("{taskId}/Tasks/{childTaskId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult RemoveChildTaskFromTask(long taskId, long childTaskId)
        {
            var result = _taskUnitService.RemoveChildTaskFromTask(taskId, childTaskId);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok("Success!");
        }

        [HttpDelete("{taskId}/Workers/{workerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult RemoveWorkerFromTask(long taskId, long workerId)
        {
            var result = _taskUnitService.RemoveWorkerFromTask(taskId, workerId);

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
        public IActionResult DeleteTask(int Id)
        {
            var result = _taskUnitService.DeleteTask(Id);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok("Success!");
        }
    }
}
