using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice.DTOs;
using Practice.Models;

namespace Practice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TasksController(TimeManagementDBContext context, IMapper mapper) : ControllerBase
    {
        private readonly TimeManagementDBContext _context = context;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDTO>>> GetTasks(
            [FromQuery] string? name = null,
            [FromQuery] int? projectId = null,
            [FromQuery] bool? active = null)
        {
            var query = _context.Tasks.AsQueryable();

            if (name != null)
            {
                name = name.ToLower();
                query = query.Where(t => 
                t.Name.ToLower()
                .Contains(name));
            }

            if(projectId.HasValue)
                query = query.Where(t => t.ProjectId == projectId.Value);

            if(active.HasValue)
                query = query.Where(t => t.IsActive == active.Value);

            var tasks = await query.ToListAsync();

            var tasksDTO = _mapper.Map<List<TaskDTO>>(tasks);

            return Ok(tasksDTO);
        }

        [HttpGet("{id}", Name = "GetTaskById")]
        public async Task<ActionResult<TaskDTO>> GetTaskById(int id)
        {
            if (id < 0)
                return BadRequest(ErrorManager<Models.Task>.InvalidId);

            var targetTask = await _context.Tasks.Include(t => t.Project).FirstOrDefaultAsync(t => t.Id == id);

            if (targetTask == null)
                return NotFound(ErrorManager<Models.Task>.EntityNotFound);

            var targetTaskDTO = _mapper.Map<TaskDTO>(targetTask);

            return Ok(targetTaskDTO);
        }

        [HttpPost]
        public async Task<ActionResult<TaskDTO>> CreateNewTask([FromBody] TaskDTO taskDTO)
        {
            if (taskDTO.Id < 0)
                return BadRequest(ErrorManager<Models.Task>.InvalidId);

            if (_context.Tasks.Any(t => t.Id == taskDTO.Id))
                return Conflict(ErrorManager<Models.Task>.EntityAlreadyExists);

            var targetProject = await _context.Projects.FindAsync(taskDTO.ProjectId);

            if (targetProject == null)
                return NotFound(ErrorManager<Project>.EntityNotFound);

            var domainTask = _mapper.Map<Models.Task>(taskDTO);

            domainTask.Project = targetProject;

            await _context.Tasks.AddAsync(domainTask);
            await _context.SaveChangesAsync();

            taskDTO.Id = domainTask.Id;

            return CreatedAtRoute("GetTaskById", new { taskDTO.Id }, taskDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TaskDTO>> EditTask(int id, [FromBody] TaskDTO taskDTO)
        {
            if (id < 0 || id != taskDTO.Id)
                return BadRequest(ErrorManager<Models.Task>.InvalidId);

            var targetTask = await _context.Tasks.FindAsync(id);

            if (targetTask == null)
                return NotFound(ErrorManager<Models.Task>.EntityNotFound);

            _mapper.Map(taskDTO, targetTask);

            await _context.SaveChangesAsync();

            return Ok(taskDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTaskById(int id)
        {
            if (id < 0)
                return BadRequest(ErrorManager<Models.Task>.InvalidId);

            var targetTask = await _context.Tasks.FindAsync(id);

            if (targetTask == null)
                return NotFound(ErrorManager<Models.Task>.EntityNotFound);

            _context.Tasks.Remove(targetTask);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
