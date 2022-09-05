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

namespace WebApplication44Udemy.Controllers
{
    [Route("api/{RestaurantId}/dish")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }
        [HttpPost]
        public ActionResult CreateDish([FromRoute] int restaurantId, [FromBody] CreateDishDto dto)
        {
            var dishId = _dishService.CreateDish(restaurantId, dto);
            return Created($"/api/{restaurantId}/dish/{dishId}", null);
        }
        [HttpGet("{dishId}")]
        public ActionResult<Dish> GetById([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            var dish = _dishService.GetById(restaurantId, dishId);
            return Ok(dish);
        }
        [HttpGet]
        public ActionResult<List<DishDto>> GetAll([FromRoute] int restaurantId)
        {
            var dishLIst = _dishService.GetAll(restaurantId);
            return Ok(dishLIst);
        }
        [HttpDelete]
        public ActionResult RemoveAll([FromRoute] int restaurantId)
        {
            _dishService.RemoveAll(restaurantId);
            return NoContent();
        }
        [Route("/api/dish/{dishId}")]
        [HttpDelete]
        public ActionResult RemoveById([FromRoute] int dishId)
        {
            _dishService.RemoveByID(dishId);
            return NoContent();
        }

    }
}
