using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using tasktracker_3.DTO;
using tasktracker_3.Interfaces.Services;
using tasktracker_3.Models;
using tasktracker_3.Services.Base;

namespace tasktracker_3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IWorkerService _workerService;
        private readonly ITaskUnitService _taskUnitService;
        private readonly BaseService<Project> _projectServiceBase;
        private readonly IMapper _mapper;

        public ProjectsController(BaseService<Project> projectServiceBase, IWorkerService workerService, ITaskUnitService taskService, IProjectService projectService, IMapper mapper)
        {
            _projectServiceBase = projectServiceBase;
            _projectService = projectService;
            _workerService = workerService;
            _taskUnitService = taskService;
            _mapper = mapper;
        }

        // GET: api/Projects
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Project>), 200)]
        public IActionResult GetAllProjects()
        {
            var result = _projectServiceBase.GetAll(false);
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<List<ProjectDTO>>(result.Models));
            }
            else return BadRequest(result.Error);
        }

        // GET: api/Projects/5
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(Project), 200)]
        public IActionResult GetProjectById(long id)
        {
            var result = _projectServiceBase.GetById(id,false);
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<ProjectDTO>(result.Model));
            }
            else return BadRequest(result.Error);
        }

        [HttpGet("{name}")]
        [ProducesResponseType(typeof(IEnumerable<Project>), 200)]
        public IActionResult GetProjectsByName(string name)
        {
            var result = _projectService.GetProjects(name);
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<List<ProjectDTO>>(result.Models));
            }
            else return BadRequest(result.Error);
        }

        [HttpGet("{id:long}/Workers")]
        [ProducesResponseType(typeof(IEnumerable<Worker>), 200)]
        public IActionResult GetTaskWorkers(long id)
        {
            var result = _projectService.GetProjectWorkers(id);
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<List<WorkerDTO>>(result.Models));
            }
            else return BadRequest(result.Error);
        }

        [HttpGet("{id:long}/Tasks")]
        [ProducesResponseType(typeof(IEnumerable<TaskUnit>), 200)]
        public IActionResult GetProjectTasks(long id)
        {
            var result = _projectService.GetProjectTasks(id);
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<List<TaskUnitDTO>>(result.Models));
            }
            else return BadRequest(result.Error);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult AddProject([FromBody] CreateProjectDTO projectCreate)
        {
            var project = _mapper.Map<Project>(projectCreate);

            var result = _projectService.AddProject(project);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return NoContent();
        }

        [HttpPut("{Id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult UpdateProject(int Id, [FromBody] CreateProjectDTO ProjectUpdate)
        {

            var project = _mapper.Map<Project>(ProjectUpdate);

            var result = _projectService.UpdateProject(Id, project);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return NoContent();
        }

        [HttpPut("{projectId}/Workers/{workerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult AddWorkerToProject(long projectId, long workerId)
        {
            var result = _projectService.AddWorkerToProject(projectId, workerId);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return NoContent();
        }

        [HttpPut("{projectId}/Tasks/{taskId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult AddTaskToProject(long projectId, long taskId)
        {
            var result = _projectService.AddTaskToProject(projectId, taskId);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return NoContent();
        }

        [HttpDelete("{projectId}/Workers/{workerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult RemoveWorkerFromProject(long projectId, long workerId)
        {
            var result = _projectService.RemoveWorkerFromProject(projectId, workerId);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return NoContent();
        }

        [HttpDelete("{projectId}/Tasks/{taskId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult RemoveTaskFromProject(long projectId, long taskId)
        {
            var result = _projectService.RemoveTaskFromProject(projectId, taskId);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return NoContent();
        }

        [HttpDelete("{Id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult DeleteProject(int Id)
        {
            var result = _projectService.DeleteProject(Id);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return NoContent();
        }
    }
}
