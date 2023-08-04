using System.Security.Claims;

namespace NetCore.WebContext
{
    public static class WebContext
    {
        private static IHttpContextAccessor httpContextAccessor;

        public static HttpContext Current => httpContextAccessor.HttpContext;


        public static void Configure(this IHttpContextAccessor httpContextAccessor)
        {
            WebContext.httpContextAccessor = httpContextAccessor;
        }

        public static string FetchHttpContextHeader(string key)
        {
            if (WebContext.Current != null)
            {
                var result = WebContext.Current.Request.Headers[key];
                if (!string.IsNullOrEmpty(result)) return result;
            }

            return string.Empty;
        }


        /// <summary>
        /// get current user data
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string FetchClaimsPrincipal(string key)
        {
            var claimsPrincipal = WebContext.Current.User as ClaimsPrincipal;

            if (WebContext.Current != null && claimsPrincipal != null)
            {
                var result = claimsPrincipal.Claims.SingleOrDefault(o => o.Type == key);
                if (result != null) return result.Value;
            }

            return string.Empty;
        }

    }
}

