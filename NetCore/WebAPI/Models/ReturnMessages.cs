using NetCore.Exceptions;
using NetCore.Extensions;

namespace NetCore.WebAPI.Models
{
    /// <summary>
    /// return message
    /// </summary>
    public static class ReturnMessages
    {
        /// <summary>
        /// match returnCode and returnMessage
        /// </summary>
        private static IDictionary<ReturnCodes, string> MessageDic = new Dictionary<ReturnCodes, string> {
            { ReturnCodes.Success_0000, "Success"  },
            { ReturnCodes.Failure_9999, "Failure" },
    };
        /// <summary>
        /// get messasge string 
        /// </summary>
        /// <param name="returnCode">returnCode</param>
        public static string GetMessage(ReturnCodes returnCode)
        {
            if (!MessageDic.ContainsKey(returnCode))
                throw new BusinessException($"ReturnCode {returnCode.ToDisplayName()} not defined");

            return MessageDic[returnCode];
        }
    }
}

