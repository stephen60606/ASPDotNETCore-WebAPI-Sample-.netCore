using Newtonsoft.Json;

namespace NetCore.WebAPI.Models
{
    /// <summary>
    /// unite type of API Response
    /// </summary>
    public class BaseResponse<T>
    {
        /// <summary>
        /// response parameters
        /// </summary>
        [JsonProperty("respHeader")]
        public ResponseHeader RespHeader { get; set; } = new ResponseHeader();

        /// <summary>
        /// response transaction body
        /// </summary>
        [JsonProperty("transResp")]
        public T TransResp { get; set; }
    }

    /// <summary>
    /// response parameters
    /// </summary>
    public class ResponseHeader
    {
        /// <summary>
        /// Request sequence value for log
        /// </summary>
        [JsonProperty("reqSeq")]
        public string ReqSeq { get; set; }
        /// <summary>
        /// Response Return Code
        /// </summary>
        [JsonProperty("returnCode")]
        public string ReturnCode { get; set; }
        /// <summary>
        /// Response Message
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }
        /// <summary>
        /// Response ExceptionData
        /// </summary>
        [JsonProperty("exceptionData")]
        public object ExceptionData { get; set; }
    }
}

