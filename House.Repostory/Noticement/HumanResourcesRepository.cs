using House.Core;
using House.IRepository;
using House.IRepository.NoticeManage;
using House.Model.SystemSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Repository.Noticement
{
    public class HumanResourcesRepository: BaseService<Humanresources>,IHumanResourcesRepository
    {
        public HumanResourcesRepository(MyDbConText db):base(db)
        {

        }
    }
}
