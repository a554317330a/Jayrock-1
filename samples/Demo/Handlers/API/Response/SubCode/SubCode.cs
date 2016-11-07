using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo.Handlers.API.Response
{
    /// <summary>
    /// Service response code
    /// </summary>
    public enum SubCode
    {
        /// <summary>
        /// Session failure + information prompt
        /// </summary>
        SessionfulPrompt = -1,
        /// <summary>
        /// Business execution is normal
        /// </summary>
        Successful = 0,
        /// <summary>
        /// Business executive normal + message
        /// </summary>
        SuccessfulPrompt = 1,
        /// <summary>
        /// Business execution failure
        /// </summary>
        Failing = 2,
        /// <summary>
        /// Business execution failure + info prompt
        /// </summary>
        FailingPrompt = 3
    }
}
