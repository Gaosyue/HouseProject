using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Model.DeviceManagement
{
    /// <summary>
    /// 单元表
    /// </summary>
    public class Unit : EntityBase
    {
        public int Building { get; set; }
        public int UnitNum { get; set; }
    }
}
