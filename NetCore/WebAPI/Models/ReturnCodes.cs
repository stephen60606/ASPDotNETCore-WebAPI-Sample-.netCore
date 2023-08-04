using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace NetCore.WebAPI.Models
{
    /// <summary>
    /// Customized return-code in response
    /// </summary>
    public enum ReturnCodes
    {
        /// <summary>
        /// success
        /// </summary>
        [Display(Name = "0000")]
        Success_0000,

        /// <summary>
        /// failure
        /// </summary>
        [Display(Name = "9999")]
        Failure_9999,

    }
}

