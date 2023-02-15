using House.Core;
using House.IRepository.PerformanceManagement;
using House.Model.PerformanceManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Repository.PerformanceManag
{
    public class AppraisalrelationRepository:BaseService<Appraisalrelation>,IAppraisalrelationRepository
    {
        public AppraisalrelationRepository(MyDbConText db):base(db)
        {

        }
    }
}
