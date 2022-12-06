using Amazon.S3;
using Amazon.S3.Model;
using Ets.Data.UnitOfWork;
using Ets.Models.Dto;
using Ets.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Ets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : Controller
    {
        private readonly IQuestionService _questionService;
        private readonly IConfiguration _configuration;

        public QuestionController(IQuestionService questionService, IConfiguration configuration)
        {
            _questionService = questionService;
            _configuration = configuration;
        }

        [HttpGet("GetQuestion")]
        public async Task<IActionResult> Get(int id)
        {
            var question = await _questionService.GetQuestion(id);

            if (question == null)
            {
                return NotFound();
            }
            return Ok(question);
        }

        [HttpGet("GetAllQuestions")]
        public async Task<IActionResult> Get()
        {
            var questions = _questionService.GetAllQuestions();

            return Ok(questions);
        }

        //[HttpGet("QuestionsListView")]
        //public async Task<IActionResult> QuestionsListView(int examsId = 0, int page = 1, int pageSize = 10)
        //{

        //    var questions = await _questionService.QuestionsListView(page, pageSize, examsId );

        //    return Ok(questions);
        //}


        [HttpPost("PostQuestion")]
        //[Authorize(Roles = "LifeAdmin")]
        public async Task<IActionResult> Post([FromForm] QuestionCreateDto questionToCreate)
        {
            await _questionService.CreateQuestion(questionToCreate);


            return Ok("Question created successfully!");

        }

        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            var uploadPicture = await UploadToBlob(file);

            var imageUrl = $"{_configuration.GetValue<string>("BlobConfig:CDNLife")}{file.FileName + Path.GetExtension(file.FileName)}";

            return Ok(imageUrl);
        }

        [NonAction]
        public async Task<PutObjectResponse> UploadToBlob(IFormFile file)
        {
            string serviceURL = _configuration.GetValue<string>("BlobConfig:serviceURL");
            string AWS_accessKey = _configuration.GetValue<string>("BlobConfig:accessKey");
            string AWS_secretKey = _configuration.GetValue<string>("BlobConfig:secretKey");
            var bucketName = _configuration.GetValue<string>("BlobConfig:bucketName");
            var keyName = _configuration.GetValue<string>("BlobConfig:defaultFolder");

            var config = new AmazonS3Config() { ServiceURL = serviceURL };
            var s3Client = new AmazonS3Client(AWS_accessKey, AWS_secretKey, config);
            keyName = String.Concat(keyName, file.FileName);

            var fs = file.OpenReadStream();
            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = keyName,
                InputStream = fs,
                ContentType = file.ContentType,
                CannedACL = S3CannedACL.PublicRead
            };

            return await s3Client.PutObjectAsync(request);
        }


        [HttpPut("UpdateQuestion")]
        public async Task<IActionResult> Update(QuestionDto questionToUpdate)
        {
            await _questionService.UpdateQuestion(questionToUpdate);

            return Ok("Question updated successfully!");
        }

        [HttpDelete("DeleteQuestion")]
        public async Task<IActionResult> Delete(int id)
        {
            await _questionService.DeleteQuestion(id);

            return Ok("Question deleted successfully!");
        }


        [HttpGet("IsExamAttended")]
        public async Task<IActionResult> IsExamAttended(int examId, int studentId)
        {
            var isExamAttended = _questionService.IsExamAttended(examId, studentId);

            return Ok(isExamAttended);
        }

        [HttpPost("UploadJsonFile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadFile(IFormFile file, CancellationToken cancellationToken)
        {
            await WriteFile(file);

            return Ok("File uploaded successfully!");
        }

        private async Task<bool> WriteFile(IFormFile file)
        {
            bool isSaveSuccess = false;
            string fileName;
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                fileName = DateTime.Now.Hour + extension;

                var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files");
                if (!Directory.Exists(pathBuilt))
                    Directory.CreateDirectory(pathBuilt);

                var path = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                isSaveSuccess = true;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return isSaveSuccess;
        }
        

    }
}
