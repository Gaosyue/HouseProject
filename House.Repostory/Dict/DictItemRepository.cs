using House.Core;
using House.IRepository.DeviceManagement;
using House.IRepository.Dict;
using House.Model.DeviceManagement;
using House.Model.SystemSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Repository.Dict
{
    public class DictItemRepository : BaseService<DictItem>, IDictItemRepository
    {
        public DictItemRepository(MyDbConText db) : base(db)
        {

        }
    }
}
