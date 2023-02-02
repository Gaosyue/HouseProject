using House.Dto;
using House.IRepository.CustomerManagement;
using House.Model.CustomerManagement;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using House.Utils;
using Core.Cache;
using Microsoft.CodeAnalysis.CSharp;

namespace House.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Customerinfo")]
    public class CustomerinfoController : ControllerBase
    {
        private readonly ICustomerinfoRepository _customerinfoRepository;
        private readonly IContractInfoRepository _contractInfoRepository;
        private readonly IPersonchargeRepository _personchargeRepository;
        public CustomerinfoController(ICustomerinfoRepository customerinfoRepository, IContractInfoRepository contractInfoRepository, IPersonchargeRepository personchargeRepository)
        {
            _customerinfoRepository = customerinfoRepository;
            _contractInfoRepository = contractInfoRepository;
            _personchargeRepository = personchargeRepository;
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageModel<Customerinfo>> GetCust(string name, int pageindex, int pagesize)
        {
            var predicate = PredicateBuilder.New<Customerinfo>(true);
            if (!string.IsNullOrWhiteSpace(name))
            {
                predicate.And(t => t.CustomerName.Contains(name));
            }

            var data = await _customerinfoRepository.GetAllListAsync(predicate);

            PageModel<Customerinfo> datalist = new PageModel<Customerinfo>();
            datalist.PageCount = data.Count();
            datalist.PageSize = Convert.ToInt32(Math.Ceiling((data.Count * 1.0 / pagesize)));
            datalist.Data = data.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            return datalist;
        }

        /// <summary>
        /// 客户信息的录入
        /// </summary>
        /// <param name="customerinfo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> CustAdd(Customerinfo customerinfo)
        {
            var cust = await _customerinfoRepository.InsertAsync(customerinfo);
            return cust;
        }


        //[HttpPost]
        //public async Task<bool> FileinfoAdd(Fileinfo fileinfo)
        //{
        //    var cust = await _fileinfo.InsertAsync(fileinfo);
        //    return cust;
        //



        /// <summary>
        /// 甲方负责人添加到Redis缓存服务器中 
        /// </summary>
        /// <param name="personcharge"></param>
        /// <returns></returns>
        [HttpPost]

        public void PersonAddRedis(Personcharge personcharge)
        {
            //加入redis
            //先从redis取出联系人数据
            var list = new List<Personcharge>();

            list = RedisHelper.Get<List<Personcharge>>("Person");

            if (list==null)
            {
                var newlist = new List<Personcharge>();
                newlist.Add(personcharge);

                RedisHelper.Set("Person", newlist);
            }
            else
            {
                list.Add(personcharge);
                RedisHelper.Set("Person", list);

            }

        }


        /// <summary>
        /// 批量添加联系人到数据库
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> PersonAdd(List<Personcharge> list)
        {
            foreach (var item in list)
            {
                var cust = await _personchargeRepository.InsertAsync(item);
            }
            return true;
        }


        /// <summary>
        /// 从Redis中取出缓存的联系人表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<Personcharge> GetRedisPersoncharge()
        {
            var list  =  RedisHelper.Get<List<Personcharge>>("Person");

            return list;
        }


        /// <summary>
        /// 甲方负责人显示
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<Personcharge>> GetPerson()
        {
            try
            {
                var predicate = PredicateBuilder.New<Personcharge>(true);
                var data = await _personchargeRepository.GetAllListAsync(predicate);

                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
