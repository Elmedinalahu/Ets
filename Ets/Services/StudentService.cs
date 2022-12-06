using AutoMapper;
using Ets.Data.UnitOfWork;
using Ets.Models.Dto;
using Ets.Models.Dto.Student;
using Ets.Models.Entities;
using Ets.Services.IService;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
//using Life.CSdotnet.LifeLibrary;

namespace Ets.Services
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;

        public StudentService(IUnitOfWork unitOfWork, IMapper mapper, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailSender = emailSender;
        }

        public async Task CreateStudent(StudentCreateDto studentToCreate)
        {
            var student = _mapper.Map<Students>(studentToCreate);

            _unitOfWork.Repository<Students>().Create(student);

            _unitOfWork.Save();
        }

        public async Task DeleteStudent(int id)
        {
            var student = await GetStudent(id);
            
            _unitOfWork.Repository<Students>().Delete(student);

            _unitOfWork.Save();
        }


        public async Task<Students> GetStudent(int id)
        {
            Expression<Func<Students, bool>> expression = x => x.Id == id;
            var student = await _unitOfWork.Repository<Students>().GetById(expression).FirstOrDefaultAsync();

            return student;
        }
        

        public IEnumerable<Result> GetExamResult(int studentId)
        {
            var examResults = _unitOfWork.Repository<ExamResults>().GetAll().Where(a => a.StudentsId == studentId);
            var students = _unitOfWork.Repository<Students>().GetAll();
            var exams = _unitOfWork.Repository<Exams>().GetAll();
            var question = _unitOfWork.Repository<Questions>().GetAll();

            var requiredData = examResults.Join(students, er => er.StudentsId, s => s.Id,
                (er, st) => new { er, st }).Join(exams, erj => erj.er.ExamsId, ex => ex.Id,
                (erj, ex) => new { erj, ex }).Join(question, exj => exj.erj.er.QuestionsId, q => q.Id,
                (exj, q) => new Result()
                {
                    StudentId = studentId,
                    ExamName = exj.ex.Name,
                    TotalQuestions = examResults.Count(a => a.StudentsId == studentId && a.ExamsId == exj.ex.Id),
                    CorrectAnswers = examResults.Count(a => a.StudentsId == studentId && a.ExamsId == exj.ex.Id && a.Answer == q.Answer),
                    WrongAnswers = examResults.Count(a => a.StudentsId == studentId && a.ExamsId == exj.ex.Id && a.Answer != q.Answer)
                });

            return Enumerable.Empty<Result>();
        }

        public bool SetExamResult(AttendExamDto attendExamDto)
        {
            foreach (var item in attendExamDto.Questions)
            {
                ExamResults examResults = new ExamResults();
                examResults.StudentsId = attendExamDto.StudentId;
                examResults.QuestionsId = item.Id;
                examResults.ExamsId = item.ExamsId;
                examResults.Answer = item.SelectedAnswer;

                _unitOfWork.Repository<ExamResults>().Create(examResults);
            }
            _unitOfWork.Save();
            return true;
        }
        
        public double GetResult(int studentId, int examId)
        {
            var examResults = _unitOfWork.Repository<ExamResults>().GetAll().Where(a => a.StudentsId == studentId && a.ExamsId == examId);
            var questions = _unitOfWork.Repository<Questions>().GetAll();

            var correctAnswers = examResults.Join(questions, er => er.QuestionsId, q => q.Id,
                (er, q) => new { er, q }).Where(a => a.er.Answer == a.q.Answer).Count();

            var wrongAnswers = examResults.Join(questions, er => er.QuestionsId, q => q.Id,
                (er, q) => new { er, q }).Where(a => a.er.Answer != a.q.Answer).Count();

            var result = (correctAnswers * 100) / (correctAnswers + wrongAnswers);


            if (result >= 50 && result <= 59)
            {
                return 6;
            }
            else if (result >= 60 && result <= 69)
            {
                return 7;
            }
            else if (result >= 70 && result <= 79)
            {
                return 8;
            }
            else if (result >= 80 && result <= 89)
            {
                return 9;
            }
            else if (result >= 90 && result <= 100)
            {
                return 10;
            }
            else
            {
                return 5;
            }
        }

        //sent the result to the student with email
        public async Task SendResult(int studentId, int examId)
        {
            var student = _unitOfWork.Repository<Students>().GetAll().Where(a => a.Id == studentId).FirstOrDefault();
            var result = GetResult(studentId, examId);
            //await _emailSender.SendEmailAsync(student.Email, "Exam Result", $"Your result is {result}");

            var pathToFile = "Templates/exam_results.html";

            string htmlBody = "";
            using (StreamReader streamReader = System.IO.File.OpenText(pathToFile))
            {
                htmlBody = streamReader.ReadToEnd();
            }
             
            var resultt = new Result
            {
                StudentId = studentId,
                ExamName = _unitOfWork.Repository<Exams>().GetAll().Where(a => a.Id == examId).FirstOrDefault().Name,
                TotalQuestions = _unitOfWork.Repository<ExamResults>().GetAll().Where(a => a.StudentsId == studentId && a.ExamsId == examId).Count(),
                CorrectAnswers = _unitOfWork.Repository<ExamResults>().GetAll().Where(a => a.StudentsId == studentId && a.ExamsId == examId && a.Answer == _unitOfWork.Repository<Questions>().GetAll().Where(b => b.Id == a.QuestionsId).FirstOrDefault().Answer).Count(),
                WrongAnswers = _unitOfWork.Repository<ExamResults>().GetAll().Where(a => a.StudentsId == studentId && a.ExamsId == examId && a.Answer != _unitOfWork.Repository<Questions>().GetAll().Where(b => b.Id == a.QuestionsId).FirstOrDefault().Answer).Count()
            };


            var data = new
            {
                studentId = resultt.StudentId,
                sxamName = resultt.ExamName,
                Result = result
            };

            //var data = new { StudentId = studentId,
            //ExamName = _unitOfWork.Repository<Exams>().GetAll().Where(a => a.Id == examId).FirstOrDefault().Name,
            //Result = result };

            var content = string.Format(htmlBody, data);

            await _emailSender.SendEmailAsync(student.Email, "Exam Result", content);
        }
       
    }

}