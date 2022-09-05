using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication44Udemy.Authorization;
using WebApplication44Udemy.Entities;
using WebApplication44Udemy.Exceptions;
using WebApplication44Udemy.Models;

namespace WebApplication44Udemy.Services
{
    public interface IRestaurantService
    {
        RestaurantDto GetById(int id);
        int CreateRestaurant(CreateRestaurantDto dto);
        PageResult<RestaurantDto> GetAll( RestaurantQuery query);
        void Delete(int id);
        public void UpdateRestaurant(int id, UpdateRestaurantDto dto);

    }

    public class RestaurantService: IRestaurantService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;
        private readonly IAuthorizationService _authorization;
        private readonly IUserContextService _userContextService;

        public RestaurantService(RestaurantDbContext dbContext, IMapper mapper, ILogger<RestaurantService> logger, IAuthorizationService authorization,
            IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _authorization = authorization;
            _userContextService = userContextService;
        }

        public RestaurantDto GetById(int id)
        {

            var result = _dbContext.Restaurants
                .Include(a => a.Adress)
                .Include(a => a.Dishes)
                .FirstOrDefault(r => r.id == id);

            if (result == null)
                throw new NotFoundException("Restaurant Not found");

            var restaruantsDto = _mapper.Map<RestaurantDto>(result);
            return restaruantsDto;
        }
        public PageResult<RestaurantDto> GetAll(RestaurantQuery query)
        {
            var baseQuery = _dbContext.Restaurants
            .Include(a => a.Adress)
            .Include(a => a.Dishes)
            .Where(r => query.searchPhrase == null ||
                (r.Name.ToLower().Contains(query.searchPhrase.ToLower()) || r.Description.ToLower().Contains(query.searchPhrase.ToLower())));



            if(query.SortBy!=null)
            {
                var SortBySelector = new Dictionary<string, Expression<Func<Restaurant, object>>>
                {
                    {nameof(Restaurant.Name),r=>r.Name },
                    {nameof(Restaurant.Category),r=>r.Category },
                    {nameof(Restaurant.Description),r=>r.Description }
                };
                var selectroToOrderBy = SortBySelector[query.SortBy];
                baseQuery = query.SortingDirection == SortingDirection.ASC ? baseQuery.OrderBy(selectroToOrderBy) : baseQuery.OrderByDescending(selectroToOrderBy);
            }


            var restaurants = baseQuery
            .Skip(query.pageSize*(query.pageNumber-1))
            .Take(query.pageSize)
            .ToList();

            var totalCount = baseQuery.Count();
            var restaruantsDto = _mapper.Map<List<RestaurantDto>>(restaurants);

            var result = new PageResult<RestaurantDto>(restaruantsDto, totalCount, query.pageSize, query.pageNumber);

            return result;
        }


        public int CreateRestaurant(CreateRestaurantDto dto)
        {

            var restaurant = _mapper.Map<Restaurant>(dto);
            restaurant.CreatedById = _userContextService.GetUserId;
            _dbContext.Restaurants.Add(restaurant);
            _dbContext.SaveChanges();

            return restaurant.id;
        }

        public void Delete(int id)
        {
            _logger.LogWarning($"Restaurant  wit ID {id} will be deleted");
            var restaurant = _dbContext.Restaurants.FirstOrDefault(r => r.id == id);
            if (restaurant == null)
                throw new NotFoundException("Restaurant not found");

            var authResult = _authorization.AuthorizeAsync(_userContextService.User, restaurant, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authResult.Succeeded)
            {
                throw new ForbidException();
            }

            _dbContext.Restaurants.Remove(restaurant);
            _dbContext.SaveChanges();
            
        }

        public void  UpdateRestaurant(int id,UpdateRestaurantDto dto)
        {
            if (!_dbContext.Restaurants.Any(r => r.id == id))
                throw new NotFoundException("Restaurant not found");

            

            var restaurant = _dbContext.Restaurants.FirstOrDefault(r => r.id == id);

            var authResult = _authorization.AuthorizeAsync(_userContextService.User, restaurant, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if(!authResult.Succeeded)
            {
                throw new ForbidException();
            }

            if (dto.Description != null)
                restaurant.Description = dto.Description;
            if (dto.HasDelivery == true || dto.HasDelivery == false)
                restaurant.HasDelivery = dto.HasDelivery;
            if (dto.Name != null)
                restaurant.Name = dto.Name;
            _dbContext.Restaurants.Update(restaurant);
            _dbContext.SaveChanges();

        }


    }
}
