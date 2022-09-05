using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication44Udemy.Entities;

namespace WebApplication44Udemy
{
    public class RestaurantSeeder
    {
        private readonly RestaurantDbContext _dbContext;
        public RestaurantSeeder(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Seed()
        { 
            if(_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }
                if (!_dbContext.Restaurants.Any())
                {
                    var restaurants = GetRestaurants();
                    _dbContext.Restaurants.AddRange(restaurants);
                    _dbContext.SaveChanges();
                }

            }

        }
        public IEnumerable<Role> GetRoles()
        {
            var Roles = new List<Role>()
            {
                new Role()
                {
                    Name="User"
                },

                new Role()
                { 
                    Name="Admin"
                },
                new Role()
                {
                    Name="Manager"
                }

            };
            return Roles;
            
        }
        public IEnumerable<Restaurant> GetRestaurants()
        {
            var restaurants = new List<Restaurant>()
            {
                new Restaurant()
                {
                    Name="KFC",
                    Category="Fast Food",
                    Description="KFC (short for Ketucky Fried Chicken) is an American fast food restaurant chain headquartered",
                    ContactEmail="contact@kfc.com",
                    HasDelivery=true,
                    Dishes= new List<Dish>()
                    {
                        new Dish()
                        {
                            Name="Nashville Hot Chicken",
                            Price=10.30M
                        },
                        new Dish()
                        {
                            Name="Chicken Nuggets",
                            Price=5.30M
                        },
                    },
                    Adress=new Adress()
                    {
                        City="Kraków",
                        Street="Długa 5",
                        PostalCode="30-001"
                    }
                    
                },
                new Restaurant()
                {
                    Name="McDonald",
                    Category="Fast Food",
                    Description="McDonald Corporation, incorprated on December 1964 is an American fast food restaurant chain headquartered",
                    ContactEmail="contact@mcdonald.com",
                    HasDelivery=true,
                    Adress=new Adress()
                    {
                        City="Kraków",
                        Street="Długa 5",
                        PostalCode="30-001"
                    }

                }


            };
            return restaurants;
        }
    }
}
