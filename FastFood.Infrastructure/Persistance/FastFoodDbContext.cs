using FastFood.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Infrastructure.Persistance
{
    public class FastFoodDbContext : DbContext
    {
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<SpecialDiet> Diets { get; set; }
        public DbSet<Allergen> Allergens { get; set; }

        public FastFoodDbContext(DbContextOptions<FastFoodDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Restaurant>(eb =>
            {
                eb
                .OwnsOne(e => e.ContactDetails);
                eb
                .HasMany(e => e.Dishes)
                .WithOne(e => e.Restaurant);
            });

            modelBuilder.Entity<Dish>(eb =>
            {
                eb
                .HasMany(e => e.AllowedForDiets);
                eb
                .HasMany(e => e.BaseIngreedients)
                .WithOne(e=>e.Dish);
            });
            modelBuilder.Entity<Ingredient>()
                .HasMany(c => c.Allergens)
                .WithMany(c=>c.Ingredients);
        }
    }
}