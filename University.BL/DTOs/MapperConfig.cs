using AutoMapper;
using University.BL.Models;

namespace University.BL.DTOs
{
    public class MapperConfig
    {
        public static MapperConfiguration MapperConfiguration()
        {
            //  Model(DB) -> DTO
            //  DTO -> Model(DB)

            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Student, StudentDTO>();
                cfg.CreateMap<StudentDTO, Student>();

                cfg.CreateMap<Course, CourseDTO>();
                cfg.CreateMap<CourseDTO, Course>();
            });
        }
    }
}
