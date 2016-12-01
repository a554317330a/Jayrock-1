using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo.Handlers.API.Response
{
    /// <summary>
    /// 列表记录响应
    /// </summary>
    public class ListActionResult : ActionResult
    {
        public ListActionResult()
        {
            Property = new ListProperty();
        }

        /// <summary>
        /// 列表数据
        /// </summary>
        public Object List { get; set; }

        /// <summary>
        /// 响应属性
        /// </summary>
        public ListProperty Property { get; set; }
    }
}
