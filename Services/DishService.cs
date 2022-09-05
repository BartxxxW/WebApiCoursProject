using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication44Udemy.Entities;
using WebApplication44Udemy.Exceptions;
using WebApplication44Udemy.Models;

namespace WebApplication44Udemy.Services
{
    public interface IDishService
    {
        public int CreateDish(int restaurantId, CreateDishDto dto);
        public DishDto GetById(int restaurantId, int dishId);
        public List<DishDto> GetAll(int restaurantId);
        public void RemoveAll(int restaurantId);
        public void RemoveByID(int dishId);
    }
    public class DishService : IDishService
    {
        private readonly IMapper _mapper;
        private readonly RestaurantDbContext _dbContext;
        public DishService(RestaurantDbContext dbContext,IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public int CreateDish(int restaurantId, CreateDishDto dto)
        {
            ChecktRestaurantByID(restaurantId);
            var newDish = _mapper.Map<Dish>(dto);
            newDish.RestaurantId = restaurantId;
            _dbContext.Dishes.Add(newDish);
            _dbContext.SaveChanges();
            return newDish.Id;

        }
        public DishDto GetById(int restaurantId, int dishId)
        {
            ChecktRestaurantByID(restaurantId);
            var dish = _dbContext.Dishes.FirstOrDefault(d => d.Id == dishId);
            if (dish is null)
                throw new NotFoundException("dish not found");
            DishDto dishDto = _mapper.Map<DishDto>(dish);
            return dishDto;
        }
        public List<DishDto> GetAll(int restaurantId)
        {
            ChecktRestaurantByID(restaurantId);
            var dish = _dbContext.Dishes.Where(d => d.RestaurantId == restaurantId).ToList();
            if (dish is null)
                throw new NotFoundException("dishes not found");
            List<DishDto> dishList = new List<DishDto>();
            dish.ForEach(d => dishList.Add(_mapper.Map<DishDto>(d)));
            return dishList;
        }
        public void RemoveAll(int restaurantId)
        {
            ChecktRestaurantByID(restaurantId);
            var dishesToDelete = _dbContext.Dishes.Where(d => d.RestaurantId == restaurantId);
            _dbContext.Dishes.RemoveRange(dishesToDelete);
            _dbContext.SaveChanges();

        }
        public void RemoveByID(int dishId)
        {
            var disheToDelete = _dbContext.Dishes.FirstOrDefault(d=>d.Id==dishId);
            if (disheToDelete is null)
                throw new NotFoundException("dish not found");
            _dbContext.Dishes.Remove(disheToDelete);
            _dbContext.SaveChanges();

        }
        private void ChecktRestaurantByID(int restaurantId)
        {
            if (!_dbContext.Restaurants.Any(r => r.id == restaurantId))
                throw new NotFoundException("restraurant not found");
        }


    }


    
}
