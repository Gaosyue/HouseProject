using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Model.DeviceManagement
{
    /// <summary>
    /// 防火墙表
    /// </summary>
    public class Firedoor : EntityBase
    {
        
        public int Building { get; set; }
        public int UnitNum { get; set; }
        public int FloorNum { get; set; }
        public bool DoorStatus { get; set; }
    }
}
