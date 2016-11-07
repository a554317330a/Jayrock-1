using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo.Handlers.API.Response
{
    /// <summary>
    /// Response error message
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Business tips information
        /// </summary>
        public String SubMsg { get; set; }

        /// <summary>
        /// Service response code
        /// </summary>
        public SubCode? SubCode { get; set; }
    }
}
