#region License, Terms and Conditions
//
// Jayrock - JSON and JSON-RPC for Microsoft .NET Framework and Mono
// Written by Atif Aziz (www.raboof.com)
// Copyright (c) 2005 Atif Aziz. All rights reserved.
//
// This library is free software; you can redistribute it and/or modify it under
// the terms of the GNU Lesser General Public License as published by the Free
// Software Foundation; either version 3 of the License, or (at your option)
// any later version.
//
// This library is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
// FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more
// details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this library; if not, write to the Free Software Foundation, Inc.,
// 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA 
//
#endregion

namespace Jayrock.JsonRpc.Web
{
    #region Imports

    using System.Collections;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using Jayrock.Services;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Jayrock.Json.RPC.JsonRpc.Web;

    using Literal = System.Web.UI.WebControls.Literal;
    using HyperLink = System.Web.UI.WebControls.HyperLink;

    #endregion

    internal sealed class JsonRpcHelp : JsonRpcPage
    {
        public JsonRpcHelp(IService service) : 
            base(service) {}

        protected override void AddHeader()
        {
            Control header = AddDiv(Body, null);
            header.ID = "Header";

            AddGeneric(header, "h1", null, PageTitle);

            base.AddHeader();
        }

        protected override void AddContent()
        {
            Control content = AddDiv(Body, null);
            content.ID = "Content";
            
            if (ServiceClass.Description.Length > 0)
                AddPara(content, "service-help", ServiceClass.Description);

            Control para = AddPara(content, "intro", null);
            AddLiteral(para, "The following ");
            AddLink(para, "JSON-RPC", "http://www.json-rpc.org/");
            AddLiteral(para, " methods are supported (try these using the ");
            AddLink(para, JsonRpcServices.GetServiceName(Service) + " test page", Request.FilePath + "?test");
            AddLiteral(para, "):");

            Control dl = AddGeneric(content, "dl", null);
            content.Controls.Add(dl);

            Method[] methods = SortedMethods;
            ArrayList idemList = new ArrayList(methods.Length);
            
            //按模块排序
            Array.Sort(methods, new ModuleComparer());

            //获取模块及方法中文名
            Dictionary<string, Dictionary<string,string>> moduleAndMethodDictionary = new Dictionary<string, Dictionary<string,string>>();

            //按模块进行展示
            string tempModuleName = "";
            foreach (Method method in methods)
            {
                string module = string.IsNullOrEmpty(method.Module) ? "Othor" : method.Module;

                if (tempModuleName != module)//判断
                {
                    tempModuleName = module;//赋值
                    AddGeneric(dl, "h2", null, module).ID = Server.HtmlEncode(module);//加入模块标题

                    moduleAndMethodDictionary.Add(module, new Dictionary<string,string>());
                }

                AddMethod(dl, method);

                moduleAndMethodDictionary[module].Add(method.Name,method.Description);

                if (method.Idempotent)
                    idemList.Add(method);
            }
            
            if (idemList.Count > 0)
            {
                AddGeneric(content, "hr", null);
                AddPara(content, null, "The following method(s) of this service are marked as idempotent and therefore safe for use with HTTP GET:");

                Control idemMethodList = AddGeneric(content, "ul", null);
                foreach (Method method in idemList)
                    AddGeneric(idemMethodList, "li", null, method.Name);
            }

            //增加模块及方法中文名到页面
            foreach (Control tempContent in Body.Controls)
            {
                if (tempContent.ID == "Content")
                {
                    Control tempContentDl = null;
                    foreach (Control temp in tempContent.Controls)
                    {
                        if (((temp as HtmlGenericControl) != null) && ((HtmlGenericControl)temp).TagName == "dl")
                        {
                            tempContentDl = temp;
                        }
                    }

                    //增加第一个父下拉框
                    Control tempDiv = AddGeneric(null, "div", "sf-menu-div");
                    Control parentUl = AddGeneric(tempDiv, "ul", "sf-menu");
                    tempContentDl.Controls.AddAt(0, tempDiv);
                    Control parentLi = AddGeneric(parentUl, "li", "");
                    AddLink(parentLi, "模块", "#");

                    //商城-订单
                    //商城-产品
                    //商城-购物车-公共
                    Control childUl = AddGeneric(parentLi, "ul", "");
                    Dictionary<string, Control> parentAndChildKeyControl = new Dictionary<string, Control>();

                    foreach (var moduleAndMethodDictionaryKey in moduleAndMethodDictionary.Keys)
                    {
                        string[] moduleAndMethodDictionaryKeySplit = moduleAndMethodDictionaryKey.Split('-');

                        string parentAndChildKey = "";
                        Control tempChildUl = childUl;
                        for (int i = 0; i < moduleAndMethodDictionaryKeySplit.Length; i++)
                        {
                            parentAndChildKey += ((i == 0 ? "" : "-") + moduleAndMethodDictionaryKeySplit[i]);

                            if (parentAndChildKeyControl.ContainsKey(parentAndChildKey)) //存在
                            {
                                tempChildUl = parentAndChildKeyControl[parentAndChildKey];
                            }
                            else//不存在
                            {
                                Control childLi = AddGeneric(tempChildUl, "li", "");
                                AddLink(childLi, "[M]" + moduleAndMethodDictionaryKeySplit[i], "#" + parentAndChildKey);
                                tempChildUl = AddGeneric(childLi, "ul", "");
                                parentAndChildKeyControl.Add(parentAndChildKey, tempChildUl);
                            }
                        }

                        if (moduleAndMethodDictionary.ContainsKey(parentAndChildKey) && parentAndChildKeyControl.ContainsKey(parentAndChildKey))
                        {
                            foreach (var moduleAndMethod in moduleAndMethodDictionary[parentAndChildKey])
                            {
                                Control childLi = AddGeneric(parentAndChildKeyControl[parentAndChildKey], "li", "");
                                AddLink(childLi, "[I]" + moduleAndMethod.Value, "#" + moduleAndMethod.Key);
                            }
                        }
                    }
                }
            }

            //加入下拉搜索资源
            AddStyleBlockFromResource("Jayrock.script.jquery.menu.superfish.css");
            AddScriptBlockFromResource("Jayrock.script.jquery.menu.jquery-1.4.2.min.js");
            AddScriptBlockFromResource("Jayrock.script.jquery.menu.hoverIntent.js");
            AddScriptBlockFromResource("Jayrock.script.jquery.menu.superfish.js");
            AddScriptBlock(@"
            $(document).ready(function(){
	            $('ul.sf-menu').superfish();
            });
            ");

            //加载回到首页
            AddScriptBlockFromResource("Jayrock.script.jquery.top.scrolltopcontrol.js");

            //加入请求和返回值参数帮助列表资源
            AddStyleBlockFromResource("Jayrock.script.helptable.helptable.css");
            base.AddContent ();
        }

        private static void AddMethod(Control parent, Method method)
        {
            Control methodTerm = AddGeneric(parent, "dt", "method");
            methodTerm.ID = method.Name;
            AddSpan(methodTerm, "method-name",null, string.Format("<a href=\"?test#{0}\">{0}</a>",method.Name));
            AddSignature(methodTerm, method);

            if (method.Description.Length > 0)
                AddGeneric(parent, "dd", "method-summary", null, "<div style=\"font-weight: bold;\">Description:</div><div style=\"margin-left: 20px;\">" + method.Description + "</div>");
            if (method.InputDescription.Length > 0)
                AddGeneric(parent, "dd", "method-summary", null, "<div style=\"font-weight: bold;\">Input:</div><div style=\"margin-left: 20px;\">" + JsonRpcBuildHtml.BuildHelpInput(method.InputDescription) + "</div>");
            if (method.OutputDescription.Length > 0)
                AddGeneric(parent, "dd", "method-summary", null, "<div style=\"font-weight: bold;\">Output:</div><div style=\"margin-left: 20px;\">" + JsonRpcBuildHtml.BuildHelpOutput(method.OutputDescription) + "</div>");
        }

        private static void AddSignature(Control parent, Method method)
        {
            Control methodSignatureSpan = AddSpan(parent, "method-sig", null);
            AddSpan(methodSignatureSpan, "method-param-open", "(");
    
            Parameter[] parameters = method.GetParameters();
            foreach (Parameter parameter in parameters)
            {
                if (parameter.Position > 0)
                    AddSpan(methodSignatureSpan, "method-param-delim", ", ");

                AddSpan(methodSignatureSpan, "method-param", parameter.Name);
            }
    
            AddSpan(methodSignatureSpan, "method-param-close", ")");
        }

        private static Control AddGeneric(Control parent, string tagName, string className)
        {
            return AddGeneric(parent, tagName, className, null);
        }

        private static Control AddGeneric(Control parent, string tagName, string className, string innerText,string innerHtml=null)
        {
            HtmlGenericControl control = new HtmlGenericControl(tagName);
            
            if (Mask.NullString(className).Length > 0) 
                control.Attributes["class"] = className;
            
            if (Mask.NullString(innerText).Length > 0) 
                control.InnerText = innerText;

            if (Mask.NullString(innerHtml).Length > 0)
                control.InnerHtml = innerHtml;

            if (parent != null)
                parent.Controls.Add(control);
            return control;
        }

        private static Control AddPara(Control parent, string className, string innerText)
        {
            return AddGeneric(parent, "p", className, innerText);
        }

        private static Control AddSpan(Control parent, string className, string innerText,string innerHtml=null)
        {
            return AddGeneric(parent, "span", className, innerText,innerHtml);
        }
    
        private static Control AddDiv(Control parent, string className)
        {
            return AddGeneric(parent, "div", className);
        }

        private Literal AddLiteral(Control parent, string text)
        {
            Literal literal = new Literal();
            literal.Text = Server.HtmlEncode(text);
            parent.Controls.Add(literal);
            return literal;
        }

        private HyperLink AddLink(Control parent, string text, string url)
        {
            HyperLink link = new HyperLink();
            link.Text = Server.HtmlEncode(text);
            link.NavigateUrl = url;
            parent.Controls.Add(link);
            return link;
        }

        protected override void AddStyleSheet()
        {
            base.AddStyleSheet();

            HtmlGenericControl style = (HtmlGenericControl) AddGeneric(Head, "style", null, @"
                @media screen {
                    body { 
                        margin: 0; 
                        font-family: arial;
                        font-size: small;
                    }

                    h1 { 
                        color: #FFF; 
                        font-size: large; 
                        padding: 0.5em;
                        background-color: #003366; 
                        margin-top: 0;
                    }

                    #Content {
                        margin: 1em;
                    }

                    dt {
                        margin-top: 0.5em;
                    }

                    dd {
                        margin-left: 2.5em;
                    }

                    .method {
                        font-size: small;
                        font-family: Monospace;
                    }

                    .method-name {
                        font-weight: bold;
                        color: #000080;
                    }

                    .method-param {
                        color: #404040;
                    }

                    .obsolete-message {
                        color: #FF0000;
                    }
                }");

            style.Attributes["type"] = "text/css";
        }

        private void AddStyleBlockFromResource(string resourceName)
        {
            using (Stream stream = GetType().Assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                AddStyleBlock(reader.ReadToEnd());
        }

        private void AddScriptBlockFromResource(string resourceName)
        {
            using (Stream stream = GetType().Assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                AddScriptBlock(reader.ReadToEnd());
        }

        private void AddScriptBlock(string script)
        {
            Head.Controls.Add(new LiteralControl("<script type='text/javascript'>" + script + "</script>"));
        }

        private void AddStyleBlock(string style)
        {
            Head.Controls.Add(new LiteralControl("<style>" + style + "</style>"));
        }
    }
}
