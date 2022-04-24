using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataGov_API_Intro_6.Models
{
    public class Parks
    {
        [Key]
        public Guid Id { get; set; }
        public int ParksId { get; set; }
        public string total { get; set; }
        public List<Park> data { get; set; }
        public string limit { get; set; }
        public int count { get; set; }
        public string start { get; set; }
    }


    public class Food
    {
        [Key]
        public Guid Id { get; set; }
        public String? fdcId { get; set; }
        public string? description { get; set; }
        public ICollection<FoodNutrients> foodNutrients { get; set; }
        public string? foodCode { get; set; }
    }

    
    public class main
    {
        [Key]
        public Guid Id { get; set; }
        public ICollection<Food> food { get; set; }

        public DateTime date { get; set; }
    }

    public class FoodNutrients
    {
        [Key]
        public Guid id { get; set; }
        public String? number { get; set; }
        public String? name { get; set; }
        public float? amount { get; set; }
        public string? unitName { get; set; }
       
    }

    public class Park
    {
        [Key]
        public Guid id { get; set; }
        public string states { get; set; }
        public string longitude { get; set; }
        public string directionsInfo { get; set; }
        public string directionsUrl { get; set; }
        public string url { get; set; }
        public string weatherInfo { get; set; }
        public string name { get; set; }
        public string latLong { get; set; }
        public string description { get; set; }
        public string designation { get; set; }
        public string parkCode { get; set; }
        public string fullName { get; set; }
        public string latitude { get; set; }
        public Parks Parks { get; set; }
    }
}
