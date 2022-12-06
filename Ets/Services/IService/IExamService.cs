using Ets.Helpers;
using Ets.Models.Dto;
using Ets.Models.Entities;

namespace Ets.Services.IService
{
    public interface IExamService
    {
        Task<List<Exams>> GetAllExams();
        Task<PagedResult<Exams>> ExamsListView(int page, int pageSize);
        Task<Exams> GetExam(int id);
        Task CreateExamWithRandomQuestions(ExamCreateDto examToCreate, int numberOfQuestions);
        Task UpdateExam(ExamDto examToUpdate);
        Task DeleteExam(int id);
    }
}
