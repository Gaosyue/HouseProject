using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Model.SystemSettings
{
    /// <summary>
    /// 分部表
    /// </summary>
    public class Deptmarage : EntityBase
    {
       
        public string Name { get; set; }
        public int SuperiorId { get; set; }
        public string DescContent { get; set; }
    }
}
