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

        //static string BASE_URL = "https://developer.nps.gov/api/v1";
        static string BASE_URL = "https://api.nal.usda.gov/";
        
        static string API_KEY = "mdBybOievMdeX3eYSC0MhFu3U7xRV18xHAPG04qb"; //Add your API key here inside ""

        // Obtaining the API key is easy. The same key should be usable across the entire
        // data.gov developer network, i.e. all data sources on data.gov.
        // https://www.nps.gov/subjects/developer/get-started.htm

        public ApplicationDbContext dbContext;

        public HomeController(ApplicationDbContext context)
        {
            dbContext = context;
        }

        public async Task<IActionResult> Index()
        {
            ICollection<Food> m = null;

            if (dbContext.Tmain.Any())
            {
                m = dbContext.Tfood.Include(p=>p.foodNutrients).ToArray();
            }
            else {
                httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Add("X-Api-Key", API_KEY);
                httpClient.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //string NATIONAL_PARK_API_PATH = BASE_URL + "/parks?limit=20";
                string NATIONAL_PARK_API_PATH = BASE_URL + "fdc/v1/foods/list?";
                string parksData = "";

                Parks parks = null;

                Food food = null;

                //httpClient.BaseAddress = new Uri(NATIONAL_PARK_API_PATH);
                httpClient.BaseAddress = new Uri(NATIONAL_PARK_API_PATH);


                try
                {
                    //HttpResponseMessage response = httpClient.GetAsync(NATIONAL_PARK_API_PATH)
                    //                                        .GetAwaiter().GetResult();
                    HttpResponseMessage response = httpClient.GetAsync(NATIONAL_PARK_API_PATH)
                                                            .GetAwaiter().GetResult();



                    if (response.IsSuccessStatusCode)
                    {
                        parksData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    }

                    if (!parksData.Equals(""))
                    {
                        // JsonConvert is part of the NewtonSoft.Json Nuget package

                        m = JsonConvert.DeserializeObject<Food[]>(parksData);
                        //parks = JsonConvert.DeserializeObject<Parks>(parksData);
                    }


                    //dbContext.parks.Add(parks);
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

        public IActionResult Createnewpage()
        {
            /*FoodNutrients f = new FoodNutrients { };*/
            ICollection<FoodNutrients> f2 = new System.Collections.ObjectModel.Collection<FoodNutrients>();

            return View(new Food { foodNutrients = f2 });

            /*var nuts = dbContext.Tfnd.ToList();
            var v = new FoodViewModel()
            {
                food = new Food(),
                foodNutrients = nuts
            };
            return Createnewpage(v);*/
        }


            [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Food food)
        {
            try
            {
                /*if (ModelState.IsValid)
                {

                    dbContext.Tfood.Add(model);
                    dbContext.SaveChanges();
                    return RedirectToAction("Foods", "Home");
                }
                
                else
                {

                    var viewModel = new Food
                    {
                        Commodity = model.Commodity,
                        GroupList = _context.Groups.ToList(),
                        SectorList = _context.Sectors.ToList()
                    };

                    return View("Create", viewModel);


                }*/
                dbContext.Tfood.Add(food);
                /*dbContext.Tfnd.Add(view.foodNutrients);*/
                dbContext.SaveChanges();

                return View(food); 

            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Error Occured");
            }

            return View();
        }

        public IActionResult Delete(string fdcID="1104086")
        {

            var Commodity = dbContext.Tfood.Include(p => p.foodNutrients).Where(m => m.fdcId == fdcID).SingleOrDefault();
                //_context.Commodities.Include(c => c.Group).SingleOrDefault(c => c.CommodityID == id);
            dbContext.Tfood.Remove(Commodity);
            dbContext.SaveChanges();
            return View();
        }

    }
}




