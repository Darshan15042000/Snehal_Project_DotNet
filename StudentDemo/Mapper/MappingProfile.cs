using AutoMapper;
using StudentDemo.DTO;
using StudentDemo.Models;

namespace StudentDemo.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Admin, AdminDTO>();

            CreateMap<CreateStudentDTO, Admin>();

            CreateMap<UpdateStudentDTO, Admin>();
        }
    }
}
