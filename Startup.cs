using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebApplication44Udemy.Authorization;
using WebApplication44Udemy.Entities;
using WebApplication44Udemy.Middleware;
using WebApplication44Udemy.Models;
using WebApplication44Udemy.Models.Validators;
using WebApplication44Udemy.Services;


namespace WebApplication44Udemy
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var authenticationSettings = new AuthenticationSettings();

            Configuration.GetSection("Authentication").Bind(authenticationSettings);
            services.AddSingleton(authenticationSettings);
            services.AddAuthentication(
                option =>
                {
                    option.DefaultAuthenticateScheme = "Bearer";
                    option.DefaultScheme = "Bearer";
                    option.DefaultChallengeScheme = "Bearer";
                }
                ).AddJwtBearer(cfg=>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters {
                        ValidIssuer = authenticationSettings.JwtIssuer,
                        ValidAudience= authenticationSettings.JwtIssuer,
                        IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))
                        ///To Complete autentykacja przez tokeny JWT
                    };

                }
                );

            services.AddAuthorization(options =>
            {
                options.AddPolicy("HasNationality", builder => builder.RequireClaim("Nationality"));
                options.AddPolicy("AtLeast20", buildier => buildier.AddRequirements(new MinimumAgeRequirement(20)));
                options.AddPolicy("AtLeast2Restaurants", buildier => buildier.AddRequirements(new MinimumRestaurantsRequirement(1)));
            }
                
            );
            services.AddScoped<IAuthorizationHandler, MinimumRestaurantsRequirementHandler>();
            services.AddScoped<IAuthorizationHandler,MinimumAgeRequirementHandler>();
            services.AddScoped<IAuthorizationHandler,ResourceOperationRequirementHandler>();
            services.AddControllers().AddFluentValidation();
            services.AddDbContext<RestaurantDbContext>(options=> options.UseSqlServer(Configuration.GetConnectionString("RestaurantDbConnection"))); 
            services.AddScoped<RestaurantSeeder>();
            services.AddAutoMapper(this.GetType().Assembly);
            services.AddScoped<IRestaurantService, RestaurantService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IDishService, DishService>();
            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddScoped<TimeRegisterMiddleware>();
            services.AddScoped<IUserContextService, UserContextService>();
            services.AddScoped<IPasswordHasher<Users>, PasswordHasher<Users>>();
            services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
            services.AddScoped<IValidator<RestaurantQuery>, RestaurantQueryValidator>();
            services.AddHttpContextAccessor();
            services.AddSwaggerGen();
            services.AddCors(options => {
                options.AddPolicy("FrontEndClient", builder => builder.AllowAnyMethod().AllowAnyHeader().WithOrigins(@"http://localhost:8080/"));
                });
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RestaurantSeeder seeder)
        {
            app.UseResponseCaching();
            app.UseStaticFiles();
            app.UseCors("FrontEndClient");
            seeder.Seed();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); 
            }
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<TimeRegisterMiddleware>();
            app.UseSwagger();
            app.UseSwaggerUI(c=>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Restaurant API");
            });
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
