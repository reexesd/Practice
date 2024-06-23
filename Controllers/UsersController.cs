using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice.DTOs;
using Practice.Models;

namespace Practice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController(TimeManagementDBContext context, IMapper mapper) : ControllerBase
    {
        private readonly TimeManagementDBContext _context = context;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            var usersDTO = _mapper.Map<List<UserDTO>>(users);

            return Ok(usersDTO);
        }

        [HttpGet("{id}", Name = "GetUserById")]
        public async Task<ActionResult<UserDTO>> GetUserById(int id)
        {
            if (id < 0)
                return BadRequest(ErrorManager<User>.InvalidId);

            var targetUser = await _context.Users.FindAsync(id);

            if (targetUser == null)
                return NotFound(ErrorManager<User>.EntityNotFound);

            var targetUserDTO = _mapper.Map<UserDTO>(targetUser);

            return Ok(targetUserDTO);
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> RegisterNewUser([FromBody] UserDTO userDTO)
        {
            if (userDTO.Id < 0)
                return BadRequest(ErrorManager<User>.InvalidId);

            if (_context.Users.Any(u => u.Id == userDTO.Id))
                return Conflict(ErrorManager<User>.EntityAlreadyExists);

            var domainUser = _mapper.Map<User>(userDTO);

            await _context.Users.AddAsync(domainUser);
            await _context.SaveChangesAsync();

            userDTO.Id = domainUser.Id;

            return CreatedAtRoute("GetUserById", new { userDTO.Id }, userDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDTO>> EditUserById(int id, [FromBody] UserDTO userDTO)
        {
            if (id < 0 || id != userDTO.Id)
                return BadRequest(ErrorManager<User>.InvalidId);
            
            var targetUser = await _context.Users.FindAsync(id);

            if (targetUser == null)
                return NotFound(ErrorManager<User>.EntityNotFound);

            _mapper.Map(userDTO, targetUser);

            await _context.SaveChangesAsync();

            return Ok(userDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUserById(int id)
        {
            if (id < 0)
                return BadRequest(ErrorManager<User>.InvalidId);

            var targetUser = await _context.Users.FindAsync(id);

            if (targetUser == null)
                return NotFound(ErrorManager<User>.EntityNotFound);

            _context.Users.Remove(targetUser);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
