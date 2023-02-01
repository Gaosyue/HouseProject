using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Model.TimeAndAttendanceManagement
{
    /// <summary>
    /// 节假日申请表
    /// </summary>
    public class Holidaymarage : EntityBase
    {
  
        public DateTime HolidayTime { get; set; }
        public string HolidayType { get; set; }

    }
}
