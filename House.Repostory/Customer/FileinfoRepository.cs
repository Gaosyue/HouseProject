using House.Core;
using House.IRepository.CustomerManagement;
using House.Model;
using House.Model.ContractManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Repository.Customer
{
    public class FileinfoRepository : BaseService<Fileinfo>, IFileinfoRepository
    {
        public FileinfoRepository(MyDbConText db) : base(db)
        {

        }
    }
}
