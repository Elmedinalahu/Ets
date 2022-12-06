namespace Ets.Models.Dto
{
    public class Result
    {
        public int StudentId { get; set; }
        public string ExamName { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public int WrongAnswers { get; set; }
    }
}
