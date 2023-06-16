using OrderEat.Domain.Entities;
using OrderEat.Domain.Interfaces;
using OrderEat.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace OrderEat.Infrastructure.Repositories
{
    public class SpecialDietRepository : ISpecialDietRepository
    {
        private readonly OrderEatDbContext _dbContext;

        public SpecialDietRepository(OrderEatDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Commit()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task Create(SpecialDiet specialDiet)
        {
            _dbContext.Add(specialDiet);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(SpecialDiet specialDiet)
        {
            _dbContext.Remove(specialDiet);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<SpecialDiet?> GetById(int id)
        {
            var diet = await _dbContext.Diets.FirstOrDefaultAsync(d => d.Id == id);
            return diet;
        }

        public async Task<SpecialDiet?> GetByName(string name)
        {
            if (name == null)
            {
                return null;
            }
            var result = await _dbContext.Diets.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());

            return result;
        }

        public IQueryable<SpecialDiet> Search(string? phrase)
        {
            var diets = _dbContext
                .Diets
                .Where(b => phrase == null
               || b.Name.ToLower().Contains(phrase.ToLower())
               || b.Description.ToLower().Contains(phrase.ToLower()));
            return diets;
        }
    }
}