using SqlSugar;
using System.Collections.Generic;

namespace House.Dto
{
    /// <summary>
    /// 分页返回基类
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class PageModel<TResult>
    {
        /// <summary>
        /// 返回状态码
        /// </summary>
        public string Code { get; set; } = "200";

        public int DataCount { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public TResult Item { get; set; }
        public object Result { get; set; }
        public List<TResult> Data { get; set; }
    }
}