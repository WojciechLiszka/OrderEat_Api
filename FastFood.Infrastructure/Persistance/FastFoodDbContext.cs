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
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Order> Orders { get; set; }

        public FastFoodDbContext(DbContextOptions<FastFoodDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Restaurant>(eb =>
            {
                eb
                .Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(30);

                eb.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(500);

                eb
                .OwnsOne(e => e.ContactDetails);

                eb
                .HasMany(e => e.Dishes);
            });

            modelBuilder.Entity<Dish>(eb =>
            {
                eb
                .Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(40);

                eb
                .Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(500);

                eb
                .HasMany(e => e.AllowedForDiets)
                .WithMany(e => e.Dishes);

                eb
                .HasMany(e => e.BaseIngreedients)
                .WithOne(e => e.Dish);
            });
            modelBuilder.Entity<Ingredient>(eb =>
              {
                  eb.
                  Property(e => e.Name).
                  IsRequired()
                  .HasMaxLength(50);

                  eb.Property
                  (e => e.Description)
                  .IsRequired()
                  .HasMaxLength(50);

                  eb
                 .HasMany(c => c.Allergens)
                 .WithMany(c => c.Ingredients);
              });
            modelBuilder.Entity<User>(eb =>
            {
                eb
                .HasOne(c => c.Role)
                .WithMany(c => c.Users);

                eb
                .Property(e => e.Email)
                .IsRequired();

                eb.
                OwnsOne(e => e.ContactDetails);

                eb
                .HasOne(e => e.Diet);
            });
            modelBuilder.Entity<Allergen>(eb =>
            {
                eb
                .Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(45);

                eb
                .Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(500);
            });
            modelBuilder.Entity<Role>(eb =>
            {
                eb
                .Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(45);
            });
            modelBuilder.Entity<SpecialDiet>(eb =>
            {
                eb
                .Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(45);

                eb
                .Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(500);
            });

            modelBuilder.Entity<Order>(eb =>
            {
                eb.OwnsOne(e => e.OrderedDishes);
            });
        }
    }
}