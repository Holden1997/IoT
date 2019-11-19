using IoT.Common.Models;
using IoT.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;


namespace IoT.WebApi.Attributs
{
    public class ExeptionFilltreAttribute : Attribute, IAsyncExceptionFilter
    {
        IExeptionLogger _exeptionLogger;

        public ExeptionFilltreAttribute(IExeptionLogger exeptionLogger)
        {
            _exeptionLogger = exeptionLogger;
        }
        public  Task OnExceptionAsync(ExceptionContext context)
        {
            Exeption exeption = new Exeption
            {
                ExeptionId = Guid.NewGuid().ToString().Substring(8),
                ActionNmae = context.ActionDescriptor.DisplayName,
                ExeptionMessage = context.Exception.Message,
                ExeptionStack = context.Exception.StackTrace,
            };
            context.Result = new ContentResult
            {
                Content = exeption.ExeptionMessage,
                ContentType = "application/json",
                StatusCode = 400
            };


            _exeptionLogger.Logger(exeption);

            return Task.CompletedTask;
        }
    }
}
