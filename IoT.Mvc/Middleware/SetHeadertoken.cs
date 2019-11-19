using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT.Mvc.Module
{
    public class Middleware
    {
        RequestDelegate _next;

        public Middleware(RequestDelegate next)
        {
            _next = next;
        }

        public async  Task Invoke(HttpContext context)
        {
            var token =  context.Request.Cookies["token"];

            if(token!=null)  
                context.Request.Headers.Add("Authorization", "Bearer " + token);
            
            await _next(context);
        }
    }
}
