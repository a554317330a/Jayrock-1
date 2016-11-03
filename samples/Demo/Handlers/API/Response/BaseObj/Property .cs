using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo.Handlers.API.Response
{
    public class Property
    {
        /// <summary>
        /// 对象名
        /// </summary>
        public String ObjectName { get; set; }

        /// <summary>
        /// 是否列表
        /// </summary>
        public virtual bool IsList { get { return false; } set{} }
    }
}
