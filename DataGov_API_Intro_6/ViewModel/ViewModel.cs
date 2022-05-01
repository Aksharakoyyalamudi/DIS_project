using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataGov_API_Intro_6.Models;

namespace DataGov_API_Intro_6.ViewModel
{
    public class FoodViewModel
    {
        /*public Food food { get; set; }
        public IEnumerable<FoodNutrients> foodNutrients { get; set; }*/
        public string fdcId { get; set; }
        public string description { get; set; }
        public string foodCode { get; set; }
        public string number { get; set; }
        public string name { get; set; }
        public float amount { get; set; }
        public string unitName { get; set; }
        /*        public IEnumerable<Group> GroupList { get; set; }
                public IEnumerable<Sector> SectorList { get; set; }*/
    }

    public class sample
    {
        public int desc { get; set; }
        public int quantity { get; set; }
    }
    
}
