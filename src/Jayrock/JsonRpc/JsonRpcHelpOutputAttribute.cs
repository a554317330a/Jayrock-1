namespace Jayrock.Json.RPC
{
    #region Imports

    using System;
    using System.Reflection;
    using Jayrock.Services;

    #endregion

    [ Serializable ]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]//Add the AllowMultiple attribute, so that it can be used for a number of ways
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
        /// Output field annotation
        /// </summary>
        /// <param name="parameter">Return field</param>
        /// <param name="type">Type</param>
        /// <param name="explanation">Explanation</param>
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
