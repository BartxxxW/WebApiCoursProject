using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication44Udemy.Entities;
using WebApplication44Udemy.Models;
using WebApplication44Udemy.Services;
using NLog.Web;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WebApplication44Udemy.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    [Authorize]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult CreateRestaurant(CreateRestaurantDto dto)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var id = _restaurantService.CreateRestaurant(dto);

            return Created($"restaurant/{id}", null);
        }
        [HttpPut("{id}")]
        public ActionResult UpdateRestaurant([FromRoute] int id, [FromBody] UpdateRestaurantDto dto)
        {
            _restaurantService.UpdateRestaurant(id, dto);
            return Ok("Restaurant Updated");
        }

        //[Authorize(Policy = "AtLeast2Restaurants")]
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<RestaurantDto>> GetAll([FromQuery] RestaurantQuery query)
        {
            //var resultAdress= await _dbContext.Adresses.ToListAsync();
            //var resultDishes = await _dbContext.Dishes.ToListAsync();

            var restaruantsDto = _restaurantService.GetAll(query);
            if (restaruantsDto == null)
                return BadRequest("sth wrong Get all");
            //var restaurantsDto = resultRestaurant.Select(r => new RestaurantDto
            //{
            //    id = r.id,
            //    Name = r.Name,
            //    Description = r.Name,
            //    Category = r.Category,
            //    HasDelivery = r.HasDelivery,

            //    City = resultAdress.Where(a=>a.Id==r.AdressId).Select(a=>a.City).ToList()[0],
            //    Street = resultAdress.Where(a => a.Id == r.AdressId).Select(a => a.Street).ToList()[0],
            //    PostalCode = resultAdress.Where(a => a.Id == r.AdressId).Select(a => a.PostalCode).ToList()[0],
            //    Dishes = resultDishes.Where(d => d.RestaurantId == r.id).Select(d=> new DishDto { 

            //        Id=d.Id,
            //        Name=d.Name,
            //        Description=d.Description,
            //        Price=d.Price
            //    }).ToList()
            //}); 
            return Ok(restaruantsDto);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AtLeast20")]
        public ActionResult<RestaurantDto> GetRestaurantById([FromRoute] int id)
        {

            var restaruantsDto = _restaurantService.GetById(id);
            return Ok(restaruantsDto);
        }
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute]int id)
        {
            _restaurantService.Delete(id);
            return NotFound();
        }

    }
}
