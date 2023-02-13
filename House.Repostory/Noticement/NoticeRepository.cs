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
    public class NoticeRepository:BaseService<Notice>,INoticeRepository
    {
        public NoticeRepository(MyDbConText db):base(db)
        {

        }
    }
}
