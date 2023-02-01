using House.Core;
using House.IRepository.DeviceManagement;
using House.Model.DeviceManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Repository.DeviceManagement
{
    public class FanRepository:BaseService<Fan>,IFanRepository
    {
        public FanRepository(MyDbConText db):base(db)
        {

        }
    }
}
