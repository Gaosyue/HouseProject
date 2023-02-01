using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Model
{
    /// <summary>
    /// 楼层表
    /// </summary>
    public class Floor : EntityBase
    {
        
        public int UnitNum { get; set; }
        public int FloorNum { get; set; }
    }
}
