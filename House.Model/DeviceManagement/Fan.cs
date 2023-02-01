using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Model.DeviceManagement
{
    /// <summary>
    /// 风机设备表
    /// </summary>
    public class Fan : EntityBase
    {
        
        public int Building { get; set; }
        public int UnitNum { get; set; }
        public int FloorNum { get; set; }
        public int FanState { get; set; }
    }
}
