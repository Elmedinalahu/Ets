using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ets.Models.Entities
{
    public class Questions
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public int Score { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public int Answer { get; set; }
        public string ImageUrl { get; set; }
        
    }
}
