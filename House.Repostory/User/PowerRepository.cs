using House.Core;
using House.IRepository;
using House.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Repository
{
    public class PowerRepository : BaseService<Power>, IPowerRepository
    {
        public PowerRepository(MyDbConText db) : base(db)
        {
        }
    }
}