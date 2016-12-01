using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Jayrock.Json;

namespace Demo.Handlers.API.Response
{
    /// <summary>
    /// 单记录快速响应容器
    /// </summary>
    public class SingleFast : IFastResult
    {
        /// <summary>
        /// 数据
        /// </summary>
        public object Object { get; set; }

        /// <summary>
        /// 业务响应代码
        /// </summary>
        public SubCode SubCode { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        public String SubMsg { get; set; }
        
        /// <summary>
        /// 对象名
        /// </summary>
        public String ObjectName { get; set; }

        #region IFastResult 成员

        public ActionResult GetActionResult()
        {
            var lar = new SingleActionResult
            {
                Object = Object
            };
            lar.Error.SubCode = SubCode;
            lar.Error.SubMsg = SubMsg;
            lar.Property.ObjectName = ObjectName;


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
                if (this.Object is Exception)//如果为异常
                {
                    this.Object = new JsonObject();
                }
            }
            else//正式环境使用包装异常
            {
                if (this.Object is Exception)//如果为异常
                {
                    this.Object = new JsonObject();
                    lar.Error.SubMsg = "网络不给力!";
                }
            }

            //装载JSON对象
            lar.Put("Obj", this.Object);

            JsonObject errorObject = new JsonObject();
            errorObject.Put("SubCode", (lar.Error.SubCode == null ? (int)SubCode.Failing : (int)lar.Error.SubCode));
            errorObject.Put("SubMsg", (string.IsNullOrEmpty(lar.Error.SubMsg) ? "" : lar.Error.SubMsg));
            lar.Put("Error", errorObject);

            JsonObject propertyObject = new JsonObject();
            propertyObject.Put("IsList", lar.Property.IsList);
            propertyObject.Put("ObjectName", (string.IsNullOrEmpty(lar.Property.ObjectName) ? "" : lar.Property.ObjectName));
            lar.Put("Property", propertyObject);
            return lar;
        }

        #endregion
    }
}
