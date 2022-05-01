using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataGov_API_Intro_6.Models
{
    public class Food
    {
        [Key]
        //public Guid Id { get; set; }
        [Required]
        public String? fdcId { get; set; }
        [Required]
        public string? description { get; set; }
        public List<FoodNutrients> foodNutrients { get; set; }
        [Required]
        public string? foodCode { get; set; }

        //public List<FoodNutrients> foodNutrientsList { get; set; }
    }
  
    public class main
    {
        [Key]
        public Guid Id { get; set; }
        public List<Food> food { get; set; }
        public DateTime date { get; set; }
    }

    public class FoodNutrients
    {
        [Key]
        public Guid id { get; set; }
        [Required]
        public String? number { get; set; }
        [Required]
        public String? name { get; set; }
        [Required]
        public float? amount { get; set; }
        [Required]
        public string? unitName { get; set; }
         /*public Food food { get; set; }*/
    }
    
}
