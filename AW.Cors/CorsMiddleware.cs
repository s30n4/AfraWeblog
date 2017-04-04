using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AW.Cors
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CorsMiddleware
    {
        private readonly RequestDelegate _next;
        private string CorsProfile { get; }

        public CorsMiddleware(RequestDelegate next, string corsProfile)
        {
            _next = next;
            CorsProfile = corsProfile;
        }
        //https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Transfer-Encoding
        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Method == "OPTIONS")
            {
                httpContext.Response.StatusCode = 200;
                if (!httpContext.Response.Headers.ContainsKey("Access-Control-Allow-Methods"))
                {
                    httpContext.Response.Headers["Access-Control-Allow-Methods"] = httpContext.Request.Headers["Access-Control-Request-Method"];

                    httpContext.Response.Headers["Access-Control-Allow-Headers"] = new[] { "Authorization", "Content-Type" };

                    httpContext.Response.Headers["Access-Control-Allow-Origin"] = "*";
                    return;
                }

            }else
            {
                var cros = new CorsService(httpContext);

                var res = cros.EvaluatePolicy(CorsProfile);

                cros.ApplyCors(res);
            }
            await _next(httpContext);
        }
    }

}