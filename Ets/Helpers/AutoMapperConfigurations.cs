using AutoMapper;
using Ets.Models.Dto;
using Ets.Models.Dto.Student;
using Ets.Models.Entities;

namespace Ets.Helpers
{
    public class AutoMapperConfigurations : Profile
    {
        public AutoMapperConfigurations()
        {
            CreateMap<Exams, ExamDto>().ReverseMap();
            CreateMap<Exams, ExamCreateDto>().ReverseMap();

            CreateMap<Questions, QuestionDto>().ReverseMap();
            CreateMap<Questions, QuestionCreateDto>().ReverseMap();

            CreateMap<Students, StudentDto>().ReverseMap();
            CreateMap<Students, StudentCreateDto>().ReverseMap();

        }
    }
}
