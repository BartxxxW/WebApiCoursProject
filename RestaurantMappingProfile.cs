using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication44Udemy.Entities;
using WebApplication44Udemy.Models;

namespace WebApplication44Udemy
{
    public class RestaurantMappingProfile: Profile
    {
        public RestaurantMappingProfile()
        {
            CreateMap<Restaurant, RestaurantDto>()
                .ForMember(r => r.PostalCode, m => m.MapFrom(s => s.Adress.PostalCode))
                .ForMember(r => r.Street, m => m.MapFrom(s => s.Adress.Street))
                .ForMember(r => r.City, m => m.MapFrom(s => s.Adress.City));

            CreateMap<Dish, DishDto>();
            CreateMap<CreateRestaurantDto, Restaurant>()
                .ForMember(r => r.Adress, m => m.MapFrom(s => new Adress()
                { City = s.City, PostalCode = s.PostalCode, Street = s.Street }));
            CreateMap<CreateDishDto, Dish>();
            CreateMap<RegisterUserDto, Users>();
        }

    }
}
