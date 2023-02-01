using SqlSugar;
using System;

namespace House.Model
{
    /// <summary>
    /// 人员
    /// </summary>
    public class Personnel : EntityBase
    {
        /// <summary>
        /// 人力资源Id
        /// </summary>
        public string HumanId { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public int ClassId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string OnlineState { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string HandIcon { get; set; }
    }
}