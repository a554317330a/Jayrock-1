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
        #region 列表例子
        //模块名称
        [JsonRpcHelpModule("例子")]
        //接口说明
        [JsonRpcHelp("用户列表")]
        //接口名称，必须为模块+方法
        [JsonRpcMethod("Demo.GetUserList", Idempotent = false)]
        //输入参数注释
        [JsonRpcHelpInput("input1", "输入参数1", JsonType.Number, false, "", "")]//输入参数注释
        [JsonRpcHelpInput("input2", "输入参数2", JsonType.Object, false, "", "{\"a\":\"aa\"}")]//输入参数注释
        [JsonRpcHelpInput("input3", "输入参数3", JsonType.Array, false, "", "[{\"a\":\"aa\"},{\"ab\":\"baa\"}]")]//输入参数注释
        //输出参数注释
        [JsonRpcHelpOutput("UserName", "用户名",JsonType.String)]//输出参数注释
        [JsonRpcHelpOutput("NickName", "昵称", JsonType.String)]//输出参数注释
        [JsonRpcHelpOutput("UserId", "用户ID", JsonType.Number)]//输出参数注释
        public JsonObject Demo_GetUserList(int input1,JsonObject input2, JsonArray input3, int pageIndex,int pageSize)
        {
            var lf = new ListFast();//列表固定返回此对象
            {
                try
                {
                    int totalSize=0;//总记录数
                    int totalIndex=0;//总页数
                    bool isNext = false;//是否有下一页
                    pageIndex = (pageIndex == 0 ? 1 : pageIndex);//第几页
                    pageSize = (pageSize == 0 ? 15 : pageSize);//一页多少条
                    JsonArray outArray = new JsonArray();//输出json对象

                    //业务逻辑
                    /*BLL.Members.Users bllUser = new BLL.Members.Users();
                    //获取总条数
                    totalSize = bllUser.GetRecordCount("UserType='UU'");
                    if (totalSize > 0) //判断总数据有数据时
                    {
                        //获取总页数
                        totalIndex = ((totalSize/pageSize) + ((totalSize%pageSize) == 0 ? 0 : 1));
                        //判断是否有下一页
                        isNext = (totalSize > (pageIndex * pageSize));

                        if (pageIndex <= totalIndex) //判断当前页数在总页数范围之内
                        {
                            //获取开始条数
                            int startIndex = Convert.ToInt32((pageIndex > 1) ? (((pageIndex - 1)*pageSize) + 1) : 0);
                            //获取结束条数
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

                            //以下属性请根据需要进行实际返回，如获取失败没数据时
                            lf.SubCode = SubCode.SuccessfulPrompt; //注意：根据实际情况进行返回
                            lf.SubMsg = "获取成功"; //注意：根据SubCode进行提示信息的设置
                        }
                        else//大于总页数
                        {
                            //以下属性请根据需要进行实际返回，如获取失败没数据时
                            lf.SubCode = SubCode.FailingPrompt; //注意：根据实际情况进行返回
                            lf.SubMsg = "暂无数据!"; //注意：根据SubCode进行提示信息的设置
                        }
                    }
                    else
                    {*/
                        //以下属性请根据需要进行实际返回，如获取失败没数据时
                        lf.SubCode = SubCode.FailingPrompt; //注意：根据实际情况进行返回
                        lf.SubMsg = "暂无数据!"; //注意：根据SubCode进行提示信息的设置
                    /*}*/

                    lf.List = outArray;
                    lf.ObjectName = "User"; //输出的对象名，注意：每个接口返回的对象名确保要不一致
                    lf.PageIndex = pageIndex;
                    lf.PageSize = pageSize;
                    lf.TotalIndex = totalIndex;
                    lf.TotalSize = totalSize;
                    lf.IsNext = isNext;
                }
                catch (Exception ex)
                {
                    //写入日志
                    //Demo.Web.LogHelp.AddErrorLog(string.Format("API: [{0}] 方法发生错误! {1}", base.Request.Headers["X-JSON-RPC"], ex.Message), ex.StackTrace, base.Request);
                    //以下自动判断开发环境和正式环境错误输出，正式环境将包装异常
                    lf.List = ex;
                    lf.SubMsg = string.Format("获取发生异常:API: [{0}] 方法发生错误! {1}", base.Request.Headers["X-JSON-RPC"], ex.Message);//测试环境输出
                    lf.SubCode = SubCode.FailingPrompt;
                }
            }
            return lf.GetActionResult();
        }
        #endregion

        #region 单个对象例子
        //模块名称
        [JsonRpcHelpModule("例子")]
        //接口说明
        [JsonRpcHelp("用户基本信息")]
        //接口名称，必须为模块+方法
        [JsonRpcMethod("Demo.GetUser", Idempotent = false)]
        //输入参数注释
        [JsonRpcHelpInput("userId", "输入参数1", JsonType.Number, true, "", "3")]//输入参数注释
        //输出参数注释
        [JsonRpcHelpOutput("UserName", "用户名", JsonType.String)]//输出参数注释
        [JsonRpcHelpOutput("NickName", "昵称", JsonType.String)]//输出参数注释
        [JsonRpcHelpOutput("UserId", "用户ID", JsonType.Number)]//输出参数注释
        public JsonObject Demo_GetUser(int userId)
        {
            var sf = new SingleFast();//单个对象固定返回此对象
            {
                try
                {
                    JsonObject outObject = new JsonObject();//输出json对象

                    //业务逻辑
                    /*BLL.Members.Users bllUser = new BLL.Members.Users();
                    Model.Members.Users user = bllUser.GetModel(userId);
                    if (user!=null)
                    {
                        //装载数据
                        outObject.Put("UserName", user.UserName);
                        outObject.Put("NickName", user.NickName);
                        outObject.Put("UserId", user.UserID);

                        //以下属性请根据需要进行实际返回，如获取失败没数据时
                        sf.SubCode = SubCode.SuccessfulPrompt; //注意：根据实际情况进行返回
                        sf.SubMsg = "获取成功"; //注意：根据SubCode进行提示信息的设置
                    }
                    else
                    {*/
                        //以下属性请根据需要进行实际返回，如获取失败没数据时
                        sf.SubCode = SubCode.FailingPrompt; //注意：根据实际情况进行返回
                        sf.SubMsg = "暂无数据!"; //注意：根据SubCode进行提示信息的设置
                    /*}*/

                    sf.Object = outObject;
                    sf.ObjectName = "User"; //输出的对象名，注意：每个接口返回的对象名确保要不一致
                }
                catch (Exception ex)
                {
                    //写入日志
                    //Demo.Web.LogHelp.AddErrorLog(string.Format("API: [{0}] 方法发生错误! {1}", base.Request.Headers["X-JSON-RPC"], ex.Message), ex.StackTrace, base.Request);
                    //以下自动判断开发环境和正式环境错误输出，正式环境将包装异常
                    sf.Object = ex;
                    sf.SubMsg = string.Format("获取发生异常:API: [{0}] 方法发生错误! {1}", base.Request.Headers["X-JSON-RPC"], ex.Message);//测试环境输出
                    sf.SubCode = SubCode.FailingPrompt;
                }
            }
            return sf.GetActionResult();
        }
        #endregion

        #region 操作判断例子
        //模块名称
        [JsonRpcHelpModule("例子")]
        //接口说明
        [JsonRpcHelp("用户登录")]
        //接口名称，必须为模块+方法
        [JsonRpcMethod("Demo.UserLogin", Idempotent = false)]
        //输入参数注释
        [JsonRpcHelpInput("userName", "用户名", JsonType.String, true, "", "test@shop.com")]//输入参数注释
        [JsonRpcHelpInput("userPassword", "密码", JsonType.String, true, "", "123456a")]//输入参数注释
        public JsonObject Demo_UserLogin(string userName,string userPassword)
        {
            var ar = new ActionResult();//操作判断固定返回此对象
            {
                try
                {
                    if (userName == userPassword)
                    {
                        //以下属性请根据需要进行实际返回，如获取失败没数据时
                        ar.Error.SubCode = SubCode.SuccessfulPrompt; //注意：根据实际情况进行返回
                        ar.Error.SubMsg = "登录成功"; //注意：根据SubCode进行提示信息的设置
                    }
                    else
                    {
                        //以下属性请根据需要进行实际返回，如获取失败没数据时
                        ar.Error.SubCode = SubCode.FailingPrompt; //注意：根据实际情况进行返回
                        ar.Error.SubMsg = "登录失败"; //注意：根据SubCode进行提示信息的设置
                    }
                }
                catch (Exception ex)
                {
                    //写入日志
                    //Demo.Web.LogHelp.AddErrorLog(string.Format("API: [{0}] 方法发生错误! {1}", base.Request.Headers["X-JSON-RPC"], ex.Message), ex.StackTrace, base.Request);
                    //以下自动判断开发环境和正式环境错误输出，正式环境将包装异常
                    ar.Error.SubMsg = string.Format("获取发生异常:API: [{0}] 方法发生错误! {1}", base.Request.Headers["X-JSON-RPC"], ex.Message);//测试环境输出
                    ar.Error.SubCode = SubCode.FailingPrompt;
                }
            }
            return ar.GetActionResult();
        }
        #endregion

        #region 图片上传例子
        //模块名称
        [JsonRpcHelpModule("例子")]
        //接口说明
        [JsonRpcHelp("上传图片")]
        //接口名称，必须为模块+方法
        [JsonRpcMethod("Demo.UserUploadImg", Idempotent = false)]
        //输入参数注释
        [JsonRpcHelpInput("userId", "用户ID", JsonType.Number, true, "", "3")]//输入参数注释
        [JsonRpcHelpInput("imgBase64", "图片转换成Base64的字符串，如测试自行去http://tool.css-js.com/base64.html进行转换", JsonType.String, true, "", "")]//输入参数注释
        //输出参数注释
        [JsonRpcHelpOutput("ImgUrl", "图片保存在服务端的地址", JsonType.String)]//输出参数注释
        public JsonObject Demo_UserUploadImg(int userId,string imgBase64)
        {
            var sf = new SingleFast();//单个对象固定返回此对象
            {
                try
                {
                    JsonObject outObject = new JsonObject();//输出json对象

                    if (userId < 0 || string.IsNullOrWhiteSpace(imgBase64))//判断参数错误
                    {
                        sf.Object = outObject;
                        sf.ObjectName = "UserAvatar";
                        sf.SubCode = SubCode.FailingPrompt;
                        sf.SubMsg = "参数错误，请重新输入!";
                        return sf.GetActionResult();
                    }
                    else
                    {
                        /*//将Base64string转换成base数组
                        byte[] image = Convert.FromBase64String(imgBase64); 

                        //这步非必须，根据具体业务进行改装
                        //头像保存模板相对路径，根据具体业务进行改装
                        string savePath = "/" + MvcApplication.UploadFolder + "/User/Gravatar/"; 
                        if (!Directory.Exists(Server.MapPath(savePath))) //查询是否存在路径
                        {
                            Directory.CreateDirectory(Server.MapPath(savePath)); //创建改目录
                        }
                        
                        //新头像相对路径
                        string imagePath = savePath + userId + ".jpg";
                        FileStream f = new FileStream(Server.MapPath(imagePath), FileMode.Create);
                        //得到服务端完成绝对路径
                        string path = Server.MapPath(imagePath);

                        //写入新头像
                        f.Write(image, 0, image.Length);
                        //关闭流
                        f.Close();
                        
                        //写入操作日志
                        AddUserOperationLog(userId, "用户上传头像");

                        //状态业务输出对象
                        outObject.Put("ImgUrl", BLL.Public.Images.Gravatar.GetGravatarByCache(path, imagePath));//这里的头像比较特殊，需要用到缓存的key，如果非业务需要，可直接输出imagePath
                        */
                        //输出对象
                        sf.Object = outObject;
                        sf.ObjectName = "UserAvatar";
                        sf.SubMsg = "修改用户头像成功";
                        sf.SubCode = SubCode.SuccessfulPrompt;
                        return sf.GetActionResult();
                    }
                }
                catch (Exception ex)
                {
                    //写入日志
                    //Demo.Web.LogHelp.AddErrorLog(string.Format("API: [{0}] 方法发生错误! {1}", base.Request.Headers["X-JSON-RPC"], ex.Message), ex.StackTrace, base.Request);
                    //以下自动判断开发环境和正式环境错误输出，正式环境将包装异常
                    sf.Object = ex;
                    sf.SubMsg = string.Format("获取发生异常:API: [{0}] 方法发生错误! {1}", base.Request.Headers["X-JSON-RPC"], ex.Message);//测试环境输出
                    sf.SubCode = SubCode.FailingPrompt;
                }
            }
            return sf.GetActionResult();
        }
        #endregion
    }
}

