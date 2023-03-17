﻿using FastFood.Domain.Entities;

namespace FastFood.Domain.Interfaces
{
    public interface IDishRepository
    {
        Task Commit();

        Task Create(Domain.Entities.Dish dish);

        Task<Dish> GetById(int id);

        IQueryable<Dish> Search(int id,string? phrase);
    }
}