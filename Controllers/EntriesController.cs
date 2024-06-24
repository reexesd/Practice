using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice.DTOs;
using Practice.Models;

namespace Practice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EntriesController(TimeManagementDBContext context, IMapper mapper): ControllerBase
    {
        private readonly TimeManagementDBContext _context = context;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EntryDTO>>> GetEntries(
            [FromQuery]int? userId = null,
            [FromQuery]int? taskId = null,
            [FromQuery]DateOnly? date = null,
            [FromQuery]DateOnly? month = null)
        {
            var query = _context.Entries.AsQueryable();

            if (userId.HasValue)
                query = query.Where(e => e.UserId == userId.Value);
            
            if (taskId.HasValue)
                query = query.Where(e => e.TaskId == taskId.Value);

            if (date.HasValue)
                query = query.Where(e => e.Date == date.Value);

            if (month.HasValue)
                query = query.Where(e => 
                e.Date.Month == month.Value.Month 
                && e.Date.Year == month.Value.Year);

            var entries = await query.ToListAsync();

            var entriesDTO = _mapper.Map<List<EntryDTO>>(entries);

            return Ok(entriesDTO);
        }

        [HttpGet("{id}", Name = "GetEntryById")]
        public async Task<ActionResult<EntryDTO>> GetEntryById(int id)
        {
            if (id < 0) 
                return BadRequest(ErrorManager<Entry>.InvalidId);

            var targetEntry = await _context.Entries.FindAsync(id);

            if (targetEntry == null)
                return NotFound(ErrorManager<Entry>.EntityNotFound);

            var targetEntryDTO = _mapper.Map<EntryDTO>(targetEntry);

            return Ok(targetEntryDTO);
        }

        [HttpPost]
        public async Task<ActionResult<EntryDTO>> CreateNewEntry([FromBody] EntryDTO entryDTO)
        {
            if (entryDTO.Id < 0)
                return BadRequest(ErrorManager<Entry>.InvalidId);

            if (_context.Entries.Any(e => e.Id == entryDTO.Id))
                return Conflict(ErrorManager<Entry>.EntityAlreadyExists);

            var targetTask = await _context.Tasks.FindAsync(entryDTO.TaskId);

            if (targetTask == null)
                return NotFound(ErrorManager<Models.Task>.EntityNotFound);

            var targetUser = await _context.Users.FindAsync(entryDTO.UserId);

            if(targetUser == null)
                return NotFound(ErrorManager<User>.EntityNotFound);

            var domainEntry = _mapper.Map<Entry>(entryDTO);

            domainEntry.Task = targetTask;
            domainEntry.User = targetUser;

            await _context.AddAsync(domainEntry);
            await _context.SaveChangesAsync();

            entryDTO.Id = domainEntry.Id;

            return CreatedAtRoute("GetEntryById", new { entryDTO.Id }, entryDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EntryDTO>> EditEntry(int id, [FromBody] EntryDTO entryDTO)
        {
            if (id < 0 || id != entryDTO.Id)
                return BadRequest(ErrorManager<Entry>.InvalidId);
            
            var targetEntry = await _context.Entries.FindAsync(id);

            if (targetEntry == null)
                return NotFound(ErrorManager<Entry>.EntityNotFound);

            _mapper.Map(entryDTO, targetEntry);

            await _context.SaveChangesAsync();

            return Ok(entryDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEntryById(int id)
        {
            if (id < 0)
                return BadRequest(ErrorManager<Entry>.InvalidId);

            var targetEntry = await _context.Entries.FindAsync(id);

            if (targetEntry == null)
                return NotFound(ErrorManager<Entry>.EntityNotFound);

            _context.Entries.Remove(targetEntry);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
