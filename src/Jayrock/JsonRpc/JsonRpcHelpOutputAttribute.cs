namespace Jayrock.Json.RPC
{
    #region Imports

    using System;
    using System.Reflection;
    using Jayrock.Services;

    #endregion

    [ Serializable ]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]//加入AllowMultiple属性，使其能对一个方法多次使用
    public sealed class JsonRpcHelpOutputAttribute : Attribute, IServiceClassModifier, IMethodModifier
    {
        private string _text;

        public JsonRpcHelpOutputAttribute() {}

        public JsonRpcHelpOutputAttribute(string text)
        {
            _text = text;
        }

        public string Text
        {
            get { return Mask.NullString(_text); }
            set { _text = value; }
        }

        public JsonRpcHelpOutputAttribute(params string[] text)
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
        /// 输出字段注释
        /// </summary>
        /// <param name="parameter">返回值字段</param>
        /// <param name="type">类型</param>
        /// <param name="explanation">说明</param>
        public JsonRpcHelpOutputAttribute(string parameter, string explanation, JsonType type)
        {
            _text = string.Format("{0}--{1}--{2};", parameter.Trim().Replace("--", "=").Replace(";", "*"), type.ToString().ToLower(), explanation.Trim().Replace("--", "=").Replace(";", "*"));
        }

        void IServiceClassModifier.Modify(ServiceClassBuilder builder)
        {
            builder.OutputDescription += Text;
        }

        void IMethodModifier.Modify(MethodBuilder builder)
        {
            builder.OutputDescription += Text;
        }
    }
}
