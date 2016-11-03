using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo.Handlers.API.Response
{
    /// <summary>
    /// 单记录响应
    /// </summary>
    public class SingleActionResult : ActionResult
    {
        public SingleActionResult()
        {
            Property = new Property();
        }

        /// <summary>
        /// 呈现的数据
        /// </summary>
        public Object Object { get; set; }

        /// <summary>
        /// 响应属性
        /// </summary>
        public Property Property { get; set; }
    }
}
