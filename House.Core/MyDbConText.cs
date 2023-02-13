using House.Model;
using House.Model.ContractManagement;
using House.Model.CustomerManagement;
using House.Model.OperationsManagement;
using House.Model.SystemSettings;
using House.Model.TimeAndAttendanceManagement;
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


        #region 客户信息录入
        /// <summary>
        /// 客户信息
        /// </summary>
        public virtual DbSet<Customerinfo> Customerinfo { get; set; }
        /// <summary>
        /// 负责人信息
        /// </summary>
        public virtual DbSet<Personcharge> Personcharge { get; set; }
        /// <summary>
        /// 文件信息
        /// </summary>
        public virtual DbSet<Fileinfo> Fileinfo { get; set; }
        #endregion


        #region 合同录入
        /// <summary>
        /// 合同信息
        /// </summary>
        public virtual DbSet<ContractInfo> ContractInfo { get; set; }
        /// <summary>
        /// 合同签约信息
        /// </summary>
        public virtual DbSet<Subscriptioninfo> Subscriptioninfo { get; set; }
        /// <summary>
        /// 合同费用表
        /// </summary>
        public virtual DbSet<ContractCharges> ContractCharges { get; set; }


        /// <summary>
        /// 项目立项
        /// </summary>
        public virtual DbSet<Projectinfo> Projectinfo { get; set; }

        
        #endregion


        #region 考勤管理
        /// <summary>
        /// 请假申请表
        /// </summary>
        public virtual DbSet<LeaveApplication> LeaveApplication { get; set; }
        /// <summary>
        /// 出差申请表
        /// </summary>
        public virtual DbSet<TravelApplication> TravelApplication { get; set; }
        /// <summary>
        /// 外勤申请表
        /// </summary>
        public virtual DbSet<OutworkApplication> OutworkApplication { get; set; }
        #endregion

        #region 公告
        /// <summary>
        /// 公告表
        /// </summary>
        public virtual DbSet<Notice> Notice { get; set; }
        #endregion
    }
}