using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo.Handlers.API.Response
{
    /// <summary>
    /// Single record response
    /// </summary>
    public class SingleActionResult : ActionResult
    {
        public SingleActionResult()
        {
            Property = new Property();
        }

        /// <summary>
        /// Presented data
        /// </summary>
        public Object Object { get; set; }

        /// <summary>
        /// Response properties
        /// </summary>
        public Property Property { get; set; }
    }
}
