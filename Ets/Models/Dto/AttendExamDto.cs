namespace Ets.Models.Dto
{
    public class AttendExamDto
    {
        public int StudentId { get; set; }
        public string ExamName { get; set; }
        public List<QuestionDto> Questions { get; set; }
        public string Message { get; set; }
    }
}
