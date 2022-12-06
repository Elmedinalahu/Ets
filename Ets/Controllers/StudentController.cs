using Ets.Models.Dto;
using Ets.Models.Dto.Student;
using Ets.Services;
using Ets.Services.IService;
using Ets.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Ets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _userService;
        private readonly IConfiguration _configuration;


        public StudentController(IStudentService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetExamResult")]
        public IActionResult GetExamResult(int studentId)
        {
            var result = _userService.GetExamResult(studentId);
            return Ok(result);
        }
        

        [HttpPost]
        [Route("SetExamResult")]
        public IActionResult SetExamResult(AttendExamDto attendExamDto)
        {
            var result = _userService.SetExamResult(attendExamDto);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetResult")]
        public IActionResult GetResult(int studentId, int examId)
        {
            var result = _userService.GetResult(studentId, examId);
            return Ok(result);
        }

        [HttpPost]
        [Route("SendResult")]
        public IActionResult SendResult(int studentId, int examId)
        {
            _userService.SendResult(studentId, examId);
            return Ok("ResultEmail sent successfully!");
        }

        [HttpGet("GetStudent")]
        public async Task<IActionResult> Get(int id)
        {
            var student = _userService.GetStudent(id);

            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpPost("PostStudent")]
        public async Task<IActionResult> Post([FromForm] StudentCreateDto studentToCreate)
        {
            await _userService.CreateStudent(studentToCreate);


            return Ok("Student created successfully!");

        }
        
        [HttpDelete("DeleteStudent")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteStudent(id);

            return Ok("Student deleted successfully!");
        }




    }
}
