using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Dto
{
    public class LeaveDto
    {
        public int Id { get; set; }
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
        /// 申请人Id
        /// </summary>
        public string Applicant { get; set; }

        /// <summary>
        /// 申请人姓名 
        /// </summary>
        public string Name { get; set; } 
    }
}
