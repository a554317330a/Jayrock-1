namespace Jayrock.Json.RPC
{
    #region Imports

    using System;
    using System.Reflection;
    using Jayrock.Services;

    #endregion

    [ Serializable ]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]//加入AllowMultiple属性，使其能对一个方法多次使用
    public sealed class JsonRpcHelpInputAttribute : Attribute, IServiceClassModifier, IMethodModifier
    {
        private string _text;

        public JsonRpcHelpInputAttribute() {}

        public JsonRpcHelpInputAttribute(string text)
        {
            _text = text;
        }

        public string Text
        {
            get { return Mask.NullString(_text); }
            set { _text = value; }
        }

        public JsonRpcHelpInputAttribute(params string[] text)
        {
            for (int i = 1; i < text.Length + 1; i++)
            {
                if (i % 2 != 0)
                {
                    _text += text[i - 1].Trim().Replace("--", "=").Replace(";", "*");
                }
                else
                {
                    _text += "--" + text[i - 1].Trim().Replace("--", "=").Replace(";", "*") + ";";
                }
            }
        }

        /// <summary>
        /// 输入参数注释
        /// </summary>
        /// <param name="parameter">请求参数</param>
        /// <param name="type">类型</param>
        /// <param name="required">必选</param>
        /// <param name="defaults">默认值</param>
        /// <param name="explanation">说明</param>
        /// <param name="testValue">测试值，专供测试页面调用</param>
        public JsonRpcHelpInputAttribute(string parameter, string explanation, JsonType type, bool required, string defaults,string testValue)
        {
            _text = string.Format("{0}--{1}--{2}--{3}--{4}--{5};", parameter.Trim().Replace("--", "=").Replace(";", "*"), type.ToString().ToLower(), required.ToString().ToLower(), defaults.Trim().Replace("--", "=").Replace(";", "*"), explanation.Trim().Replace("--", "=").Replace(";", "*"), testValue.Trim().Replace("--", "=").Replace(";", "*"));
        }

        void IServiceClassModifier.Modify(ServiceClassBuilder builder)
        {
            builder.InputDescription += Text;
        }

        void IMethodModifier.Modify(MethodBuilder builder)
        {
            builder.InputDescription += Text;
        }
    }
}
