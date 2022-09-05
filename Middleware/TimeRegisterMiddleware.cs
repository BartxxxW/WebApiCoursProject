using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication44Udemy.Exceptions;

namespace WebApplication44Udemy.Middleware
{
    public class TimeRegisterMiddleware : IMiddleware
    {
        private readonly ILogger<TimeRegisterMiddleware> _logger;
        public TimeRegisterMiddleware(ILogger<TimeRegisterMiddleware> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Stopwatch a = new Stopwatch();
            a.Start();
            await next.Invoke(context);
            a.Stop();
            if (a.Elapsed.TotalSeconds > 4)
                _logger.LogInformation("MOre than 4 seconds");

        }
    }
}
