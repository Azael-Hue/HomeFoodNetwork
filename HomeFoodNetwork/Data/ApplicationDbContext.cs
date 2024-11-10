using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HomeFoodNetwork.Models;

namespace HomeFoodNetwork.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Recipe> Recipe { get; set; } = default!;
        public DbSet<RecipeSteps> RecipeSteps { get; set; } = default!;
    }
}
