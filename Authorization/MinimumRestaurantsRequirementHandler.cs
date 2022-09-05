using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication44Udemy.Entities;

namespace WebApplication44Udemy.Authorization
{
    public class MinimumRestaurantsRequirementHandler : AuthorizationHandler<MinimumRestaurantsRequirement>
    {
        private RestaurantDbContext _dbContex;
        public MinimumRestaurantsRequirementHandler(RestaurantDbContext dbContext)
        {
            _dbContex = dbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumRestaurantsRequirement requirement)
        {
            var userId = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            int nRest = _dbContex.Restaurants.Where(r => r.CreatedById == userId).Count();
            if (nRest >= requirement.NumberOfCreatedRestaurants)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
