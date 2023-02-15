using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Model.TimeAndAttendanceManagement
{
    /// <summary>
    /// 外勤申请表
    /// </summary>
    public class OutworkApplication : EntityBase
    {

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 地点
        /// </summary>
        public string Place { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 加班时长
        /// </summary>
        public decimal Duration { get; set; }


        /// <summary>
        /// 路程
        /// </summary>
        public decimal Kilometers { get; set; }



        /// <summary>
        /// 员工Id
        /// </summary>
        public string Applicant { get; set; }
    }
}
