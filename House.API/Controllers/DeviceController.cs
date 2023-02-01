using House.Dto;
using House.IRepository.DeviceManagement;
using House.IRepository.User;
using House.Model;
using House.Repository.User;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using House.Utils;

namespace House.API.Controllers
{
    /// <summary>
    /// 设备控制器(管理所有设备)
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Device")]
    public class DeviceController : ControllerBase
    {
        private readonly IWaterMeterRepository _IWaterMeterRepository;

        public DeviceController(IWaterMeterRepository iwatermeterrepository)
        {
            _IWaterMeterRepository = iwatermeterrepository;
        }

        /// <summary>
        /// 水表查询（号楼）条件所用
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<Building>> GetAll()
        {
            var data = await _IWaterMeterRepository.GetAllListAsync();

            var Buikcount = data.Distinct(testc => testc.Building);

            List<Building> Blist = new List<Building>();

            foreach (var item in Buikcount)
            {
                Building a = new Building();
                a.Num = item.Building;
                a.Name = item.Building + "号楼";
                Blist.Add(a);
            }
            return Blist;
        }

        /// <summary>
        /// 水表查询(单元)条件所用
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<UnitNum>> GetUnitAll(int id)
        {
            if (id != -1)
            {
                var predicate = PredicateBuilder.New<WaterMeter>(true);
                predicate = predicate.And(t => t.Building == id);
                var data = await _IWaterMeterRepository.GetAllListAsync(predicate);

                var Unitcount = data.Distinct(t => t.UnitNum);
                List<UnitNum> Clist = new List<UnitNum>();
                foreach (var item in Unitcount)
                {
                    UnitNum a = new UnitNum();
                    a.Num = item.UnitNum;
                    a.Name = item.UnitNum + "单员";
                    Clist.Add(a);
                }
                return Clist;
            }
            return null;
        }

        /// <summary>
        /// 水表数据的显示
        /// </summary>
        /// <param name="building"></param>
        /// <param name="unitnum"></param>
        /// <param name="state"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageModel<WaterMeter>> WaterGetData(int building, int unitnum, string state, int pageindex, int pagesize)
        {
            var predicate = PredicateBuilder.New<WaterMeter>(true);
            if (building != -1)
            {
                predicate.And(t => t.Building == building);
            }
            if (unitnum != -1)
            {
                predicate.And(t => t.UnitNum == unitnum);
            }
            if (!string.IsNullOrEmpty(state))
            {
                if (state == "true")
                {
                    predicate.And(t => t.WaterState == Convert.ToBoolean(state));
                }
                else if (state == "false")
                {
                    predicate.And(t => t.WaterState == Convert.ToBoolean(state));
                }
                else
                {
                }
            }

            var data = await _IWaterMeterRepository.GetAllListAsync(predicate);

            PageModel<WaterMeter> datalist = new PageModel<WaterMeter>();
            datalist.PageCount = data.Count();
            datalist.PageSize = Convert.ToInt32(Math.Ceiling((data.Count * 1.0) / pagesize));
            datalist.Data = data.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            return datalist;
        }

        /// <summary>
        /// 修改水表的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> EditState(int id)
        {
            var predicate = PredicateBuilder.New<WaterMeter>(true);
            predicate.And(t => t.Id == id);
            var data = await _IWaterMeterRepository.FirstOrDefaultAsync(predicate);
            if (data.WaterState == true)
            {
                data.WaterState = false;
                return await _IWaterMeterRepository.UpdateAsync(data);
            }
            else
            {
                data.WaterState = true;
                return await _IWaterMeterRepository.UpdateAsync(data);
            }
        }
    }
}