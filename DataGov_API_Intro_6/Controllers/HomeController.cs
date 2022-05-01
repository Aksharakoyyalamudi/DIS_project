using DataGov_API_Intro_6.Models;
using Microsoft.AspNetCore.Mvc;
using DataGov_API_Intro_6.DataAccess;
using System.Diagnostics;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DataGov_API_Intro_6.ViewModel;

namespace DataGov_API_Intro_6.Controllers
{
    public class HomeController : Controller
    {
        HttpClient httpClient;
        static string BASE_URL = "https://api.nal.usda.gov/";
        
        static string API_KEY = "mdBybOievMdeX3eYSC0MhFu3U7xRV18xHAPG04qb"; //Add your API key here inside ""

        // Obtaining the API key is easy. The same key should be usable across the entire
        // data.gov developer network, i.e. all data sources on data.gov.
        // https://www.nps.gov/subjects/developer/get-started.htm
        [ThreadStatic]
        public static ApplicationDbContext dbContext;
        
        public HomeController(ApplicationDbContext context)
        {
            dbContext = context;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
            public async Task<IActionResult> Food()
        {
            
            List<Food> m = null;

            if (dbContext.Tmain.Any())
            {
                m = dbContext.Tfood.Include(p=>p.foodNutrients).ToList();
            }
            else
            {
                httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Add("X-Api-Key", API_KEY);
                httpClient.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                
                string NATIONAL_PARK_API_PATH = BASE_URL + "fdc/v1/foods/list?";
                string parksData = "";
                Food food = null;

                
                httpClient.BaseAddress = new Uri(NATIONAL_PARK_API_PATH);


                try
                {
                    HttpResponseMessage response = httpClient.GetAsync(NATIONAL_PARK_API_PATH)
                                                            .GetAwaiter().GetResult();

                    if (response.IsSuccessStatusCode)
                    {
                        parksData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    }

                    if (!parksData.Equals(""))
                    {
                        // JsonConvert is part of the NewtonSoft.Json Nuget package
                        m = JsonConvert.DeserializeObject<Food[]>(parksData).Where(i => !string.IsNullOrEmpty(i.fdcId) && !string.IsNullOrEmpty(i.description) && !string.IsNullOrEmpty(i.foodCode))
                        .ToList();
                    
                    }
                    dbContext.Tmain.Add(new main() { food = m, date = DateTime.Now });
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    // This is a useful place to insert a breakpoint and observe the error message
                    Console.WriteLine(e.Message);
                }
            }
            return View(m);
        }

        public IActionResult Foods()
        {
            var foods = dbContext.Tfood.Include(p => p.foodNutrients).ToList();
            ViewData["Foods"] = foods;
            return View();
        }

        /*public IActionResult Nutrients(String fdcId)
        {
            Console.WriteLine(fdcId);
            var fdnutreints = dbContext.Tfood.Include(p => p.foodNutrients).Where(k=>k.fdcId == fdcId).ToList();
            //ViewData["FoodNutrients"] = fdnutreints;
            return View(fdnutreints);
        }*/

        public IActionResult WholeNutirnts(String fdcId="1104086")
        {
            Console.WriteLine(fdcId);
            var fdnutreints = dbContext.Tfood.Include(p => p.foodNutrients).Where(k => k.fdcId == fdcId).ToList();
            //ViewData["FoodNutrients"] = fdnutreints;
            return View(fdnutreints);
        }

        public IActionResult Createnewpage(string fdcID)
        {
            /*FoodNutrients f = new FoodNutrients { };
            ICollection<FoodNutrients> f2 = new System.Collections.ObjectModel.Collection<FoodNutrients>();

            return View(new Food { foodNutrients = f2 });*/
            //Console.WriteLine(fdcId);
            //return View();

            /*var nuts = dbContext.Tfnd.ToList();
            var v = new FoodViewModel()
            {
                food = new Food(),
                foodNutrients = nuts
            };
            return Createnewpage(v);*/
           if (string.IsNullOrEmpty(fdcID))
              return View();
            else
            {
                var foodDetails = dbContext.Tfood.Include(n => n.foodNutrients).Where(s => s.fdcId == fdcID).FirstOrDefault();
                //var foodnutr = dbContext.Tfnd.ToList();
                if (foodDetails == null)
                    return NotFound();
                else
                {
                    var foodnut = foodDetails.foodNutrients.FirstOrDefault();
                    var viewmodel = new FoodViewModel()
                    {
                        fdcId = foodDetails.fdcId,
                        description = foodDetails.description,
                        foodCode = foodDetails.foodCode,
                        number = foodnut.number,
                        name = foodnut.name,
                        amount = foodnut.amount.GetValueOrDefault(),
                        unitName = foodnut.unitName,
                    };
                    return View(viewmodel);
                }
            }


        }

        [HttpGet]
        /* public IActionResult Edit(string fdcID)
         {
             var foodDetails = dbContext.Tfood.Include(n => n.foodNutrients).Where(s => s.fdcId == fdcID).FirstOrDefault();
             //var foodnutr = dbContext.Tfnd.ToList();
             if (foodDetails == null)
                 return NotFound();
             else
             {
                 var foodnut = foodDetails.foodNutrients.FirstOrDefault();
                 var viewmodel = new FoodViewModel()
                 {
                     fdcId = foodDetails.fdcId,
                     description = foodDetails.description,
                     foodCode = foodDetails.foodCode,
                     number = foodnut.number,
                     name = foodnut.name,
                     number = foodDetails.foodNutrients.FirstOrDefault().number,
                     number = foodDetails.foodNutrients.FirstOrDefault().number,




                 };
                 return View(viewmodel);
             }


         }*/

        public void Refresh()
        {
            (dbContext as DbContext).Database.CloseConnection();
            (dbContext as DbContext).Database.OpenConnection();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FoodViewModel view)
        {
            /*DataGov_API_Intro_6.Models.Food f = new DataGov_API_Intro_6.Models.Food();*/
            List<DataGov_API_Intro_6.Models.Food> selectedCollection2 = dbContext.Tfood.Include(f => f.foodNutrients).ToList();
            //List<DataGov_API_Intro_6.Models.Food> selectedCollection2 ;
            var foodDetails = dbContext.Tfood.Include(n => n.foodNutrients).AsNoTracking().Where(s => s.fdcId == view.fdcId).FirstOrDefault();
            try
            {
                Console.WriteLine(view);
                var fn = new DataGov_API_Intro_6.Models.FoodNutrients
                {
                    number = view.number,
                    name = view.name,
                    amount = view.amount,
                    unitName = view.unitName

                };
                List<DataGov_API_Intro_6.Models.FoodNutrients> selectedCollection = new List<DataGov_API_Intro_6.Models.FoodNutrients> { fn };
                var f = new Food
                {
                    fdcId = view.fdcId,
                    description = view.description,
                    foodCode = view.foodCode,
                    foodNutrients = selectedCollection
                };
                Console.WriteLine(f);
                
                if (foodDetails != null)
                {
                    var Commodity = dbContext.Tfood.Where(m => m.fdcId == f.fdcId).Include(c => c.foodNutrients).SingleOrDefault();
                    dbContext.Tfood.Remove(Commodity);
                    selectedCollection2.Remove(Commodity);
                    dbContext.SaveChanges();
                }
                //List<Food> selectedCollection2 =new List<Food>();
                //else
                //{
                //foreach (var nutrients in f.foodNutrients)
                //{
                //  dbContext.Entry(foodDetails.foodNutrients.FirstOrDefault()).CurrentValues.SetValues(nutrients);
                // }
                //dbContext.Entry(Food).Reload();
                //Refresh();
                //   dbContext.Tfood.Update(f);
                //}
                selectedCollection2.Add(f);
                dbContext.Tfood.Add(f);
                dbContext.SaveChanges();

                    /*return View(); */
                    // return View("Index", f);             
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Error Occured");
            }

            return View("Food", selectedCollection2);
            //return View("Index",);
        }

        public IActionResult Delete(Food f)
        {

            var Commodity = dbContext.Tfood.Where(m => m.fdcId == f.fdcId).Include(c => c.foodNutrients).SingleOrDefault();
            /*var Commodity = dbContext.Tfood.Include(c => c.foodNutrients).SingleOrDefault(c => c.fdcId == f.fdcId);*/
            
            //_context.Commodities.Include(c => c.Group).SingleOrDefault(c => c.CommodityID == id);
            dbContext.Tfood.Remove(Commodity);
            List<DataGov_API_Intro_6.Models.Food> selectedCollection2 = dbContext.Tfood.Include(f => f.foodNutrients).ToList();
            dbContext.SaveChanges();
            return View("Food", selectedCollection2);
        }

        public ActionResult Search(String id = null)

        {

            var registration = dbContext.Tfood.Include(n=>n.foodNutrients).
                Where(p => p.fdcId.Contains(id));

            return View("Food",Index);

        }

    }
}




