using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Jayrock.Json;

namespace Demo.Handlers.API.Response
{
    /// <summary>
    /// 响应结果
    /// </summary>
    public class ActionResult : JsonObject
    {
        public ActionResult()
        {
            Error = new Error();
        }

        /// <summary>
        /// 响应代码
        /// </summary>
        public Error Error { get; set; }

        /// <summary>
        /// 获取响应结果
        /// </summary>
        /// <returns></returns>
        public ActionResult GetActionResult()
        {
            var lar = new ActionResult
            {
                Error = this.Error
            };
            lar.Error.SubCode = this.Error.SubCode;
            lar.Error.SubMsg = this.Error.SubMsg;

            //判断SubCode是否设置正确
            if (lar.Error.SubCode != null)
            {
                if (lar.Error.SubCode == Response.SubCode.SessionfulPrompt && String.IsNullOrEmpty(lar.Error.SubMsg)) throw new Exception("SubCode为-1时，需要设置ErrMsg值！");
                if (lar.Error.SubCode == Response.SubCode.Successful && !String.IsNullOrEmpty(lar.Error.SubMsg)) throw new Exception("SubCode为0时，不允许设置ErrMsg值！");
                if (lar.Error.SubCode == Response.SubCode.SuccessfulPrompt && String.IsNullOrEmpty(lar.Error.SubMsg)) throw new Exception("SubCode为1时，需要设置ErrMsg值！");
                if (lar.Error.SubCode == Response.SubCode.Failing && !String.IsNullOrEmpty(lar.Error.SubMsg)) throw new Exception("SubCode为2时，不允许设置ErrMsg值！");
                if (lar.Error.SubCode == Response.SubCode.FailingPrompt && String.IsNullOrEmpty(lar.Error.SubMsg)) throw new Exception("SubCode不为3时，需要设置ErrMsg值！");
            }

            //包装异常
            if (HttpContext.Current.IsDebuggingEnabled)//判断是否为测试环境
            {
                if (this.Error.SubMsg.Contains("获取发生异常"))//如果为异常
                {
                    
                }
            }
            else//正式环境使用包装异常
            {
                if (this.Error.SubMsg.Contains("获取发生异常"))//如果为异常
                {
                    Error.SubMsg = "网络不给力!";
                }
            }

            //装载JSON对象
            JsonObject errorObject = new JsonObject();
            errorObject.Put("SubCode", (this.Error.SubCode == null ? (int) SubCode.Failing : (int) this.Error.SubCode));
            errorObject.Put("SubMsg", (string.IsNullOrEmpty(this.Error.SubMsg) ? "" : this.Error.SubMsg));
            lar.Put("Error", errorObject);

            return lar;
        }
    }
}
