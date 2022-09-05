using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApplication44Udemy.Authorization
{
    public class MinimumAgeRequirementHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        private readonly ILogger<MinimumAgeRequirement> _logger;
        //DEBUG!!!
        public MinimumAgeRequirementHandler(ILogger<MinimumAgeRequirement> logger)
        {
            _logger = logger;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {
            var dateOfBirth = DateTime.Parse(context.User.FindFirst(c => c.Type == "DateOfBirth").Value);
            var email = context.User.FindFirst(c => c.Type == ClaimTypes.Name).Value;

            _logger.LogInformation("Logging user "+ email );
            if(dateOfBirth.AddYears(requirement.MinimumAge) <= DateTime.Now)
            {
                _logger.LogInformation("Sucssed " + email);
                context.Succeed(requirement);
            }
            else
            {
                _logger.LogInformation("Failed for " + email);
            }


            return Task.CompletedTask;
        }
    }
}
