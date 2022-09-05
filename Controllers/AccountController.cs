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
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountSservice;

        public AccountController(IAccountService accountService)
        {
            _accountSservice = accountService;
        }
        [HttpPost("register")]
        public ActionResult RegisterUser([FromBody] RegisterUserDto dto)
        {
            _accountSservice.RegisterUser(dto);
            return Ok();
        }
        [HttpPost("Login")]
        public ActionResult Login([FromBody] LoginDto dto)
        {
            var token = _accountSservice.GenerateJwt(dto);
            return Ok(token);
        }

    }
}
