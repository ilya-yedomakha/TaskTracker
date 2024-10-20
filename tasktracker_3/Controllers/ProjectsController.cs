using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using tasktracker_3.DTO;
using tasktracker_3.Interfaces.Services;
using tasktracker_3.Models;

namespace tasktracker_3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IWorkerService _workerService;
        private readonly ITaskUnitService _taskUnitService;
        private readonly IMapper _mapper;

        public ProjectsController(IWorkerService workerService, ITaskUnitService taskService, IProjectService projectService, IMapper mapper)
        {
            _projectService = projectService;
            _workerService = workerService;
            _taskUnitService = taskService;
            _mapper = mapper;
        }

        // GET: api/Projects
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Project>), 200)]
        public IActionResult GetProjects()
        {
            var projects = _mapper.Map<List<ProjectDTO>>(_projectService.GetProjects());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(projects);
        }

        // GET: api/Projects/5
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(Project), 200)]
        public IActionResult GetProject(long id)
        {
            var project = _projectService.GetProject(id);

            if (project == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(_mapper.Map<ProjectDTO>(project));
        }

        [HttpGet("{name}")]
        [ProducesResponseType(typeof(IEnumerable<Project>), 200)]
        public IActionResult GetTasks(string name)
        {
            var projects = _projectService.GetProjects(name);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(_mapper.Map<List<ProjectDTO>>(projects));
        }

        [HttpGet("{id:long}/Workers")]
        [ProducesResponseType(typeof(IEnumerable<Worker>), 200)]
        public IActionResult GetTaskWorkers(long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var projectworkers = _projectService.GetProjectWorkers(id);
            if (projectworkers == null)
            {
                return NotFound("There is no such project");
            }

            return Ok(_mapper.Map<List<WorkerDTO>>(projectworkers));
        }

        [HttpGet("{id:long}/Tasks")]
        [ProducesResponseType(typeof(IEnumerable<TaskUnit>), 200)]
        public IActionResult GetProjectTasks(long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var projectTasks = _projectService.GetProjectTasks(id);
            if (projectTasks == null)
            {
                return NotFound("There is no such project");
            }

            return Ok(_mapper.Map<List<TaskUnitDTO>>(projectTasks));
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult AddProject([FromBody] CreateProjectDTO projectCreate)
        {
            if (projectCreate == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = _mapper.Map<Project>(projectCreate);

            var result = _projectService.AddProject(project);
            return result;
        }

        [HttpPut("{Id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult UpdateProject(int Id, [FromBody] CreateProjectDTO ProjectUpdate)
        {
            if (ProjectUpdate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = _mapper.Map<Project>(ProjectUpdate);

            var result = _projectService.UpdateProject(Id, project);
            return result;
        }

        [HttpPut("{projectId}/Workers/{workerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult AddWorkerToProject(long projectId, long workerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _projectService.AddWorkerToProject(projectId, workerId);
            return result;
        }

        [HttpPut("{projectId}/Tasks/{taskId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult AddTaskToProject(long projectId, long taskId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _projectService.AddTaskToProject(projectId, taskId);
            return result;
        }

        [HttpDelete("{projectId}/Workers/{workerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult RemoveWorkerFromProject(long projectId, long workerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _projectService.RemoveWorkerFromProject(projectId, workerId);
            return result;
        }

        [HttpDelete("{projectId}/Tasks/{taskId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult RemoveTaskFromProject(long projectId, long taskId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _projectService.RemoveTaskFromProject(projectId, taskId);
            return result;
        }

        [HttpDelete("{Id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult DeleteProject(int Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _projectService.DeleteProject(Id);
            return result;
        }
    }
}
