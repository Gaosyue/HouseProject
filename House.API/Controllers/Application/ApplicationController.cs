﻿using House.IRepository.ApplicationManage;
using House.IRepository.ContractManagement;
using House.Model.CustomerManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using House.Model.TimeAndAttendanceManagement;

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





    }
}