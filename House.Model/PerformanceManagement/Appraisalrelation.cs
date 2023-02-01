using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Model.PerformanceManagement
{
    /// <summary>
    /// 考核任务与项目关联表
    /// </summary>
    public class Appraisalrelation : EntityBase
    {
        
        public int AssessId { get; set; }
        public int ProjectID { get; set; }
    }
}
