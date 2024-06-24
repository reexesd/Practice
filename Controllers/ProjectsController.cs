using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice.DTOs;
using Practice.Models;

namespace Practice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectsController(TimeManagementDBContext context, IMapper mapper) : ControllerBase
    {
        private readonly TimeManagementDBContext _context = context;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDTO>>> GetProjects(
            [FromQuery] string? name = null,
            [FromQuery] bool? active = null)
        {
            var query = _context.Projects.AsQueryable();

            if (active.HasValue)
                query = query.Where(p => p.IsActive == active.Value);

            if (name != null)
            {
                name = name.ToLower();
                query = query.Where(p => 
                p.Name.ToLower()
                .Contains(name));
            }

            var projects = await query.ToListAsync();

            var projectsDTO = _mapper.Map<List<ProjectDTO>>(projects);

            return Ok(projectsDTO);
        }

        [HttpGet("{id}", Name = "GetProjectById")]
        public async Task<ActionResult<ProjectDTO>> GetProjectById(int id)
        {
            if (id < 0)
                return BadRequest(ErrorManager<Project>.InvalidId);

            var targetProject = await _context.Projects.FindAsync(id);

            if (targetProject == null)
                return NotFound(ErrorManager<Project>.EntityNotFound);

            var projectDTO = _mapper.Map<ProjectDTO>(targetProject);

            return Ok(projectDTO);
        }

        [HttpPost]
        public async Task<ActionResult<ProjectDTO>> CreateNewProject([FromBody] ProjectDTO projectDTO)
        {
            if (projectDTO.Id < 0)
                return BadRequest(ErrorManager<Project>.InvalidId);

            if (_context.Projects.Any(p => p.Id == projectDTO.Id))
                return Conflict(ErrorManager<Project>.EntityAlreadyExists);

            var domainProject = _mapper.Map<Project>(projectDTO);

            await _context.Projects.AddAsync(domainProject);
            await _context.SaveChangesAsync();

            projectDTO.Id = domainProject.Id;

            return CreatedAtRoute("GetProjectById", new { projectDTO.Id }, projectDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProjectDTO>> EditProjectById(int id, [FromBody] ProjectDTO projectDTO)
        {
            if (id < 0 || id != projectDTO.Id)
                return BadRequest(ErrorManager<Project>.InvalidId);

            var targetProject = await _context.Projects.FindAsync(id);

            if (targetProject == null)
                return NotFound(ErrorManager<Project>.EntityNotFound);

            _mapper.Map(projectDTO, targetProject);

            await _context.SaveChangesAsync();

            return Ok(projectDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProjectById(int id)
        {
            if (id < 0)
                return BadRequest(ErrorManager<Project>.InvalidId);

            var targetProject = await _context.Projects.FindAsync(id);

            if (targetProject == null)
                return NotFound(ErrorManager<Project>.EntityNotFound);

            _context.Projects.Remove(targetProject);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
