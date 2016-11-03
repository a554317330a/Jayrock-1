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

    using System.IO;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using Jayrock.Json;
    using Jayrock.Services;
    using System;
    using Jayrock.Json.RPC.JsonRpc.Web;

    using HyperLink = System.Web.UI.WebControls.HyperLink;

    #endregion

    internal sealed class JsonRpcTester : JsonRpcPage
    {
        public JsonRpcTester(IService service) : 
            base(service) {}

        protected override string PageTitle
        {
            get { return "Test " + base.PageTitle; }
        }

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
                AddGeneric(content, "span", "service-help", ServiceClass.Description);

            Control form = AddGeneric(content, "form");
            form.ID = "TestForm";
    
            Control selectionPara = AddPara(form, null, "Select method to test: ");
    
            HtmlSelect methodSelector = new HtmlSelect();
            methodSelector.ID = "Method";
            methodSelector.Attributes.Add("onchange", "return Method_onchange(this)");
            methodSelector.Attributes.Add("style", "width:600px;");

            Method[] methods = SortedMethods;
            Array.Sort(methods, new ModuleComparer());
            foreach (Method method in methods)
            {
                methodSelector.Items.Add(new System.Web.UI.WebControls.ListItem() { Value = method.Name, Text = string.Format("[{0}]{1}({2})", string.IsNullOrEmpty(method.Module) ? "Other" : method.Module, method.Name, method.Description) });
            }
    
            selectionPara.Controls.Add(methodSelector);

            HtmlInputButton testButton = new HtmlInputButton();
            testButton.ID = "Test";
            testButton.Value = "Test";
            testButton.Attributes["onclick"] = "return Test_onclick(this)";
            testButton.Attributes["accesskey"] = "T";
            selectionPara.Controls.Add(new LiteralControl(" "));
            selectionPara.Controls.Add(testButton);

            selectionPara.Controls.Add(new LiteralControl(" "));

            HyperLink helpLink = new HyperLink();
            helpLink.Text = "Help";
            helpLink.NavigateUrl = Request.FilePath + "?help";
            selectionPara.Controls.Add(helpLink);

            //模块
            HtmlGenericControl module = new HtmlGenericControl("p");
            module.ID = "module";
            selectionPara.Controls.Add(module);

            //方法
            HtmlGenericControl Interface = new HtmlGenericControl("p");
            Interface.ID = "interface";
            selectionPara.Controls.Add(Interface);

            //注释
            HtmlGenericControl description = new HtmlGenericControl("p");
            description.ID = "description";
            selectionPara.Controls.Add(description);

            //输入
            HtmlGenericControl input = new HtmlGenericControl("p");
            input.ID = "input";
            selectionPara.Controls.Add(input);

            Control requestPara = AddPara(form, null, "Request parameters: ");

            HtmlTextArea requestArea = new HtmlTextArea();
            requestArea.ID = "Request";
            requestArea.Rows = 10;
            requestArea.Attributes.Add("title", "Enter the array of parameters (in JSON) to send in the RPC request.");
            requestPara.Controls.Add(requestArea);

            //输出
            HtmlGenericControl output = new HtmlGenericControl("p");
            output.ID = "output";
            requestPara.Controls.Add(output);

            Control responsePara = AddPara(form, null, "Response result/error: ");

            HtmlTextArea responseArea = new HtmlTextArea();
            responseArea.ID = "Response";
            responseArea.Rows = 10;
            responseArea.Attributes.Add("readonly", "readonly");
            responseArea.Attributes.Add("title", "The result or error object (in JSON) from the last RPC response.");
            responsePara.Controls.Add(responseArea);

            Control statsPara = AddPara(content, null, null);
            statsPara.ID = "Stats";

            Control headersPre = AddGeneric(content, "pre");
            headersPre.ID = "Headers";

            AddScriptBlockFromResource("Jayrock.json.js");
            //加入下拉搜索资源
            AddStyleBlockFromResource("Jayrock.script.jquery.flexselect.flexselect.css");
            AddScriptBlockFromResource("Jayrock.script.jquery.flexselect.jquery.min.js");
            AddScriptBlockFromResource("Jayrock.script.jquery.flexselect.liquidmetal.js");
            AddScriptBlockFromResource("Jayrock.script.jquery.flexselect.jquery.flexselect.js");

            AddScriptBlock(@"
                var callTemplates = " + BuildCallTemplatesObject() + @";
                var callTemplatesDescription = " + BuildCallTemplatesObjectDescription() + @";
                var callTemplatesInput = " + BuildCallTemplatesObjectInput() + @";
                var callTemplatesOutput = " + BuildCallTemplatesObjectOutput() + @";                
                var callTemplatesModule = " + BuildCallTemplatesObjectModule() + @";
                var theForm = null;
                var nextRequestId = 0;

                Number.prototype.formatWhole = function()
                {
                    var s = this.toFixed(0).toString();
                    var groups = [];
                    while (s.length > 0)
                    {
                        groups.unshift(s.slice(-3));
                        s = s.slice(0, -3);
                    }
                    return groups.join();
                }

                window.onload = function() 
                { 
                    theForm = document.forms[0]; 
                    Method_onchange(theForm.Method); 
                }

                function Method_onchange(sender)
                {
                    theForm.Request.value = callTemplates[sender.options[sender.selectedIndex].value];
                    interface.innerHTML = 'Method:'+'" + "<div style=\"margin-left: 20px;\">" + @"'+sender.options[sender.selectedIndex].value+'" + "</div>" + @"';
                    callTemplatesDescription[sender.options[sender.selectedIndex].value] == ''?description.innerHTML = '':description.innerHTML = 'Description:'+'" + "<div style=\"margin-left: 20px;\">" + @"'+callTemplatesDescription[sender.options[sender.selectedIndex].value]+'" + "</div>" + @"';
                    callTemplatesInput[sender.options[sender.selectedIndex].value] == ''?input.innerHTML = '':input.innerHTML = 'Input:'+'" +"<div style=\"margin-left: 20px;\">"+@"'+callTemplatesInput[sender.options[sender.selectedIndex].value]+'"+"</div>"+@"';
                    callTemplatesOutput[sender.options[sender.selectedIndex].value] == ''?output.innerHTML='':output.innerHTML = 'Output:'+'" + "<div style=\"margin-left: 20px;\">" + @"'+callTemplatesOutput[sender.options[sender.selectedIndex].value]+'" + "</div>" + @"';
                    callTemplatesModule[sender.options[sender.selectedIndex].value] == ''?module.innerHTML='':module.innerHTML = 'Module:'+'" + "<div style=\"margin-left: 20px;\">" + @"'+callTemplatesModule[sender.options[sender.selectedIndex].value]+'" + "</div>" + @"';
                }

                function Test_onclick(sender)
                {
                    var stats = document.getElementById('Stats');
                    setText(stats, null);

                    var headers = document.getElementById('Headers');
                    setText(headers, null);

                    var form = theForm;

                    try
                    {
                        var request = { 
                            id : ++nextRequestId, 
                            method : form.Method.value, 
                            params : theForm.Request.value };
                        
                        form.Response.value = '';
                        form.Response.className = '';
                        var response = callSync(request);
                        setText(stats, 'Time taken = ' + (response.timeTaken / 1000).toFixed(4) + ' milliseconds; Response size = ' + response.http.text.length.formatWhole() + ' char(s)');
                        setText(headers, response.http.headers);
                        if (response.error != null) throw response.error;
                        form.Response.value = JSON.stringify(response.result);
                    }
                    catch (e)
                    {
                        form.Response.className = 'error';
                        form.Response.value = JSON.stringify(e);
                        alert(e.message);
                    }
                }

                function callSync(request)
                {
                    var http = window.XMLHttpRequest ?
                        new XMLHttpRequest() :
                        new ActiveXObject('Microsoft.XMLHTTP');
                    http.open('POST', '" + Request.FilePath + @"', false);
                    http.setRequestHeader('Content-Type', 'text/plain; charset=utf-8');
                    http.setRequestHeader('X-JSON-RPC', request.method);
                    http.send('{\""id\"":' + request.id + ',\""method\"":\""' + request.method + '\"",\""params\"":' + request.params + '}');
                    if (http.status != 200)
                        throw { message : http.status + ' ' + http.statusText, toString : function() { return this.message; } };
                    var clockStart = new Date();
                    var response = JSON.parse(http.responseText);
                    response.timeTaken = (new Date()) - clockStart;
                    response.http = { text : http.responseText, headers : http.getAllResponseHeaders() };
                    return response;
                }

                function setText(e, text)
                {
                    while (e.firstChild)
                        e.removeChild(e.firstChild);

                    if (text == null) 
                        return;

                    text = text.toString();

                    if (0 === text.length) 
                        return;

                    var textNode = document.createTextNode(text);
                    e.appendChild(textNode);
                }
                
                function getarg(url)
                {
                    var arg=url.split('#');
                    if(arg.length>1) return arg[1].toString();
                    else return '';
                }

                $(function(){
                    if(getarg(window.location.href).length>0){
                        $('#Method').find('option[value='+getarg(window.location.href)+']').attr('selected',true);
                    }
                }); 

                //下拉搜索
                $(document).ready(function() {
                    $('#Method').flexselect();
                });

                ");

            //加载回到首页
            AddScriptBlockFromResource("Jayrock.script.jquery.top.scrolltopcontrol.js");

            //加入请求和返回值参数帮助列表资源
            AddStyleBlockFromResource("Jayrock.script.helptable.helptable.css");

            base.AddContent();

            //添加json格式化工具
            using (Stream stream = GetType().Assembly.GetManifestResourceStream("Jayrock.script.jsonformat.jsontemplate.htm"))
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                form.Controls.Add(new LiteralControl(reader.ReadToEnd()));
            //添加json格式化工具
        }

        private void AddScriptBlockFromResource(string resourceName) {
            using (Stream stream = GetType().Assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                AddScriptBlock(reader.ReadToEnd());
        }

        private void AddStyleBlockFromResource(string resourceName)
        {
            using (Stream stream = GetType().Assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                AddStyleBlock(reader.ReadToEnd());
        }

        private JsonObject BuildCallTemplatesObject()
        {
            JsonObject info = new JsonObject();
            StringBuilder sb = new StringBuilder();
    
            foreach (Method method in ServiceClass.GetMethods())
            {
                sb.Length = 0;
                sb.Append("{ ");

                Parameter[] parameters = method.GetParameters();
                
                if (parameters.Length == 0)
                {
                    sb.Append("/* void */");
                }
                else
                {
                    foreach (Parameter parameter in parameters)
                    {
                        if (parameter.Position > 0) 
                            sb.Append(", ");

                        sb.Append('"').Append(parameter.Name).Append("\" : " + JsonRpcBuildHtml.GetTestValue(method.InputDescription, parameter.Name));
                    }
                }

                sb.Append(" }");
                info.Put(method.Name, sb.ToString());
            }
            return info;
        }

        private JsonObject BuildCallTemplatesObjectDescription()
        {
            JsonObject info = new JsonObject();
            StringBuilder sb = new StringBuilder();

            foreach (Method method in ServiceClass.GetMethods())
            {
                sb.Length = 0;

                sb.Append(method.Description);

                info.Put(method.Name, sb.ToString());
            }
            return info;
        }

        private JsonObject BuildCallTemplatesObjectInput()
        {
            JsonObject info = new JsonObject();
            StringBuilder sb = new StringBuilder();

            foreach (Method method in ServiceClass.GetMethods())
            {
                sb.Length = 0;

                sb.Append(JsonRpcBuildHtml.BuildHelpInput(method.InputDescription));

                info.Put(method.Name, sb.ToString());
            }
            return info;
        }

        private JsonObject BuildCallTemplatesObjectOutput()
        {
            JsonObject info = new JsonObject();
            StringBuilder sb = new StringBuilder();

            foreach (Method method in ServiceClass.GetMethods())
            {
                sb.Length = 0;

                sb.Append(JsonRpcBuildHtml.BuildHelpOutput(method.OutputDescription));

                info.Put(method.Name, sb.ToString());
            }
            return info;
        }

        private JsonObject BuildCallTemplatesObjectModule()
        {
            JsonObject info = new JsonObject();
            StringBuilder sb = new StringBuilder();

            foreach (Method method in ServiceClass.GetMethods())
            {
                sb.Length = 0;

                sb.Append(string.IsNullOrEmpty(method.Module) ? "Othor" : method.Module);

                info.Put(method.Name, sb.ToString());
            }
            return info;
        }

        private static Control AddGeneric(Control parent, string tagName)
        {
            return AddGeneric(parent, tagName, null);
        }

        private static Control AddGeneric(Control parent, string tagName, string className)
        {
            return AddGeneric(parent, tagName, className, null);
        }

        private static Control AddGeneric(Control parent, string tagName, string className, string innerText)
        {
            HtmlGenericControl control = new HtmlGenericControl(tagName);
            
            if (Mask.NullString(className).Length > 0) 
                control.Attributes["class"] = className;
            
            if (Mask.NullString(innerText).Length > 0) 
                control.InnerText = innerText;
            
            parent.Controls.Add(control);
            return control;
        }

        private static Control AddPara(Control parent, string className, string innerText)
        {
            return AddGeneric(parent, "p", className, innerText);
        }

        private static Control AddDiv(Control parent, string className)
        {
            return AddGeneric(parent, "div", className);
        }

        private void AddScriptBlock(string script)
        {
            Head.Controls.Add(new LiteralControl("<script type='text/javascript'>" + script + "</script>"));
        }
        
        private void AddStyleBlock(string style)
        {
            Head.Controls.Add(new LiteralControl("<style>" + style + "</style>"));
        }

        protected override void AddStyleSheet()
        {
            base.AddStyleSheet();

            HtmlGenericControl style = (HtmlGenericControl) AddGeneric(Head, "style", null, @"
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

                #Request, #Response {
                    display: block;
                    margin-top: 0.5em;
                    width: 90%;
                }

                #Response.error {
                    color: red;
                }

                #Headers {
                    font-family: Monospace;
                    font-size: small;
                }");

            style.Attributes["type"] = "text/css";
        }
    }
}
