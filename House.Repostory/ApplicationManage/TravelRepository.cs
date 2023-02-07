using House.Core;
using House.IRepository.ApplicationManage;
using House.IRepository.ContractManagement;
using House.Model.ContractManagement;
using House.Model.TimeAndAttendanceManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Repository.ApplicationManage
{
    public class TravelRepository : BaseService<TravelApplication>, ITravelRepository
    {
        public TravelRepository(MyDbConText db) : base(db)
        {

        }
    }
}
