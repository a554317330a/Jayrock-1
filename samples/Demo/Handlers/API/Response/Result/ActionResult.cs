using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Jayrock.Json;

namespace Demo.Handlers.API.Response
{
    /// <summary>
    /// Response result
    /// </summary>
    public class ActionResult : JsonObject
    {
        public ActionResult()
        {
            Error = new Error();
        }

        /// <summary>
        /// Response code
        /// </summary>
        public Error Error { get; set; }

        /// <summary>
        /// Obtain response results
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

            //To determine whether the SubCode is set correctly
            if (lar.Error.SubCode != null)
            {
                if (lar.Error.SubCode == Response.SubCode.SessionfulPrompt && String.IsNullOrEmpty(lar.Error.SubMsg)) throw new Exception("SubCode for the -1, you need to set the value of ErrMsg!");
                if (lar.Error.SubCode == Response.SubCode.Successful && !String.IsNullOrEmpty(lar.Error.SubMsg)) throw new Exception("When SubCode is 0, the ErrMsg value is not allowed!");
                if (lar.Error.SubCode == Response.SubCode.SuccessfulPrompt && String.IsNullOrEmpty(lar.Error.SubMsg)) throw new Exception("When SubCode is 1, you need to set the ErrMsg value!");
                if (lar.Error.SubCode == Response.SubCode.Failing && !String.IsNullOrEmpty(lar.Error.SubMsg)) throw new Exception("When SubCode is 2, the ErrMsg value is not allowed!");
                if (lar.Error.SubCode == Response.SubCode.FailingPrompt && String.IsNullOrEmpty(lar.Error.SubMsg)) throw new Exception("SubCode is not 3, you need to set the value of ErrMsg!");
            }

            //Packing anomaly
            if (HttpContext.Current.IsDebuggingEnabled)//To determine whether the test environment
            {
                if (this.Error.SubMsg.Contains("获取发生异常") || this.Error.SubMsg.ToLower().Contains("exception"))//If it is abnormal
                {
                    
                }
            }
            else//Formal environmental use of packaging anomalies
            {
                if (this.Error.SubMsg.Contains("获取发生异常") || this.Error.SubMsg.ToLower().Contains("exception"))//If it is abnormal
                {
                    Error.SubMsg = "Network anomaly!";
                }
            }

            //Load JSON object
            JsonObject errorObject = new JsonObject();
            errorObject.Put("SubCode", (this.Error.SubCode == null ? (int) SubCode.Failing : (int) this.Error.SubCode));
            errorObject.Put("SubMsg", (string.IsNullOrEmpty(this.Error.SubMsg) ? "" : this.Error.SubMsg));
            lar.Put("Error", errorObject);

            return lar;
        }
    }
}
