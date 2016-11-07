using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo.Handlers.API.Response
{
    public class ListProperty : Property
    {
        /// <summary>
        /// Whether list
        /// </summary>
        public override bool IsList { get { return true; } set{} }

        /// <summary>
        /// Page request current page
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Paging request page size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Whether or not there is a next page
        /// </summary>
        public bool IsNext { get; set; }

        /// <summary>
        /// Record number
        /// </summary>
        public int TotalSize { get; set; }

        /// <summary>
        /// PageCount
        /// </summary>
        public int TotalIndex { get; set; }
    }
}
