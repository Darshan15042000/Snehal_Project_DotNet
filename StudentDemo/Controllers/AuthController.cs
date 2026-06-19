using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentDemo.Data;
using StudentDemo.DTO;
using StudentDemo.Models;
using StudentDemo.Services;
//using StudentDemo.Services;

namespace StudentDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly AppDbContext _context;
        private readonly ISupportClientService _service;
       
        private readonly IDeveloperService _devService;

        //Dependency Injection (Constructor Based)
        public AuthController(
            JwtService jwtService,
            AppDbContext context,
            ISupportClientService service,
            IDeveloperService devService)
        {
            _jwtService = jwtService;
            _context = context;
            _service = service;
            _devService = devService;
        }
        [HttpPost("/register_for_support")]
        public async Task<IActionResult> Register(
            SupportClientRegisterDTO dto)
        {
            var result = await _service.RegisterClient(dto);

            return Ok(result);
        }


        [HttpPost("register_for_Developer")]
        public async Task<IActionResult> Register(
            [FromBody] DeveloperDTO dto)
        {
            var result = await _devService.RegisterDeveloper(dto);

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(Admin student)
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