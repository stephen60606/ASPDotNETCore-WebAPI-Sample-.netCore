namespace NetCore.WebAPI.Models
{
    /// <summary>
    /// Generic base request
    /// </summary>
    /// <typeparam name="T">Request、Response Data Type</typeparam>
    public class BaseRequest<T>
    {
        /// <summary>
        /// Request Parameters
        /// </summary>
        public ReqHeader ReqHeader { get; set; }

        /// <summary>
        /// Request transaction body
        /// </summary>
        public T TransReq { get; set; }
    }

    /// <summary>
    /// Request Parameters
    /// </summary>
    public class ReqHeader
    {
        /// <summary>
        /// Request sequence value for log
        /// </summary>
        /// <example></example>
        public string ReqSeq { get; set; }

        /// <summary>
        /// Channel id
        /// </summary>
        /// <example>JSIB</example>
        public string ChannelId { get; set; }
    }
}

