using House.Core;
using House.IRepository.CustomerManagement;
using House.Model.CustomerManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Repository.CustomerManagement
{
    public class PersonchargeRepository:BaseService<Personcharge>,IPersonchargeRepository
    {
        public PersonchargeRepository(MyDbConText db):base(db)
        {

        }
    }
}
