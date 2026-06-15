using AutoMapper;
using Microsoft.AspNetCore.Mvc;  
using StudentDemo.Data;
using StudentDemo.DTO;
using StudentDemo.Models;


namespace StudentManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly IMapper _mapper;

        public StudentController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetStudents()
        {
            var students = _context.Students.ToList();

            var result = _mapper.Map<List<StudentDTO>>(students);

            return Ok(result);
        }


        [HttpPost]
        public IActionResult AddStudent(CreateStudentDTO dto)
        {
            var student = _mapper.Map<Student>(dto);

            _context.Students.Add(student);
            _context.SaveChanges();

            return Ok(student);
        }
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
