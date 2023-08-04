using System.Diagnostics;
using Castle.DynamicProxy;
using NetCore.Logging;
using NLog;

namespace NetCore.WebAPI.Middlewares
{
    /// <summary>
    /// middleware for HttpContext default setting
    /// </summary>
    public class SetDefautMiddleware
    {
        #region initialization

        private readonly RequestDelegate next;

        public SetDefautMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        #endregion initialization

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();

            await next(context);
        }
    }

    /// <summary>
    /// interceptor
    /// </summary>
    public class LoggingInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();
        }
    }


    /// <summary>
    /// set middleware for monitoring(e.g. res req time, exception...)
    /// </summary>
    public class ReqRespMiddleware
    {

        //both works for writing logs
        private readonly ILogProvider logProvider;
        protected readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly RequestDelegate next;
        private Stopwatch sw;


        public ReqRespMiddleware(RequestDelegate next, ILogProvider logProvider)
        {
            this.next = next;
            this.logProvider = logProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            #region filter for api request

            if (!context.Request.Path.StartsWithSegments("/api") || HttpMethods.IsOptions(context.Request.Method))
            {
                await next(context);
                return;
            }

            #endregion filter for api request

            var originalBody = context.Response.Body;
            this.sw = new Stopwatch();
            try
            {
                var req = await context.Request.FetchRequestBodyAsync();
                using (var ms = new MemoryStream())
                {
                    context.Response.Body = ms;

                    this.sw.Start();
                    try
                    {
                        await next(context);

                        //TODO : interceptor for AOP
                    }
                    catch (Exception e)
                    {
                        logger.Error("context: " + context + "," + "ex: " + e);
                    }
                    this.sw.Stop();

                    var resp = await context.Response.FetchResponseBody();

                    await ms.CopyToAsync(originalBody);
                }
            }
            finally
            {
                context.Response.Body = originalBody;
            }
        }

    }
}

