using AutoMapper;
using StudentDemo.DTO;
using StudentDemo.Models;

namespace StudentDemo.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Student, StudentDTO>();

            CreateMap<CreateStudentDTO, Student>();

            CreateMap<UpdateStudentDTO, Student>();
        }
    }
}
