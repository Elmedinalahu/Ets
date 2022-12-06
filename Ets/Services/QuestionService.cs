using AutoMapper;
using Ets.Data.UnitOfWork;
using Ets.Helpers;
using Ets.Models.Dto;
using Ets.Models.Entities;
using Ets.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ets.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public QuestionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateQuestion(QuestionCreateDto questionToCreate)
        {
            var question = _mapper.Map<Questions>(questionToCreate);

            _unitOfWork.Repository<Questions>().Create(question);

            _unitOfWork.Save();
        }

        public async Task DeleteQuestion(int id)
        {
            var question = await GetQuestion(id);

            _unitOfWork.Repository<Questions>().Delete(question);

            _unitOfWork.Save();
        }

        public async Task<List<Questions>> GetAllQuestions()
        {
            var questions = _unitOfWork.Repository<Questions>().GetAll();

            return questions.ToList();
        }

        public async Task<Questions> GetQuestion(int id)
        {
            Expression<Func<Questions, bool>> expression = x => x.Id == id;
            var question = await _unitOfWork.Repository<Questions>().GetById(expression).FirstOrDefaultAsync();

            return question;
        }

        public async Task<Questions> GetWithIncludes(int id)
        {
            Expression<Func<Questions, bool>> expression = x => x.Id == id;
            var question = await _unitOfWork.Repository<Questions>().GetByConditionWithIncludes(expression, "Category, Unit").FirstOrDefaultAsync();

            return question;
        }

        public bool IsExamAttended(int examId, int studentId)
        {
            Expression<Func<ExamResults, bool>> expression = x => x.ExamsId == examId && x.StudentsId == studentId;
            var examResult = _unitOfWork.Repository<ExamResults>().GetByCondition(expression).FirstOrDefault();

            if (examResult != null)
            {
                return true;
            }
            else
            {
                return false;
            }

            
        }

        //public async Task<PagedResult<Questions>> QuestionsListView(int page, int pageSize, int examsId)
        //{

        //    IQueryable<Questions> exams;


        //    if (examsId is not 0)
        //    {
        //        Expression<Func<Questions, bool>> conditionByExam = x => x.ExamsId == examsId;
        //        exams = _unitOfWork.Repository<Questions>()
        //                                     .GetByCondition(conditionByExam);
        //    }
        //    else // dismiss category
        //    {
        //        exams = _unitOfWork.Repository<Questions>().GetAll();
        //    }

        //    var count = await exams.CountAsync();

        //    var examsPaged = new PagedResult<Questions>()
        //    {
        //        TotalItems = count,
        //        PageNumber = page,
        //        PageSize = pageSize,
        //        Data = await exams
        //                    .Skip((page - 1) * pageSize)
        //                    .Take(pageSize).ToListAsync()
        //    };

        //    return examsPaged;
        //}

        public async Task UpdateQuestion(QuestionDto questionToUpdate)
        {
            Questions? question = await GetQuestion(questionToUpdate.Id);

            question.Question = questionToUpdate.Question;

            _unitOfWork.Repository<Questions>().Update(question);

            _unitOfWork.Save();

        }






    }
}
