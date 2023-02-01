using SqlSugar;
using System;

namespace House.Model
{
    /// <summary>
    /// 角色用户关联表
    /// </summary>
    public class PersonnelRole : EntityBase
    {
        /// <summary>
        ///  用户Id
        /// </summary>
        public int PersonnelId { get; set; }

        /// <summary>
        ///  角色Id
        /// </summary>
        public int RoleId { get; set; }
    }
}