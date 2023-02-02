using House.Core;
using House.IRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Repository
{
    public class FileinfoRepository : BaseService<FileInfo>, IFileinfoRepository
    {
        public FileinfoRepository(MyDbConText db) : base(db)
        {

        }
    }
}
