using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo.Handlers.API.Response
{
    /// <summary>
    /// Quick result interface
    /// </summary>
    public interface IFastResult
    {
        /// <summary>
        /// Obtain response results
        /// </summary>
        /// <returns></returns>
        ActionResult GetActionResult();
    }
}
