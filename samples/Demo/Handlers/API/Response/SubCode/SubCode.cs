using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo.Handlers.API.Response
{
    /// <summary>
    /// 业务响应码
    /// </summary>
    public enum SubCode
    {
        /// <summary>
        /// Session失效+信息提示
        /// </summary>
        SessionfulPrompt = -1,
        /// <summary>
        /// 业务执行正常
        /// </summary>
        Successful = 0,
        /// <summary>
        /// 业务执行正常+信息提示
        /// </summary>
        SuccessfulPrompt = 1,
        /// <summary>
        /// 业务执行失败
        /// </summary>
        Failing = 2,
        /// <summary>
        /// 业务执行失败+信息提示
        /// </summary>
        FailingPrompt = 3
    }
}
