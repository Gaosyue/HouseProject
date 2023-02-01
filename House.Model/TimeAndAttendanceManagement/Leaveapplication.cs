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
    public class Leaveapplication : EntityBase
    {
        
        public string MyProperty { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal Statistics { get; set; }
        public string Remarks { get; set; }
        public string Applicant { get; set; }
    }
}
