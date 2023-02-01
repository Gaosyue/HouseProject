using House.Core;
using House.IRepository;
using House.IRepository.Dict;
using House.Model.SystemSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Repository.Dict
{
    public class DictTypeRepository :BaseService<DictType>,IDictTypeRepository
    {
        public DictTypeRepository(MyDbConText db) : base(db)
        {

        }
    }
}
