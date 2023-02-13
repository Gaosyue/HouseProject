using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Model.TimeAndAttendanceManagement
{
    /// <summary>
    /// 休假申请表
    /// </summary>
    public class LeaveApplication : EntityBase
    {

        /// <summary>
        /// 申请类型
        /// </summary>
        
        public string Reason { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 考勤统计
        /// </summary>
        public decimal Statistics { get; set; }

        /// <summary>
        /// 详情
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 申请人
        /// </summary>
        public string Applicant { get; set; }
    }
}
