using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo.Handlers.API.Response
{
    /// <summary>
    /// List log response
    /// </summary>
    public class ListActionResult : ActionResult
    {
        public ListActionResult()
        {
            Property = new ListProperty();
        }

        /// <summary>
        /// List data
        /// </summary>
        public Object List { get; set; }

        /// <summary>
        /// Response properties
        /// </summary>
        public ListProperty Property { get; set; }
    }
}
