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
    /// 出差申请表
    /// </summary>
    public class TravelApplication : EntityBase
    {
   
        public string ProjectName { get; set; }
        public string TravelPlace { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal Duration { get; set; }
        public string Remarks { get; set; }

        public decimal Kilometers { get; set; }
        public string Applicant { get; set; }
    }
}
