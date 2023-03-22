﻿using FastFood.Domain.Entities;

namespace FastFood.Domain.Interfaces
{
    public interface IIngredientRepository
    {
        Task Commit();

        Task Create(Ingredient ingredient);

        Task<Ingredient?> GetById(int Id);

        Task Delete(Ingredient ingredient);
    }
}