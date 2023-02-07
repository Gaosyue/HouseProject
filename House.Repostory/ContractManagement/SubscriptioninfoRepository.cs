using House.Core;
using House.IRepository.ContractManagement;
using House.Model.ContractManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Repository.ContractManagement
{
    public class SubscriptioninfoRepository:BaseService<Subscriptioninfo>,ISubscriptionInfoRepository
    {
        public SubscriptioninfoRepository(MyDbConText db):base(db)
        {

        }
    }
}
