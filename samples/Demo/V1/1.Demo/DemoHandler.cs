using System;
using System.Collections.Generic;
using System.IO;
using Demo.Handlers.API.Response;
using Jayrock.Json;
using Jayrock.Json.RPC;
using Jayrock.JsonRpc;

namespace Demo.API.API.V3
{
    public partial class DemoHandler:BaseHandler
    {
        #region List example
        //Module name
        [JsonRpcHelpModule("Example")]
        //Interface description
        [JsonRpcHelp("User list")]
        //Interface name, must be a module + method
        [JsonRpcMethod("Demo.GetUserList", Idempotent = false)]
        //Input parameter annotation
        [JsonRpcHelpInput("input1", "Input parameter 1", JsonType.Number, false, "", "")]//Input parameter annotation
        [JsonRpcHelpInput("input2", "Input parameter 2", JsonType.Object, false, "", "{\"a\":\"aa\"}")]//Input parameter annotation
        [JsonRpcHelpInput("input3", "Input parameter 3", JsonType.Array, false, "", "[{\"a\":\"aa\"},{\"ab\":\"baa\"}]")]//Input parameter annotation
        //Output parameter annotation
        [JsonRpcHelpOutput("UserName", "User name", JsonType.String)]//Output parameter annotation
        [JsonRpcHelpOutput("NickName", "Nick name", JsonType.String)]//Output parameter annotation
        [JsonRpcHelpOutput("UserId", "User ID", JsonType.Number)]//Output parameter annotation
        public JsonObject Demo_GetUserList(int input1,JsonObject input2, JsonArray input3, int pageIndex,int pageSize)
        {
            var lf = new ListFast();//List is fixed to return to this object
            {
                try
                {
                    int totalSize = 0;//Total record number
                    int totalIndex = 0;//PageCount
                    bool isNext = false;//Whether or not there is a next page
                    pageIndex = (pageIndex == 0 ? 1 : pageIndex);//The first few pages
                    pageSize = (pageSize == 0 ? 15 : pageSize);//How many pages of a page
                    JsonArray outArray = new JsonArray();//Output JSON object

                    //Business logic
                    /*BLL.Members.Users bllUser = new BLL.Members.Users();
                    //Get the total number of bars
                    totalSize = bllUser.GetRecordCount("UserType='UU'");
                    if (totalSize > 0) //When the total data is judged
                    {
                        //Gets the total number of pages
                        totalIndex = ((totalSize/pageSize) + ((totalSize%pageSize) == 0 ? 0 : 1));
                        //To determine if there is a next page
                        isNext = (totalSize > (pageIndex * pageSize));

                        if (pageIndex <= totalIndex) //To determine the current page within the total number of pages in the range
                        {
                            //Get started
                            int startIndex = Convert.ToInt32((pageIndex > 1) ? (((pageIndex - 1)*pageSize) + 1) : 0);
                            //Get the end of the bar
                            int endIndex = Convert.ToInt32((pageIndex > 1) ? ((startIndex + pageSize) - 1) : pageSize);

                            List<Model.Members.Users> listUser = bllUser.GetPageList("UU", "", startIndex, endIndex);
                            foreach (var user in listUser)
                            {
                                JsonObject obj = new JsonObject();
                                obj.Put("UserName", user.UserName);
                                obj.Put("NickName", user.NickName);
                                obj.Put("UserId", user.UserID);
                                outArray.Add(obj);
                            }

                            //The following properties are returned in accordance with the actual requirements, such as when a failure is not performed
                            lf.SubCode = SubCode.SuccessfulPrompt; //Note: return based on the actual situation
                            lf.SubMsg = "Obtain success"; //Note: according to the SubCode prompt information settings
                        }
                        else//Greater than the total number of pages
                        {
                            //The following properties are returned in accordance with the actual requirements, such as when a failure is not performed
                            lf.SubCode = SubCode.FailingPrompt; //Note: return based on the actual situation
                            lf.SubMsg = "No data!"; //Note: according to the SubCode prompt information settings
                        }
                    }
                    else
                    {*/
                        //The following properties are returned in accordance with the actual requirements, such as when a failure is not performed
                        lf.SubCode = SubCode.FailingPrompt; //Note: return based on the actual situation
                        lf.SubMsg = "No data!"; //Note: according to the SubCode prompt information settings
                    /*}*/

                    lf.List = outArray;
                    lf.ObjectName = "User"; //The output of the object name, attention: each interface returned to the object name to ensure that it is not consistent
                    lf.PageIndex = pageIndex;
                    lf.PageSize = pageSize;
                    lf.TotalIndex = totalIndex;
                    lf.TotalSize = totalSize;
                    lf.IsNext = isNext;
                }
                catch (Exception ex)
                {
                    //Write log
                    //Demo.Web.LogHelp.AddErrorLog(string.Format("API: [{0}] Method error! {1}", base.Request.Headers["X-JSON-RPC"], ex.Message), ex.StackTrace, base.Request);
                    //The following automatic judgment of the development environment and the formal environmental error output, the official environment will be packaging anomalies
                    lf.List = ex;
                    lf.SubMsg = string.Format("Get an exception:API: [{0}] Method error! {1}", base.Request.Headers["X-JSON-RPC"], ex.Message);//Test environment output
                    lf.SubCode = SubCode.FailingPrompt;
                }
            }
            return lf.GetActionResult();
        }
        #endregion

        #region Example of a single object
        //Module name
        [JsonRpcHelpModule("Example")]
        //Interface description
        [JsonRpcHelp("User basic information")]
        //Interface name, must be a module + method
        [JsonRpcMethod("Demo.GetUser", Idempotent = false)]
        //Input parameter annotation
        [JsonRpcHelpInput("userId", "input parameter 1", JsonType.Number, true, "", "3")]//Input parameter annotation
        //Output parameter annotation
        [JsonRpcHelpOutput("UserName", "User name", JsonType.String)]//Output parameter annotation
        [JsonRpcHelpOutput("NickName", "Nick name", JsonType.String)]//Output parameter annotation
        [JsonRpcHelpOutput("UserId", "User ID", JsonType.Number)]//Output parameter annotation
        public JsonObject Demo_GetUser(int userId)
        {
            var sf = new SingleFast();//A single object is fixed to return this object
            {
                try
                {
                    JsonObject outObject = new JsonObject();//Output JSON object

                    //Business logic
                    /*BLL.Members.Users bllUser = new BLL.Members.Users();
                    Model.Members.Users user = bllUser.GetModel(userId);
                    if (user!=null)
                    {
                        //Loading data
                        outObject.Put("UserName", user.UserName);
                        outObject.Put("NickName", user.NickName);
                        outObject.Put("UserId", user.UserID);

                        //The following properties are returned in accordance with the actual requirements, such as when a failure is not performed
                        sf.SubCode = SubCode.SuccessfulPrompt; //Note: return based on the actual situation
                        sf.SubMsg = "获取成功"; //Note: according to the SubCode prompt information settings
                    }
                    else
                    {*/
                        //The following properties are returned in accordance with the actual requirements, such as when a failure is not performed
                        sf.SubCode = SubCode.FailingPrompt; //Note: return based on the actual situation
                        sf.SubMsg = "No data!"; //Note: according to the SubCode prompt information settings
                    /*}*/

                    sf.Object = outObject;
                    sf.ObjectName = "User"; //The output of the object name, attention: each interface returned to the object name to ensure that it is not consistent
                }
                catch (Exception ex)
                {
                    //Write log
                    //Demo.Web.LogHelp.AddErrorLog(string.Format("API: [{0}] Method error! {1}", base.Request.Headers["X-JSON-RPC"], ex.Message), ex.StackTrace, base.Request);
                    //The following automatic judgment of the development environment and the formal environmental error output, the official environment will be packaging anomalies
                    sf.Object = ex;
                    sf.SubMsg = string.Format("Get an exception:API: [{0}] Method error! {1}", base.Request.Headers["X-JSON-RPC"], ex.Message);//Test environment output
                    sf.SubCode = SubCode.FailingPrompt;
                }
            }
            return sf.GetActionResult();
        }
        #endregion

        #region Operational judgment example
        //Module name
        [JsonRpcHelpModule("Example")]
        //Interface description
        [JsonRpcHelp("User login")]
        //Interface name, must be a module + method
        [JsonRpcMethod("Demo.UserLogin", Idempotent = false)]
        //Input parameter annotation
        [JsonRpcHelpInput("userName", "User name", JsonType.String, true, "", "test@shop.com")]//Input parameter annotation
        [JsonRpcHelpInput("userPassword", "Password", JsonType.String, true, "", "123456a")]//Input parameter annotation
        public JsonObject Demo_UserLogin(string userName,string userPassword)
        {
            var ar = new ActionResult();//Operation to determine the fixed return to this object
            {
                try
                {
                    if (userName == userPassword)
                    {
                        //The following properties are returned in accordance with the actual requirements, such as when a failure is not performed
                        ar.Error.SubCode = SubCode.SuccessfulPrompt; //Note: return based on the actual situation
                        ar.Error.SubMsg = "Login success"; //Note: according to the SubCode prompt information settings
                    }
                    else
                    {
                        //The following properties are returned in accordance with the actual requirements, such as when a failure is not performed
                        ar.Error.SubCode = SubCode.FailingPrompt; //Note: return based on the actual situation
                        ar.Error.SubMsg = "Login failed"; //Note: according to the SubCode prompt information settings
                    }
                }
                catch (Exception ex)
                {
                    //Write log
                    //Demo.Web.LogHelp.AddErrorLog(string.Format("API: [{0}] Method error! {1}", base.Request.Headers["X-JSON-RPC"], ex.Message), ex.StackTrace, base.Request);
                    //The following automatic judgment of the development environment and the formal environmental error output, the official environment will be packaging anomalies
                    ar.Error.SubMsg = string.Format("Get an exception:API: [{0}] Method error! {1}", base.Request.Headers["X-JSON-RPC"], ex.Message);//Test environment output
                    ar.Error.SubCode = SubCode.FailingPrompt;
                }
            }
            return ar.GetActionResult();
        }
        #endregion

        #region Picture upload example
        //Module name
        [JsonRpcHelpModule("Example")]
        //Interface description
        [JsonRpcHelp("Upload pictures")]
        //Interface name, must be a module + method
        [JsonRpcMethod("Demo.UserUploadImg", Idempotent = false)]
        //Input parameter annotation
        [JsonRpcHelpInput("userId", "User ID", JsonType.Number, true, "", "3")]//Input parameter annotation
        [JsonRpcHelpInput("imgBase64", "Convert images into Base64 strings, such as the test to go to http://tool.css-js.com/base64.html for conversion", JsonType.String, true, "", "")]//Input parameter annotation
        //Output parameter annotation
        [JsonRpcHelpOutput("ImgUrl", "Pictures saved in the server's address", JsonType.String)]//Output parameter annotation
        public JsonObject Demo_UserUploadImg(int userId,string imgBase64)
        {
            var sf = new SingleFast();//A single object is fixed to return this object
            {
                try
                {
                    JsonObject outObject = new JsonObject();//Output JSON object

                    if (userId < 0 || string.IsNullOrWhiteSpace(imgBase64))//Error of judgment parameter
                    {
                        sf.Object = outObject;
                        sf.ObjectName = "UserAvatar";
                        sf.SubCode = SubCode.FailingPrompt;
                        sf.SubMsg = "Parameter error, please re-enter!";
                        return sf.GetActionResult();
                    }
                    else
                    {
                        /*//Convert Base64string into base array
                        byte[] image = Convert.FromBase64String(imgBase64); 

                        //This step is not necessary, according to the specific business of modification
                        //Avatar save template relative path, according to the specific business of modification
                        string savePath = "/" + MvcApplication.UploadFolder + "/User/Gravatar/"; 
                        if (!Directory.Exists(Server.MapPath(savePath))) //Query whether there is a path
                        {
                            Directory.CreateDirectory(Server.MapPath(savePath)); //Create a directory
                        }
                        
                        //New avatar relative path
                        string imagePath = savePath + userId + ".jpg";
                        FileStream f = new FileStream(Server.MapPath(imagePath), FileMode.Create);
                        //Get the server to complete the absolute path
                        string path = Server.MapPath(imagePath);

                        //Write new avatar
                        f.Write(image, 0, image.Length);
                        //Close flow
                        f.Close();
                        
                        //Write operation log
                        AddUserOperationLog(userId, "User upload Avatar");

                        //Status service output object
                        outObject.Put("ImgUrl", BLL.Public.Images.Gravatar.GetGravatarByCache(path, imagePath));//The avatar is more special, you need to use the cache key, if not the business needs, can be directly output imagePath
                        */
                        //Output object
                        sf.Object = outObject;
                        sf.ObjectName = "UserAvatar";
                        sf.SubMsg = "Modify user avatar success";
                        sf.SubCode = SubCode.SuccessfulPrompt;
                        return sf.GetActionResult();
                    }
                }
                catch (Exception ex)
                {
                    //Write log
                    //Demo.Web.LogHelp.AddErrorLog(string.Format("API: [{0}] Method error! {1}", base.Request.Headers["X-JSON-RPC"], ex.Message), ex.StackTrace, base.Request);
                    //The following automatic judgment of the development environment and the formal environmental error output, the official environment will be packaging anomalies
                    sf.Object = ex;
                    sf.SubMsg = string.Format("Get an exception:API: [{0}] Method error! {1}", base.Request.Headers["X-JSON-RPC"], ex.Message);//Test environment output
                    sf.SubCode = SubCode.FailingPrompt;
                }
            }
            return sf.GetActionResult();
        }
        #endregion
    }
}

