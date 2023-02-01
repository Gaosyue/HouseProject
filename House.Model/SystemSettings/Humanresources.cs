using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Model.SystemSettings
{
    /// <summary>
    /// 人力资源表
    /// </summary>
    public class Humanresources : EntityBase
    {
        
        public string Account { get; set; }
        public string Name { get; set; }
        public string Pwd { get; set; }
        public bool State { get; set; }
        public int DeptId { get; set; }
        public string Major { get; set; }
        public string Position { get; set; }
        public string NumberID { get; set; }
        public string Phone { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
        public string Residence { get; set; }
        public DateTime DateofBirth { get; set; }
        public string ManagePost { get; set; }

    }
}
