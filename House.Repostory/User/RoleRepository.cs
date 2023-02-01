using House.Core;
using House.IRepository;
using House.IRepository.User;
using House.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Repository
{
    public class RoleRepository : BaseService<Role>, IRoleRepository
    {
        public RoleRepository(MyDbConText db) : base(db)
        {
        }
    }
}