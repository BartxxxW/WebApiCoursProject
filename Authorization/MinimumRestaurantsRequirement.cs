using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication44Udemy.Authorization
{
    public class MinimumRestaurantsRequirement:IAuthorizationRequirement
    {
        public int NumberOfCreatedRestaurants { get; }

        public MinimumRestaurantsRequirement(int numberOfRestaurants)
        {
            NumberOfCreatedRestaurants = numberOfRestaurants;
        }
    }
}
