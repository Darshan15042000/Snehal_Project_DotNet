using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;  
using StudentDemo.Data;
using StudentDemo.DTO;
using StudentDemo.Models;


namespace StudentDemo.Controllers
{
    [ApiController]
    [Route("api/Admin")]
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly IMapper _mapper;

        public StudentController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet("/TestApi")]
        public IActionResult TestApi()
        {
            return Ok("Only Logged In Users Can Access");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getUserData")]
        public IActionResult GetUserData()
        {
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            if (role != "Admin")
            {
                return StatusCode(403, new
                {
                    Message = "Access Denied. Only Admin can access this API."
                });
            }

            var students = _context.Students.ToList();

            var result = _mapper.Map<List<AdminDTO>>(students);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]  
        [HttpPost]
        public IActionResult AddStudent(CreateStudentDTO dto)
        {
            var student = _mapper.Map<Admin>(dto);

            _context.Students.Add(student);

            _context.SaveChanges();

            return Ok(student);
        }

        [Authorize(Roles = "Admin")]
        // Update an existing student data
        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id, [FromBody] UpdateStudentDTO dto)
        {
            var existingStudent = _context.Students.Find(id);

            if (existingStudent == null)
            {
                return NotFound("Student not found");
            }

            _mapper.Map(dto, existingStudent);

            _context.SaveChanges();

            return Ok(existingStudent);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            var existingStudent = _context.Students.Find(id);

            if (existingStudent == null)
            {
                return NotFound("Student not found");
            }

            _context.Students.Remove(existingStudent);
            _context.SaveChanges();

            return Ok("Student Deleted Successfully");
        }


    }
}
