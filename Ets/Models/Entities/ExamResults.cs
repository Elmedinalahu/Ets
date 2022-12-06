using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ets.Models.Entities
{
    public class ExamResults
    {
        public int Id { get; set; }
        public int? StudentsId { get; set; }
        public Students Students { get; set; }
        public int? ExamsId { get; set; }
        public Exams Exams { get; set; }
        public int? QuestionsId { get; set; }
        public Questions Questions { get; set; }
        public int Answer { get; set; }
        
    }
}
