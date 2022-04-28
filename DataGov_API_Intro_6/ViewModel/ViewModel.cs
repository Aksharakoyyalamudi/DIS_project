using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataGov_API_Intro_6.Models;

namespace DataGov_API_Intro_6.ViewModel
{
    public class FoodViewModel
    {
        public Food food { get; set; }
        public IEnumerable<FoodNutrients> foodNutrients { get; set; }
/*        public IEnumerable<Group> GroupList { get; set; }
        public IEnumerable<Sector> SectorList { get; set; }*/
    }

    
}
