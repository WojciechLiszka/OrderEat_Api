using FastFood.Domain.Entities;
using FastFood.Domain.Interfaces;
using FastFood.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Infrastructure.Repositories
{
    public class FastFoodRepository : IFastFoodRepository
    {
        private readonly FastFoodDbContext _dbContext;

            
        

        public async Task Register(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return;
        }

        public FastFoodRepository(FastFoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Commit()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> Create(Allergen allergen)
        {
            _dbContext.Allergens.Add(allergen);
            await _dbContext.SaveChangesAsync();
            return allergen.Id;
        }

        public async Task<Allergen?> GetById(int id)
        {
            var result = await _dbContext.Allergens.FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task<Allergen?> GetByName(string name)
        {
            var result = await _dbContext.Allergens.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());

            return result;
        }

        public async Task Delete(Allergen allergen)
        {
            _dbContext.Allergens.Remove(allergen);

            await _dbContext.SaveChangesAsync();
        }

        public IQueryable<Allergen> Search(string? phrase)
        {
            var allergens = _dbContext
                .Allergens
                .Where(b => phrase == null
                || b.Name.ToLower().Contains(phrase.ToLower())
                || b.Description.ToLower().Contains(phrase.ToLower()));
            return allergens;
        }

        public bool EmailInUse(string email)
            => _dbContext.Users.Any(u => u.Email == email);

    }
}