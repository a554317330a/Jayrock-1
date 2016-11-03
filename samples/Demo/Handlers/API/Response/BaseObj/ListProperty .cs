using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo.Handlers.API.Response
{
    public class ListProperty : Property
    {
        /// <summary>
        /// 是否列表
        /// </summary>
        public override bool IsList { get { return true; } set{} }

        /// <summary>
        /// 分页请求当前页
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 分页请求页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 是否有下一页
        /// </summary>
        public bool IsNext { get; set; }

        /// <summary>
        /// 记录数
        /// </summary>
        public int TotalSize { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalIndex { get; set; }
    }
}
