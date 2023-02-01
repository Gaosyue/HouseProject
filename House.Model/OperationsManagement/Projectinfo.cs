using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Model.OperationsManagement
{
    /// <summary>
    /// 项目信息表
    /// </summary>
    public class Projectinfo : EntityBase
    {
       
        public string ContractId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Overview { get; set; }
        public DateTime EntryTime { get; set; }
    }
}
