using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Model.ContractManagement
{
    /// <summary>
    /// 合同信息表
    /// </summary>
    public class ContractInfo : EntityBase
    {
       
        public string ContractId { get; set; }
        public string ContractNum { get; set; }
        public string ContractName { get; set; }
        public string ConstructionUnit { get; set; }
        public string ProjectLeader { get; set; }
        public string Phone { get; set; }
        public string FirstParty { get; set; }
        public string FirstPhone { get; set; }
        public string ProjectlLocation { get; set; }
        public decimal OriginalAmount { get; set; }
        public decimal ActualAmount { get; set; }
        public DateTime SigningDate { get; set; }
        public string CustomerId { get; set; }

    }
}
