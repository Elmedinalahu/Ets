
using Ets.Models.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ets.Models.Dto
{
    public class QuestionDto
    {
       
        public int Id { get; set; }
        [Required]
        [Display(Name = "Question")]
        public string Question { get; set; }
        public int Score { get; set; }
        [Required]
        [Display(Name = "Option 1")]
        public string Option1 { get; set; }
        [Required]
        [Display(Name = "Option 2")]
        public string Option2 { get; set; }
        [Required]
        [Display(Name = "Option 3")]
        public string Option3 { get; set; }
        [Required]
        [Display(Name = "Option 4")]
        public string Option4 { get; set; }
        [Required]
        public int Answer { get; set; }
        public string ImageUrl { get; set; }
        public int? ExamsId { get; internal set; }
        public int SelectedAnswer { get; internal set; }
    }
}
