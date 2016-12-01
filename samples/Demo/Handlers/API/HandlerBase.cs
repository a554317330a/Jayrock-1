using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.SessionState;
using Jayrock.Json;
using Jayrock.JsonRpc;
using Jayrock.JsonRpc.Web;
using Demo;

namespace Demo.Handlers.API
{
    public class HandlerBase : JsonRpcHandler, IRequiresSessionState
    {
        #region 常量成员

        protected string API_VERSION = "";//api的版本
        
        #endregion

        #region 构造

        private bool requestSecurity = false;
        public HandlerBase(bool _requestSecurity)
        {
            requestSecurity = _requestSecurity;
        }

        #endregion

        #region Sign验证

        public override void ProcessRequest()
        {
            try
            {
                base.ProcessRequest();
            }
            catch (JsonRpcException )
            {
                if (HttpContext.Current.IsDebuggingEnabled)
                    throw;

                //LogHelp.AddInvadeLog(ex.Message, System.Web.HttpContext.Current.Request);
                Response.Clear();
                Response.StatusCode = 404;
                Response.Status = "404 Not Found";
                Response.End();
            }
        }

        protected override IDictionary GetFeatures()
        {
            if (HttpContext.Current.IsDebuggingEnabled)
            {
                return base.GetFeatures();
            }

            string key = typeof(JsonRpcService).FullName;
            IDictionary config = (IDictionary)HttpRuntime.Cache.Get(key);

            if (config == null)
            {
                config = new Hashtable(6);

                config.Add("rpc", typeof(JsonRpcExecutive).AssemblyQualifiedName);
                config.Add("getrpc", typeof(JsonRpcGetProtocol).AssemblyQualifiedName);

                HttpRuntime.Cache.Add(key, config, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
            }
            return config;
        }

        protected override bool RequestSecurity(HttpContext context)
        { 
            //判断Sign验证
            return true;
        }

        #endregion

        #region 数据加密解密

        /// <summary>
        /// 解密输入数据
        /// </summary>
        public override System.IO.TextReader DecryptRequest(string request)
        {
            //解密
            /*var obj = Jayrock.Json.Conversion.JsonConvert.Import<JsonObject>(request);
            if (obj["params"] != null)
            {
                obj["params"] = Jayrock.Json.Conversion.JsonConvert.Import(AESHelper.Decrypt(obj["params"].ToString()));
            }
            return new StringReader(obj.ToString());*/
            //不解密
            return new StringReader(request);
        }

        /// <summary>
        /// 加密输出数据
        /// </summary>
        public override IDictionary EncryptResponse(IDictionary response)
        {
            //加密
            /*if (response["result"] != null)
            {
                response["result"] = AESHelper.Encrypt(response["result"].ToString());
            }
            return response;*/
            //不加密
            return response;
              
        }

        #endregion
    }
}