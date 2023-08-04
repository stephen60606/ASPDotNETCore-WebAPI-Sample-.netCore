using System.Net.Http;
using Microsoft.AspNetCore.Diagnostics;
using NetCore.Exceptions;
using NetCore.Extensions;
using NetCore.WebAPI.Models;
using Newtonsoft.Json;

namespace NetCore.WebAPI.Middlewares
{
    public static class WebAPIExceptionHandler
    {
        /// <summary>
        /// centralized exception handle
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <returns></returns>
        public static async Task ExceptionHandleAsync(this HttpContext context)
        {
            var ex = context.Features.Get<IExceptionHandlerFeature>();

            // TODO : Log Exception

            await WriteResponseAsync(context, ex.Error);
        }

        /// <summary>
        /// Write exception to response body
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="ex">Exception</param>
        /// <param name="configuration">configuration</param>
        /// <returns></returns>
        public static async Task WriteResponseAsync(HttpContext context, Exception ex, IConfiguration configuration = null)
        {
            #region Prepare response

            var req = JsonConvert.DeserializeObject<BaseRequest<dynamic>>(context.Request.FetchRequestBodyAsync().Result);

            var response = new BaseResponse<object>()
            {
                RespHeader = new ResponseHeader()
                {
                    ReqSeq = req?.ReqHeader?.ReqSeq,
                    ReturnCode = ReturnCodes.Failure_9999.ToDisplayName(),
                    Message = ReturnMessages.GetMessage(ReturnCodes.Failure_9999),
                    ExceptionData = configuration.GetValue<bool>("DeBugMode") == true ? ex.InnerException ?? ex : null
                }
            };

            if (ex is BusinessException)
            {
                var be = ex as BusinessException;
                response.RespHeader.Message = be.ErrorMessage;
                response.RespHeader.ReturnCode = string.IsNullOrEmpty(be.ErrorCode) ? response.RespHeader.ReturnCode : be.ErrorCode;
            }

            #endregion Prepare response

            try
            {
                context.Response.StatusCode = StatusCodes.Status200OK;
                context.Response.ContentType = "application/json";
                var json = JsonConvert.SerializeObject(response);
                await context.Response.WriteAsync(json);
            }
            catch
            {
                response.RespHeader.ExceptionData = new { (response.RespHeader.ExceptionData as Exception).Message, (response.RespHeader.ExceptionData as Exception).StackTrace };
                var json = JsonConvert.SerializeObject(response);
                await context.Response.WriteAsync(json);
            }
        }
    }
}

