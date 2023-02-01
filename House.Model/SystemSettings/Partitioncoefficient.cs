using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Model.SystemSettings
{
    /// <summary>
    /// 奖金分配表
    /// </summary>
    public class Partitioncoefficient : EntityBase
    {
        
        public string ItemName { get; set; }
        public decimal Proportion { get; set; }
    }
}
