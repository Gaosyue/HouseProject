using House.Dto;
using House.IRepository.PerformanceManagement;
using House.Model.PerformanceManagement;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace House.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Assessment")]
    public class AssessmentController : ControllerBase
    {
        //考核任务
        private readonly IAssessmentRepository _assessment;
        //考核项目明细
        private readonly IAssessmentItemRepository _item;
        //考核任务与项目关联表
        private readonly IAppraisalrelationRepository _appraisalrelation;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="assessment"></param>
        /// <param name="item"></param>
        /// <param name="appraisalrelation"></param>
        public AssessmentController(IAssessmentRepository assessment, IAssessmentItemRepository item, IAppraisalrelationRepository appraisalrelation)
        {
            _assessment = assessment;
            _item = item;
            _appraisalrelation = appraisalrelation;
        }


        /// <summary>
        /// 考核列表显示
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<Assessment>> GetAssessment()
        {
            try
            {
                return await _assessment.GetAllListAsync();
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 删除考核列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<Token> DelAssessment(int id)
        {
            var assess = PredicateBuilder.New<Assessment>(true);
            assess.And(x => x.Id == id);
            bool res = await _assessment.DeleteAsync(assess);
            return new Token { Code = res ? 200 : 500 };
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<bool> UpdArchiveorNot(int id)
        {
            var notice = PredicateBuilder.New<Assessment>(true);
            notice.And(p => p.Id == id);
            var data = await _assessment.FirstOrDefaultAsync(notice);
            if (data.ArchiveorNot == false)
            {
                data.ArchiveorNot = true;
                return await _assessment.UpdateAsync(data);
            }
            else
            {
                data.ArchiveorNot = false;
                return await _assessment.UpdateAsync(data);
            }
        }

        /// <summary>
        /// 添加考核列表
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> AddAssessment(Assessment a)
        {
            var res = await _assessment.InsertAsync(a);
            return res;
        }

        /// <summary>
        /// 考核回显
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Assessment> EditAssessment(int id)
        {
            var res = await _assessment.FirstOrDefaultAsync(p => p.Id == id);

            return res;
        }

        /// <summary>
        /// 修改回显信息
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<bool> UpdNotice(Assessment a)
        {
            var res = await _assessment.UpdateAsync(a);
            return res;
        }

        /// <summary>
        /// 考核统计
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<AssessmentItem>> GetAssessmentItem()
        {
            try
            {
                return await _item.GetAllListAsync();
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}
