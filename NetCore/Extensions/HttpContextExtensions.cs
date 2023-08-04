using System;
using Microsoft.Extensions.Primitives;

namespace NetCore
{
    /// <summary>
    /// extension methods for HttpContext and its properties
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// fetch request as string
        /// </summary>
        /// <param name="request">HttpRequest</param>
        /// <returns></returns>
        public static async Task<string> FetchRequestBodyAsync(this HttpRequest request)
        {
            request.Body.Seek(0, SeekOrigin.Begin);
            var bodyAsText = await new StreamReader(request.Body).ReadToEndAsync();
            request.Body.Seek(0, SeekOrigin.Begin);

            return bodyAsText;
        }

        /// <summary>
        /// fetch response as string
        /// </summary>
        /// <param name="response">HttpResponse</param>
        /// <returns></returns>
        public static async Task<string> FetchResponseBody(this HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var bodyAsText = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return bodyAsText;
        }

        /// <summary>
        /// fetch client ip
        /// </summary>
        /// <param name="request">HttpRequest</param>
        /// <param name="ipHeaderName">default "X-Forwarded-For"</param>
        /// <returns></returns>
        public static string FetchClientIp(this HttpRequest request, string ipHeaderName = "X-Forwarded-For")
        {
            var ip = request.Headers.TryGetValue(ipHeaderName, out StringValues xForwardFors) && xForwardFors.Any() ?
                      xForwardFors.First().Split(",").First().Split(":").First().Trim() : request.HttpContext.Connection.RemoteIpAddress.ToString();
            return ip;
        }
    }
}

