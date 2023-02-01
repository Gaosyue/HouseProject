using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Model.SystemSettings
{
    /// <summary>
    /// 字典表
    /// </summary>
    public class DictType : EntityBase
    {
        public string Name { get; set; }

        public string Desc { get; set; }

        public bool State { get; set; }
    }
}
