using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Model
{
    /// <summary>
    /// 异常日志表
    /// </summary>
    public class Exceptionlog : EntityBase
    {
       
        public string Account { get; set; }
        public DateTime HappenTime { get; set; }
        public string DescribeContent { get; set; }
        public string ProjectName { get; set; }
        public string ControllerNmae { get; set; }
        public string MethodNmae { get; set; }
        public string Parameter { get; set; }
        public string Detailed { get; set; }
    }
}
