using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication44Udemy.Entities;
using WebApplication44Udemy.Models;
using Microsoft.AspNetCore.Identity;
using WebApplication44Udemy.Exceptions;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace WebApplication44Udemy.Services
{
    public interface IAccountService
    {
        public void RegisterUser(RegisterUserDto dto);
        public string GenerateJwt(LoginDto dto);
    }
    public class AccountService:IAccountService
    {
        private readonly RestaurantDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<Users> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;

        public AccountService(RestaurantDbContext context,IMapper mapper,IPasswordHasher<Users> passwordHasher,AuthenticationSettings authenticationSettings)
        {
            _context = context;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
        }
        public void  RegisterUser(RegisterUserDto dto)
        {
            var newUser = new Users
            {
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,
                Nationality = dto.Nationality,
                RoleId = dto.RoleId
            };
            var hashedPassword = _passwordHasher.HashPassword(newUser,dto.Password);
            newUser.PasswordHash = hashedPassword;
            _context.Users.Add(newUser);
            _context.SaveChanges();  
        }
        public string GenerateJwt(LoginDto dto)
        {
            var user = _context.Users.Include(u=>u.Role)
                .FirstOrDefault(u => u.Email == dto.Email);
            
            if(user==null)
            {
                throw new BadRequestException("wrong email");
            }
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result==PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("wrong email");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,$"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role,$"{user.Role.Name}"),
                new Claim("DateOfBirth",user.DateOfBirth.Value.ToString("yyyy-MM-dd")),               
            };

            if(!string.IsNullOrEmpty(user.Nationality))
            {
                claims.Add(new Claim("Nationality", user.Nationality));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var kred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);
            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer, _authenticationSettings.JwtIssuer,claims,
                expires: expires, signingCredentials: kred);

            var TokenHandler = new JwtSecurityTokenHandler();
            return TokenHandler.WriteToken(token);
        }
    }
}
