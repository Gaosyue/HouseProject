using House.Core;
using House.IRepository.NoticeManage;
using House.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Repository.Noticement
{
    public class LoginLogRepository:BaseService<Loginlog>,ILoginLogRepository
    {
        public LoginLogRepository(MyDbConText db):base(db)
        {

        }
    }
}
