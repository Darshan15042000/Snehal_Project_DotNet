using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentDemo.Data;
using StudentDemo.Models;
//using StudentDemo.Services;

namespace StudentDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly AppDbContext _context;

        public AuthController(
            JwtService jwtService,
            AppDbContext context)
        {
            _jwtService = jwtService;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(Student student)
        {
            var existingUser = await _context.Students
                .FirstOrDefaultAsync(x => x.Email == student.Email);

            if (existingUser != null)
            {
                return BadRequest("Email already exists");
            }

            //student.Role = "User";

            _context.Students.Add(student);

            await _context.SaveChangesAsync();

            return Ok("Registration Successful");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            var user = await _context.Students
                .FirstOrDefaultAsync(x =>
                    x.Email == model.Username &&
                    x.Password == model.Password);

            if (user == null)
            {
                return Unauthorized("Invalid Email or Password");
            }

            var token = _jwtService.GenerateToken(
                user.Id,
                user.Name,
                user.Role);

            return Ok(new
            {
                Token = token
            });
        }
    }
}