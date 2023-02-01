using SqlSugar;
using System;

namespace House.Model
{
    /// <summary>
    /// 角色表
    /// </summary>
    public class Role : EntityBase
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public bool State { get; set; }
    }
}