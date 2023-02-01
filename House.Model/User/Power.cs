using SqlSugar;
using System;

namespace House.Model
{
    /// <summary>
    /// 权限表
    /// </summary>
    public class Power : EntityBase
    {
        /// <summary>
        /// 部门名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 上级Id
        /// </summary>
        public int SuperiorId { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int OrderId { get; set; }
    }
}