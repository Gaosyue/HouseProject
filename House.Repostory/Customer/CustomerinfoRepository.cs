using House.Core;
using House.IRepository.CustomerManagement;
using House.Model.CustomerManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Repository.Customer
{
    public class CustomerinfoRepository : BaseService<Customerinfo>, ICustomerinfoRepository
    {
        public CustomerinfoRepository(MyDbConText db) : base(db)
        {

        }
    }
}