using System.ComponentModel.DataAnnotations;

namespace Ets.Models.Entities
{
    public class Exams
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        [Required]
        public List<Questions> Questions { get; set; }
        public HashSet<ExamResults> ExamResults { get; set; } = new HashSet<ExamResults>();
    }
}
