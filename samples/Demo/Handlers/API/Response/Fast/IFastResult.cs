using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo.Handlers.API.Response
{
    /// <summary>
    /// 快速结果接口
    /// </summary>
    public interface IFastResult
    {
        /// <summary>
        /// 获取响应结果
        /// </summary>
        /// <returns></returns>
        ActionResult GetActionResult();
    }
}
