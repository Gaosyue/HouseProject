using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Model.CustomerManagement
{
    /// <summary>
    /// 客户信息表
    /// </summary>

    public class Customerinfo : EntityBase
    {
        
        public string Number { get; set; }
        public string CustomerName { get; set; }
        public string CompanyAddress { get; set; }
        public string Contacts { get; set; }
        public string Telephone { get; set; }
        public string BankAccount { get; set; }
        public string BankName { get; set; }
        public string EnterpriseCode { get; set; }
        public string CustomerType { get; set; }
        public string Industry { get; set; }
        public string CreditRating { get; set; }
        public string Representative { get; set; }
        public string TaxpayerNum { get; set; }
    }
}
