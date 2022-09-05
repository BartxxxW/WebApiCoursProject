using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication44Udemy.Authorization
{
    public class MinimumAgeRequirement : IAuthorizationRequirement
    {
        public int  MinimumAge {get;}
        public MinimumAgeRequirement(int iAge)
        {
            MinimumAge = iAge;
        }
    }
}
