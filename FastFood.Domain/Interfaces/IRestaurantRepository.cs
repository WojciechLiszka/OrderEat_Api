﻿using FastFood.Domain.Entities;

namespace FastFood.Domain.Interfaces
{
    public interface IRestaurantRepository
    {
        Task Commit();
        Task Create(Restaurant restaurant);

        Task Delete(Restaurant restaurant);

        Task<Restaurant?> GetById(int id);

        Task<Restaurant?> GetByName(string name);

        IQueryable<Restaurant> Search(string? phrase);
    }
}