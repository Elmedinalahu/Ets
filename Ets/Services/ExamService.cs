using Ets.Data.UnitOfWork;
using Ets.Services.IService;
using AutoMapper;
using Ets.Models.Entities;
using System.Linq.Expressions;
using Ets.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Ets.Helpers;

namespace Ets.Services
{
    public class ExamService : IExamService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ExamService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<Exams>> GetAllExams()
        {
            var exams = _unitOfWork.Repository<Exams>().GetAll().Include(x => x.Questions).ToList();

            return exams.ToList();
        }



        public async Task<Exams> GetExam(int id)
        {
            Expression<Func<Exams, bool>> expression = x => x.Id == id;
            var exam = await _unitOfWork.Repository<Exams>().GetById(expression).Include(x => x.Questions).FirstOrDefaultAsync();

            return exam;

        }

        public async Task CreateExamWithRandomQuestions(ExamCreateDto examToCreate, int numberOfQuestions)
        {
            var exam = _mapper.Map<Exams>(examToCreate);
            var questions = _unitOfWork.Repository<Questions>().GetAll().ToList();
            var randomQuestions = questions.OrderBy(x => Guid.NewGuid()).Take(numberOfQuestions).ToList();
            exam.Questions = randomQuestions;
            _unitOfWork.Repository<Exams>().Create(exam);
            _unitOfWork.Save();
        }

        public async Task UpdateExam(ExamDto examToUpdate)
        {
            Exams? exam = await GetExam(examToUpdate.Id);

            exam.Name = examToUpdate.Name;

            _unitOfWork.Repository<Exams>().Update(exam);

            _unitOfWork.Save();
        }

        public async Task DeleteExam(int id)
        {
            Exams? exam = await GetExam(id);

            _unitOfWork.Repository<Exams>().Delete(exam);

            _unitOfWork.Save();
        }

        public async Task<PagedResult<Exams>> ExamsListView(int page, int pageSize)
        {

            var exams = _unitOfWork.Repository<Exams>().GetAll();

            var count = await exams.CountAsync();

            var examsPaged = new PagedResult<Exams>()
            {
                TotalItems = count,
                PageNumber = page,
                PageSize = pageSize,
                Data = await exams
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize).ToListAsync()
            };

            return examsPaged;

        }
    }
}
