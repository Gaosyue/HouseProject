using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Model.SystemSettings
{
    /// <summary>
    /// 面试管理表
    /// </summary>
    public class Interviewmarage : EntityBase
    {
        
        public int DeptId { get; set; }
        public string Name { get; set; }
        public DateTime InterviewTime { get; set; }
        public bool Result { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
    }
}
