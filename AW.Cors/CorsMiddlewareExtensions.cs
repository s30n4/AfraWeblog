using Microsoft.AspNetCore.Builder;

namespace AW.Cors
{
    public static class CorsMiddlewareExtensions
    {
        // Extension method used to add the middleware to the HTTP request pipeline.
        public static IApplicationBuilder UseCorsOrigin(this IApplicationBuilder builder, string corsProfile)
        {
            return builder.UseMiddleware<CorsMiddleware>(corsProfile);
        }
    }
}