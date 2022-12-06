using Ets.Models.Dto;
using Ets.Models.Dto.Student;
using Ets.Models.Entities;

namespace Ets.Services.IService
{
    public interface IStudentService
    {

        IEnumerable<Result> GetExamResult(int studentId);
        bool SetExamResult(AttendExamDto attendExamDto);
        double GetResult(int examId, int studentId);
        Task SendResult(int studentId, int examId);

        Task<Students> GetStudent(int id);
        Task CreateStudent(StudentCreateDto studentToCreate);
        Task DeleteStudent(int id);





    }
}
    