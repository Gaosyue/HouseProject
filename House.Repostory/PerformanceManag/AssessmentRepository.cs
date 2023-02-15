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
    public class AssessmentRepository:BaseService<Assessment>, IAssessmentRepository
    {
        public AssessmentRepository(MyDbConText db):base(db)
        {

        }
    }
}
