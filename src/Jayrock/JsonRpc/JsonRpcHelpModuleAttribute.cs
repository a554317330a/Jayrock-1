namespace Jayrock.Json.RPC
{
    #region Imports

    using System;
    using System.Reflection;
    using Jayrock.Services;

    #endregion

    [ Serializable ]
    [ AttributeUsage(AttributeTargets.All) ]
    public sealed class JsonRpcHelpModuleAttribute : Attribute, IServiceClassModifier, IMethodModifier
    {
        private string _text;

        public JsonRpcHelpModuleAttribute() {}

        public JsonRpcHelpModuleAttribute(string text)
        {
            _text = text;
        }

        public string Text
        {
            get { return Mask.NullString(_text); }
            set { _text = value; }
        }

        void IServiceClassModifier.Modify(ServiceClassBuilder builder)
        {
            builder.Module = Text;
        }

        void IMethodModifier.Modify(MethodBuilder builder)
        {
            builder.Module = Text;
        }
    }
}
