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

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        // GET: api/Projects
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Project>), 200)]
        public IActionResult GetAllProjects()
        {
            var result = _projectService.GetAllProjects(false);
            if (result.IsSuccess)
            {
                return Ok(result.ModelDTOs);
            }
            else return BadRequest(result.Error);
        }

        // GET: api/Projects/5
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(Project), 200)]
        public IActionResult GetProjectById(long id)
        {
            var result = _projectService.GetProjectById(id,false);
            if (result.IsSuccess)
            {
                return Ok(result.ModelDTO);
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
                return Ok(result.ModelDTOs);
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
                return Ok(result.ModelDTOs);
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
                return Ok(result.ModelDTOs);
            }
            else return BadRequest(result.Error);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult AddProject([FromBody] CreateProjectDTO projectCreate)
        {
            //var project = _mapper.Map<Project>(projectCreate);

            var result = _projectService.AddProject(projectCreate);
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
        public IActionResult UpdateProject(int Id, [FromBody] CreateProjectDTO ProjectUpdate)
        {

            //var project = _mapper.Map<Project>(ProjectUpdate);

            var result = _projectService.UpdateProject(Id, ProjectUpdate);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok("Success!");
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

            return Ok("Success!");
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

            return Ok("Success!");
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

            return Ok("Success!");
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

            return Ok("Success!");
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

            return Ok("Success!");
        }
    }
}
