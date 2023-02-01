﻿using House.Model;
using House.Model.SystemSettings;
using Microsoft.EntityFrameworkCore;

namespace House.Core
{
    public class MyDbConText : DbContext
    {
        public MyDbConText(DbContextOptions<MyDbConText> options) : base(options)
        {
        }

        #region RBAC一套

        /// <summary>
        /// 人员
        /// </summary>
        public virtual DbSet<Personnel> Personnel { get; set; }

        /// <summary>
        /// 角色用户关联表
        /// </summary>
        public virtual DbSet<PersonnelRole> PersonnelRole { get; set; }

        /// <summary>
        /// 角色表
        /// </summary>
        public virtual DbSet<Role> Role { get; set; }

        /// <summary>
        /// 权限角色关联
        /// </summary>
        public virtual DbSet<RolePower> RolePower { get; set; }

        /// <summary>
        /// 权限表
        /// </summary>
        public virtual DbSet<Power> Power { get; set; }

        #endregion

        #region 设备数据

        /// <summary>
        /// 水表
        /// </summary>
        public virtual DbSet<WaterMeter> WaterMeter { get; set; }

        #endregion


        #region 公共字典

        /// <summary>
        /// 字典类
        /// </summary>
        public virtual DbSet<DictType> DictType { get; set; }


        /// <summary>
        /// 字典项
        /// </summary>
        public virtual DbSet<DictItem> DictItem { get; set; }
        #endregion
    }
}