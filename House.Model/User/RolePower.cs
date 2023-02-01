using SqlSugar;
using System;

namespace House.Model
{
    /// <summary>
    /// 权限角色关联
    /// </summary>
    public class RolePower : EntityBase
    {
        /// <summary>
        ///  角色Id
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        ///  角色Id
        /// </summary>
        public int PowerId { get; set; }
    }
}