namespace Jayrock.Json.RPC
{
    #region Imports

    using System;
    using System.Reflection;
    using Jayrock.Services;

    #endregion

    [ Serializable ]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]//����AllowMultiple���ԣ�ʹ���ܶ�һ���������ʹ��
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
        /// �������ע��
        /// </summary>
        /// <param name="parameter">�������</param>
        /// <param name="type">����</param>
        /// <param name="required">��ѡ</param>
        /// <param name="defaults">Ĭ��ֵ</param>
        /// <param name="explanation">˵��</param>
        /// <param name="testValue">����ֵ��ר������ҳ�����</param>
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
