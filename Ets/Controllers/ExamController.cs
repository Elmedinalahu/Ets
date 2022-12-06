using Ets.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Ets.Models.Dto;

namespace Ets.Controllers
{
    [ApiController]
    public class ExamController : Controller
    {
        private readonly IExamService _examService;
        private readonly ILogger<ExamController> _logger;
        private readonly IStringLocalizer<ExamController> _localizer;
        private readonly IEmailSender _emailSender;

        public ExamController(IExamService examService, ILogger<ExamController> logger, IStringLocalizer<ExamController> localizer, IEmailSender emailSender)
        {
            _examService = examService;
            _logger = logger;
            _localizer = localizer;
            _emailSender = emailSender;
        }

        [HttpGet("GetExam")]
        //[Authorize(Roles = "LifeUser")]
        public async Task<IActionResult> Get(int id)
        {
            var exam = await _examService.GetExam(id);

            if (exam == null)
            {
                return NotFound();
            }
            return Ok(exam);
        }

        [HttpGet("GetAllExams")]
        public async Task<IActionResult> Get()
        {
            var exams = _examService.GetAllExams();

            return Ok(exams);
        }

        [HttpGet("ExamsListView")]
        public async Task<IActionResult> ExamsListView(int page = 1, int pageSize = 10)
        {

            var exams = await _examService.ExamsListView(page, pageSize);

            return Ok(exams);
        }

        [HttpPost("PostExamWithRandomQuestions")]
        //[Authorize(Roles = "LifeAdmin")]
        public async Task<IActionResult> Post(ExamCreateDto examToCreate, int numberOfQuestions)
        {
            await _examService.CreateExamWithRandomQuestions(examToCreate, numberOfQuestions);

            return Ok("Exam created successfully!");
        }

        [HttpPut("UpdateExam")]
        [Authorize(Roles = "LifeAdmin")]
        public async Task<IActionResult> Update(ExamDto examToUpdate)
        {
            await _examService.UpdateExam(examToUpdate);

            return Ok("Exam updated successfully!");
        }

        [HttpDelete("DeleteExam")]
        [Authorize(Roles = "LifeAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _examService.DeleteExam(id);

            return Ok("Exam deleted successfully!");
        }
    }
}
