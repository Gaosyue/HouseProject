using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Model
{
    /// <summary>
    /// 操作日志表
    /// </summary>
    public class Operationlog : EntityBase
    {
        
        public string Account { get; set; }
        public DateTime OperationTime { get; set; }
        public string Execute { get; set; }
        public string ProjectNmae { get; set; }
        public string ControllerNmae { get; set; }
        public string MethodNmae { get; set; }
        public string Parameter { get; set; }

    }
}
