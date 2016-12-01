using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Demo.API.API.V3 
{
    public partial class BaseHandler : Demo.Handlers.API.HandlerBase
    {
        public BaseHandler() : base(true)
        {
            API_VERSION = "V1";//设置版本
        }

        /// <summary>
        /// API加密验证
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override bool RequestSecurity(System.Web.HttpContext context)
        {
            //Sign验证
            return true;
        }

        /// <summary>
        /// 输出数据加密
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public override System.Collections.IDictionary EncryptResponse(System.Collections.IDictionary response)
        {
            //加密
            return base.EncryptResponse(response);
        }

        /// <summary>
        /// 输入数据解密
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public override TextReader DecryptRequest(string request)
        {
            //解密
            return base.DecryptRequest(request);
        }
    }
}

