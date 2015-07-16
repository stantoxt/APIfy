using APIfy.Database.Models;
using System.Data.Entity;

namespace APIfy.Database.Contexts
{
    public class ApifyContext : DbContext
    {
        public ApifyContext() : base()
        {
        }

        public DbSet<Module> Consoles { get; set; }
    }
}
