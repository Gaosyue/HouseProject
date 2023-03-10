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
    public class OutWorkRepository : BaseService<OutworkApplication>, IOutWorkRepository
    {
        public OutWorkRepository(MyDbConText db) : base(db)
        {

        }
    }
}
