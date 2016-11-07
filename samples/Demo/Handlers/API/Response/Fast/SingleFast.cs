using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Jayrock.Json;

namespace Demo.Handlers.API.Response
{
    /// <summary>
    /// Quick response container for single record
    /// </summary>
    public class SingleFast : IFastResult
    {
        /// <summary>
        /// Data
        /// </summary>
        public object Object { get; set; }

        /// <summary>
        /// Business response code
        /// </summary>
        public SubCode SubCode { get; set; }

        /// <summary>
        /// Prompt information
        /// </summary>
        public String SubMsg { get; set; }
        
        /// <summary>
        /// Object name
        /// </summary>
        public String ObjectName { get; set; }

        #region IFastResult member

        public ActionResult GetActionResult()
        {
            var lar = new SingleActionResult
            {
                Object = Object
            };
            lar.Error.SubCode = SubCode;
            lar.Error.SubMsg = SubMsg;
            lar.Property.ObjectName = ObjectName;


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
                if (this.Object is Exception)//If it is abnormal
                {
                    this.Object = new JsonObject();
                }
            }
            else//Formal environmental use of packaging anomalies
            {
                if (this.Object is Exception)//If it is abnormal
                {
                    this.Object = new JsonObject();
                    lar.Error.SubMsg = "Network anomaly!";
                }
            }

            //Load JSON object
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
