using System;
namespace NetCore.Exceptions
{
    /// <summary>
    /// Exception for manually throw
    /// </summary>
    [Serializable]
    public class BusinessException : Exception
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public BusinessException(string message)
        {
            this.ErrorMessage = message;
        }

        public BusinessException(string errorCode, string message) : this(message)
        {
            this.ErrorCode = errorCode;
        }
    }
}

