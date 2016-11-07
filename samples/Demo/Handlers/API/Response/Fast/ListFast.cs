using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Jayrock.Json;

namespace Demo.Handlers.API.Response
{
    /// <summary>
    /// Quick response container
    /// </summary>
    public class ListFast : IFastResult
    {
        /// <summary>
        /// Data
        /// </summary>
        public object List { get; set; }

        /// <summary>
        /// Business response code
        /// </summary>
        public SubCode SubCode { get; set; }

        /// <summary>
        /// Whether or not there is a next page
        /// </summary>
        public bool IsNext { get; set; }

        /// <summary>
        /// Page request current page
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Paging request page size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total record number
        /// </summary>
        public int TotalSize { get; set; }
        
        /// <summary>
        /// PageCount
        /// </summary>
        public int TotalIndex { get; set; }

        /// <summary>
        /// Object name
        /// </summary>
        public String ObjectName { get; set; }

        /// <summary>
        /// Business tips information
        /// </summary>
        public String SubMsg { get; set; }

        #region IFastResult member

        public ActionResult GetActionResult()
        {
            var lar = new ListActionResult
            {
                List = List
            };
            lar.Error.SubCode = SubCode;
            lar.Error.SubMsg = SubMsg;
            lar.Property.IsNext = IsNext;
            lar.Property.TotalSize = TotalSize;
            lar.Property.TotalIndex = TotalIndex;
            lar.Property.PageIndex = PageIndex;
            lar.Property.PageSize = PageSize;
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
                if (this.List is Exception)//If it is abnormal
                {
                    this.List = new JsonArray();
                }
            }
            else//Formal environmental use of packaging anomalies
            {
                if (this.List is Exception)//If it is abnormal
                {
                    this.List = new JsonArray();
                    lar.Error.SubMsg = "Network anomaly!";
                }
            }

            //Load JSON object
            lar.Put("List", this.List);

            JsonObject errorObject = new JsonObject();
            errorObject.Put("SubCode", (lar.Error.SubCode == null ? (int)SubCode.Failing : (int)lar.Error.SubCode));
            errorObject.Put("SubMsg", (string.IsNullOrEmpty(lar.Error.SubMsg) ? "" : lar.Error.SubMsg));
            lar.Put("Error", errorObject);

            JsonObject propertyObject = new JsonObject();
            propertyObject.Put("IsList", lar.Property.IsList);
            propertyObject.Put("IsNext", lar.Property.IsNext);
            propertyObject.Put("ObjectName", (string.IsNullOrEmpty(lar.Property.ObjectName) ? "" : lar.Property.ObjectName));
            propertyObject.Put("PageIndex", lar.Property.PageIndex);
            propertyObject.Put("PageSize", lar.Property.PageSize);
            propertyObject.Put("TotalIndex", lar.Property.TotalIndex);
            propertyObject.Put("TotalSize", lar.Property.TotalSize);
            lar.Put("Property", propertyObject);
            return lar;
        }

        #endregion
    }
}
