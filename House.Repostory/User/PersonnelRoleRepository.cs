using House.Core;
using House.IRepository;
using House.IRepository.User;
using House.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Repository.User
{
    public class PersonnelRoleRepository : BaseService<PersonnelRole>, IPersonnelRoleRepository
    {
        public PersonnelRoleRepository(MyDbConText db) : base(db)
        {
        }
    }
}