using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace House.Model
{
    /// <summary>
    /// 公共基类
    /// </summary>
    public class EntityBase 
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 默认页码
        /// </summary>
        [NotMapped]
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 每页显示记数
        /// </summary>
        [NotMapped]
        public int PageSize { get; set; } = 10;
    }
}