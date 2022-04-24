using Microsoft.EntityFrameworkCore;
using DataGov_API_Intro_6.Models;
namespace DataGov_API_Intro_6.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
            base(options)
        { }

        public DbSet<Food> Tfood { get; set; }
        public DbSet<main> Tmain { get; set; }

        
        //public DbSet<Parks> parks { get; set; }
        //public DbSet<Nutrients> Tnut { get; set; }

        public DbSet<FoodNutrients> Tfnd { get; set; }
    }
}
