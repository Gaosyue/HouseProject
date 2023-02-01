using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Model
{
    /// <summary>
    /// 楼号表
    /// </summary>
    public class BuildingNum : EntityBase
    {
       
        public int Building { get; set; }
    }
}
