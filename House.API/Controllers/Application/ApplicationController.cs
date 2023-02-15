using House.IRepository.ApplicationManage;
using House.IRepository.ContractManagement;
using House.Model.CustomerManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using House.Model.TimeAndAttendanceManagement;
using House.Dto;
using LinqKit;
using House.IRepository.User;
using House.Model;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using MathNet.Numerics.Statistics;

namespace House.API.Controllers.Application
{
    /// <summary>
    /// 考勤申请
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Application")]
    public class ApplicationController : ControllerBase
    {
        //接口层实例化
        //LeaveRepository   ILeaveRepository    TravelRepository   ITravelRepository      OutWorkRepository   IOutWorkRepository

        private readonly ILeaveRepository _leaveRepository;
        private readonly ITravelRepository _travelRepository;
        private readonly IOutWorkRepository _outWorkRepository;
        private readonly IPersonnelRepository _personnelRepository;

        /// <summary>
        /// 接口注入控制器
        /// </summary>
        /// <param name="leaveRepository"></param>
        /// <param name="travelRepository"></param>
        /// <param name="outWorkRepository"></param>
        /// <param name="personnelRepository"></param>
        public ApplicationController(IPersonnelRepository personnelRepository, ILeaveRepository leaveRepository, ITravelRepository travelRepository, IOutWorkRepository outWorkRepository)
        {
            _leaveRepository= leaveRepository;
            _travelRepository= travelRepository;
            _outWorkRepository= outWorkRepository;
            _personnelRepository = personnelRepository;
        }
        

        #region 新增申请

        /// <summary>
        /// 新增请假
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> LeaveSave(LeaveApplication model)
        {
            if (model.Id==0)
            {
                var res = await _leaveRepository.InsertAsync(model);
                return res;
            }
            else
            {
                return await _leaveRepository.UpdateAsync(model);
            }
        }


        /// <summary>
        /// 新增出差
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> TravelSave(TravelApplication model)
        {
            if (model.Id==0)
            {
                var res = await _travelRepository.InsertAsync(model);
                return res;
            }
            else
            {
                return await _travelRepository.UpdateAsync(model);
            }
        }


        /// <summary>
        /// 新增外勤
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> OutWorkSave(OutworkApplication model)
        {
            if (model.Id==0)
            {
                var res = await _outWorkRepository.InsertAsync(model);
                return res;
            }
            else
            {
                return await _outWorkRepository.UpdateAsync(model);
            }
        }

        #endregion


        #region 根据员工查询申请

        /// <summary>
        /// 查询请假信息(单)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageModel<LeaveDto>> GetLeaveApplication(string applicant)
        {
            if (!string.IsNullOrWhiteSpace(applicant))
            {
                var predic = PredicateBuilder.New<LeaveApplication>(true);
                predic.And(m => m.Applicant == applicant);
                var cust = await _leaveRepository.GetAllListAsync(predic);

                var pred = PredicateBuilder.New<Personnel>(true);
                predic.And(m => m.Applicant == applicant);
                List<Personnel> user =await _personnelRepository.GetAllListAsync(pred);

                //var list = cust.Join(user,)
                var list = cust.Join(user, a => a.Applicant, b => b.Id.ToString(), (a, b) => new LeaveDto
                {
                    Id = a.Id,
                    Reason = a.Reason,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    Statistics = a.Statistics,
                    Remarks = a.Remarks,
                    Applicant = b.Id.ToString(),
                    Name = b.Name
                }).ToList();

                return new PageModel<LeaveDto> { Data = list };
            }
            else
            {
                return new PageModel<LeaveDto> { Data = null };
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
                var cust = await _travelRepository.GetAllListAsync(predic);
                return new PageModel<TravelApplication> { Data = cust };
            }
            else
            {
                return new PageModel<TravelApplication> { Data = null };
            }
        }


        /// <summary>
        /// 查询外勤申请表信息(单)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageModel<OutworkApplication>> GetOutworkApplication(string applicant)
        {
            if (!string.IsNullOrWhiteSpace(applicant))
            {
                var predic = PredicateBuilder.New<OutworkApplication>(true);
                predic.And(m => m.Applicant == applicant);

                var cust = await _outWorkRepository.GetAllListAsync(predic);

                return new PageModel<OutworkApplication> { Data = cust };
            }
            else
            {
                return new PageModel<OutworkApplication> { Data = null };
            }
        }
        #endregion


        #region 删除申请

        /// <summary>
        /// 删除请假申请
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> DelLeave(int id)
        {
            var predic = PredicateBuilder.New<LeaveApplication>(true);
            predic.And(m => m.Id == id);
            var cust = await _leaveRepository.DeleteAsync(predic);
            return true;
        }


        /// <summary>
        /// 删除出差申请
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> DelTravel(int id)
        {
            var predic = PredicateBuilder.New<TravelApplication>(true);
            predic.And(m => m.Id == id);
            var cust = await _travelRepository.DeleteAsync(predic);
            return true;
        }


        /// <summary>
        /// 删除外勤申请
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> DelOutwork(int id)
        {
            var predic = PredicateBuilder.New<OutworkApplication>(true);
            predic.And(m => m.Id == id);
            var cust = await _outWorkRepository.DeleteAsync(predic);
            return true;
        }

        #endregion 


        #region 根据主键Id查询申请

        /// <summary>
        /// 查询请假信息(单)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageModel<LeaveApplication>> GetLeaveModel(int id)
        {
            var predic = PredicateBuilder.New<LeaveApplication>(true);
            predic.And(m => m.Id == id);
            var cust = await _leaveRepository.FirstOrDefaultAsync(predic);
            return new PageModel<LeaveApplication> { Item = cust };
        }


        /// <summary>
        /// 查询出差申请表信息(单)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageModel<TravelApplication>> GetTravelModel(int id)
        {
            var predic = PredicateBuilder.New<TravelApplication>(true);
            predic.And(m => m.Id == id);
            var cust = await _travelRepository.FirstOrDefaultAsync(predic);
            return new PageModel<TravelApplication> { Item = cust };
        }


        /// <summary>
        /// 查询外勤申请表信息(单)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageModel<OutworkApplication>> GetOutworkModel(int id)
        {

            var predic = PredicateBuilder.New<OutworkApplication>(true);
            predic.And(m => m.Id == id);

            var cust = await _outWorkRepository.FirstOrDefaultAsync(predic);

            return new PageModel<OutworkApplication> { Item = cust };

        }
        #endregion


        #region 修改申请
        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<bool> UpdLeave(LeaveApplication model)
        {
            return await _leaveRepository.UpdateAsync(model);
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<bool> UpdTravel(TravelApplication model)
        {
            return await _travelRepository.UpdateAsync(model);
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<bool> UpdOutwork(OutworkApplication model)
        {
            return await _outWorkRepository.UpdateAsync(model);
        }

        #endregion
    }
}
