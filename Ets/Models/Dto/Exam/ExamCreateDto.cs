using Ets.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace Ets.Models.Dto
{
    public class ExamCreateDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Author { get; set; }

    }
}
