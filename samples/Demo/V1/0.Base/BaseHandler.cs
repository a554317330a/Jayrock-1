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
            API_VERSION = "V1";//Set version
        }

        /// <summary>
        /// API encryption authentication
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override bool RequestSecurity(System.Web.HttpContext context)
        {
            //Sign Verification
            return true;
        }

        /// <summary>
        /// Output data encryption
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public override System.Collections.IDictionary EncryptResponse(System.Collections.IDictionary response)
        {
            //Encryption
            return base.EncryptResponse(response);
        }

        /// <summary>
        /// Input data decryption
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public override TextReader DecryptRequest(string request)
        {
            //Decrypt
            return base.DecryptRequest(request);
        }
    }
}

