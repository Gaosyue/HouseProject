using House.IRepository.ApplicationManage;
using House.IRepository.ContractManagement;
using House.Model.CustomerManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using House.Model.TimeAndAttendanceManagement;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace House.API.Controllers.Application
{
    /// <summary>
    /// 考勤申请
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Application")]
    public class ApplicationController : ControllerBase
    {
        //接口层实例化
        //LeaveRepository   ILeaveRepository    TravelRepository   ITravelRepository      OutWorkRepository   IOutWorkRepository

        private readonly ILeaveRepository _leaveRepository;
        private readonly ITravelRepository _travelRepository;
        private readonly IOutWorkRepository _outWorkRepository;

        /// <summary>
        /// 接口注入控制器
        /// </summary>
        /// <param name="leaveRepository"></param>
        /// <param name="travelRepository"></param>
        /// <param name="outWorkRepository"></param>
        public ApplicationController(ILeaveRepository leaveRepository, ITravelRepository travelRepository, IOutWorkRepository outWorkRepository)
        {
            _leaveRepository= leaveRepository;
            _travelRepository= travelRepository;
            _outWorkRepository= outWorkRepository;
        }


        /// <summary>
        /// 新增请假
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> LeaveAdd(LeaveApplication model)
        {
            try
            {
                var res = await _leaveRepository.InsertAsync(model);
                return res;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 新增出差
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> TravelAdd(TravelApplication model)
        {
            try
            {
                var res = await _travelRepository.InsertAsync(model);
                return res;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 新增外勤
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> OutWorkAdd(OutworkApplication model)
        {
            try
            {
                var res = await _outWorkRepository.InsertAsync(model);
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 查询请假信息(单)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageModel<LeaveApplication>> GetLeaveApplication(string applicant)
        {
            if (!string.IsNullOrWhiteSpace(applicant))
            {
                var predic = PredicateBuilder.New<LeaveApplication>(true);
                predic.And(m => m.Applicant == applicant);
                var cust = await _leaveRepository.FirstOrDefaultAsync(predic);
                return new PageModel<LeaveApplication> { Data = cust };
            }
            else
            {
                return new PageModel<LeaveApplication> { Data = null };
            }
        }


        /// <summary>
        /// 查询出差申请表信息(单)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageModel<TravelApplication>> GetTravelApplication(string applicant)
        {
            if (!string.IsNullOrWhiteSpace(applicant))
            {
                var predic = PredicateBuilder.New<TravelApplication>(true);
                predic.And(m => m.Applicant == applicant);
                var cust = await _travelRepository.FirstOrDefaultAsync(predic);
                return new PageModel<TravelApplication> { Data = cust };
            }
            else
            {
                return new PageModel<TravelApplication> { Data = null };
            }
        }

        ///// <summary>
        ///// 查询请假信息(单)
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //public async Task<PageModel<LeaveApplication>> GetLeaveApplication(string reason)
        //{
        //    if (!string.IsNullOrWhiteSpace(reason))
        //    {

        //    }
        //}



    }
}
