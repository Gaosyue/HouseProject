using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Dto
{
    public class CustomerInfoDto
    {
        /// <summary>
        /// 客户编号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 公司地址
        /// </summary>
        public string CompanyAddress { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// 公司电话
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 开户银行账号
        /// </summary>
        public string BankAccount { get; set; }
    }
}
