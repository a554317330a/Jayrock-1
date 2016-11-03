using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo.Handlers.API.Response
{
    /// <summary>
    /// 响应错误信息
    /// </summary>
    public class Error
    {
        /// <summary>
        /// 业务提示信息
        /// </summary>
        public String SubMsg { get; set; }

        /// <summary>
        /// 业务响应码
        /// </summary>
        public SubCode? SubCode { get; set; }
    }
}
