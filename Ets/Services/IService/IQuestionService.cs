using Ets.Helpers;
using Ets.Models.Dto;
using Ets.Models.Entities;

namespace Ets.Services.IServices
{
    public interface IQuestionService
    {
        Task<List<Questions>> GetAllQuestions();
        //Task<PagedResult<Questions>> QuestionsListView(int page, int pageSize, int examsId);
        Task<Questions> GetQuestion(int id);
        Task CreateQuestion(QuestionCreateDto questionToCreate);
        Task UpdateQuestion(QuestionDto questionToUpdate);
        Task DeleteQuestion(int id);
        Task<Questions> GetWithIncludes(int id);
        bool IsExamAttended(int examId, int studentId);

    }
}
