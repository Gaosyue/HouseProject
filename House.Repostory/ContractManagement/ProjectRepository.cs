using House.Core;
using House.IRepository.CustomerManagement;
using House.Model.OperationsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Repository.ContractManagement
{
    /// <summary>
    /// 项目信息 实现层
    /// </summary>
    public class ProjectRepository : BaseService<Projectinfo>, IProjectRepository
    {
        public ProjectRepository(MyDbConText db) : base(db)
        {
        }
    }
}
