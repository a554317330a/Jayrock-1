namespace Jayrock.Json.RPC
{
    #region Imports

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    
    #endregion

    /// <summary>
    /// Json 数据类型
    /// </summary>
    public enum JsonType
    {
        /// <summary>
        /// Double precision floating point format in JavaScript
        /// </summary>
        Number,

        /// <summary>
        /// Double quotes slash Unicode
        /// </summary>
        String,

        /// <summary>
        /// True Or False
        /// </summary>
        Boolean,

        /// <summary>
        /// Ordered sequence of values
        /// </summary>
        Array,

        /// <summary>
        /// It can be a string, a number, a real or a fake (true/false), an empty (null), an array, an object, etc.
        /// </summary>
        Value,

        /// <summary>
        /// The key of the unordered set
        /// </summary>
        Object,

        /// <summary>
        /// Empty，return null
        /// </summary>
        Null,
    }
}
