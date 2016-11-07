using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo.Handlers.API.Response
{
    public class Property
    {
        /// <summary>
        /// Object name
        /// </summary>
        public String ObjectName { get; set; }

        /// <summary>
        /// Whether list
        /// </summary>
        public virtual bool IsList { get { return false; } set{} }
    }
}
