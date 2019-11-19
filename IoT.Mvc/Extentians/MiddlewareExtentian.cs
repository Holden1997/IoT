using IoT.Mvc.Module;
using Microsoft.AspNetCore.Builder;

namespace IoT.Mvc.Extentians
{
    public static class BuilderExtensions
    {
        public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<Middleware>();
        }
    }
}
