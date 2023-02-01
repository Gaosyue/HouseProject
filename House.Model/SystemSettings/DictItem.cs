using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Model.SystemSettings
{
    /// <summary>
    /// 字典项表
    /// </summary>
    public class DictItem : EntityBase
    {
        
        //父级Id
        public int Coding { get; set; }

        public string ItemName { get; set; }

        public int OrderId { get; set; }

        public bool State { get; set; }
    }
}
