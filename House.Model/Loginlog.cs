using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Model
{
    /// <summary>
    /// 登录日志表
    /// </summary>
    public class Loginlog : EntityBase
    {
        
        public string User { get; set; }
        public DateTime LoginTime { get; set; }
        public string PCIP { get; set; }
        public string PCName { get; set; }
        public string OS { get; set; }
        public string Browser { get; set; }
    }
}
