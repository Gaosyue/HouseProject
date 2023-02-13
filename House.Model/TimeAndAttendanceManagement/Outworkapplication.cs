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
        
        public string ProjectName { get; set; }
        public string Place { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal Duration { get; set; }
        public decimal Kilometers { get; set; }
        public string Applicant { get; set; }
    }
}
