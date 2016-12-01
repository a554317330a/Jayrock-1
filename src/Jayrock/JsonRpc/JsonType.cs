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
        /// 在JavaScript中的双精度浮点格式
        /// </summary>
        Number,

        /// <summary>
        /// 双引号的反斜杠转义的Unicode
        /// </summary>
        String,

        /// <summary>
        /// true 或 false
        /// </summary>
        Boolean,

        /// <summary>
        /// 值的有序序列
        /// </summary>
        Array,

        /// <summary>
        /// 它可以是一个字符串，一个数字，真的还是假（true/false），空(null )，数组，对象等
        /// </summary>
        Value,

        /// <summary>
        /// 无序集合键值对
        /// </summary>
        Object,

        /// <summary>
        /// empty，返回null
        /// </summary>
        Null,
    }
}
