﻿using FastFood.Domain.Entities;
using FastFood.Domain.Interfaces;
using FastFood.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Infrastructure.Repositories
{
    public class SpecialDietRepository : ISpecialDietRepository
    {
        private readonly FastFoodDbContext _dbContext;

        public SpecialDietRepository(FastFoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Commit()
        {
            _dbContext.SaveChangesAsync();
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

        public async Task<SpecialDiet> GetById(int id)
        {
            var diet = await _dbContext.Diets.FirstOrDefaultAsync(d => d.Id == id);
            return diet;
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