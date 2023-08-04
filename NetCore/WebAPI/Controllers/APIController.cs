using Microsoft.AspNetCore.Mvc;
using NetCore.Extensions;
using NetCore.WebAPI.Models;
using Newtonsoft.Json;


namespace NetCore.WebAPI.Controllers
{
    /// <summary>
    /// provide specific response
    /// </summary>
    public class APIController : ControllerBase
    {
        protected internal virtual IActionResult Success<T>(T content)
        {
            var req = JsonConvert.DeserializeObject<BaseRequest<dynamic>>(HttpContext.Request.FetchRequestBodyAsync().Result);

            var resp = new BaseResponse<T>()
            {
                RespHeader = new ResponseHeader()
                {
                    ReqSeq = req?.ReqHeader?.ReqSeq,
                    ReturnCode = ReturnCodes.Success_0000.ToDisplayName(),
                    Message = ReturnMessages.GetMessage(ReturnCodes.Success_0000)
                },
                TransResp = content
            };

            return Ok(resp);
        }

        protected internal virtual IActionResult Failure(ReturnCodes returnCode = ReturnCodes.Failure_9999)
        {
            var req = JsonConvert.DeserializeObject<BaseRequest<dynamic>>(HttpContext.Request.FetchRequestBodyAsync().Result);

            var resp = new BaseResponse<object>()
            {
                RespHeader = new ResponseHeader()
                {
                    ReqSeq = req?.ReqHeader?.ReqSeq,
                    ReturnCode = returnCode.ToDisplayName(),
                    Message = ReturnMessages.GetMessage(returnCode)
                }
            };

            return Ok(resp);
        }
    }
}

