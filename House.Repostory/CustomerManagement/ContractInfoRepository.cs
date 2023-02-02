using House.Core;
using House.IRepository;
using House.IRepostory;
using House.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Repository
{
    public class ContractInfoRepository : BaseService<ContractInfo>, IContractInfoRepository
    {
        public ContractInfoRepository(MyDbConText db) : base(db)
        {

        }
    }
}
