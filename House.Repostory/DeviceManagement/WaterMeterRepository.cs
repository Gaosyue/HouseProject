using House.Core;
using House.IRepository.DeviceManagement;
using House.IRepository.User;
using House.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Repository.DeviceManagement
{
    public class WaterMeterRepository : BaseService<WaterMeter>, IWaterMeterRepository
    {
        public WaterMeterRepository(MyDbConText db) : base(db)
        {
        }
    }
}