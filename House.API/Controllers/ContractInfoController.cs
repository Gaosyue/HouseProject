using Core.Cache;
using House.Dto;
using House.IRepository.ContractManagement;
using House.Model.ContractManagement;
using House.Model.CustomerManagement;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace House.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ContractInfo")]
    public class ContractInfoController : ControllerBase
    {
        private readonly IContractInfoRepository _contractInfoRepository;
        private readonly ISubscriptionInfoRepository _subscriptionInfoRepository;
        private readonly IContractChargesRepository _contractChargesRepository;


        public ContractInfoController(IContractInfoRepository contractInfoRepository, ISubscriptionInfoRepository subscriptionInfoRepository, IContractChargesRepository contractChargesRepository)
        {
            _contractInfoRepository = contractInfoRepository;
            _subscriptionInfoRepository = subscriptionInfoRepository;
            _contractChargesRepository=contractChargesRepository;
        }


        /// <summary>
        /// 显示合同信息列表
        /// </summary>
        /// <param name="contractName"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageModel<ContractInfo>> GetCust(string contractName, int pageindex, int pagesize)
        {
            var predicate = PredicateBuilder.New<ContractInfo>(true);
            if (!string.IsNullOrWhiteSpace(contractName))
            {
                predicate.And(t => t.ContractName.Contains(contractName));
            }

            var data = await _contractInfoRepository.GetAllListAsync(predicate);

            int i = data.Count();
            PageModel<ContractInfo> datalist = new PageModel<ContractInfo>();
            datalist.DataCount = i;
            datalist.PageCount = (int)Math.Ceiling(data.Count() * 1.0 / pagesize);
            datalist.Data = data.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            return datalist;
        }


        /// <summary>
        /// 合同信息的录入
        /// </summary>
        /// <param name="contractInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> SubscrAdd(ContractInfo contractInfo)
        {
            try
            {
                var res = await _contractInfoRepository.InsertAsync(contractInfo);
                return res;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 根据Id删除合同信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> DelSubscr(int id)
        {
            try
            {
                var predicate = PredicateBuilder.New<ContractInfo>(true);

                predicate.And(t => t.Id == id);

                var model = await _contractInfoRepository.FirstOrDefaultAsync(predicate);

                var res = await _contractInfoRepository.DeleteAsync(model);

                return res;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// 合同签约信息添加到Redis缓存服务器中 
        /// </summary>
        /// <param name="subscriptioninfo"></param>
        /// <returns></returns>
        [HttpPost]
        public bool SubscriptionAddRedis(Subscriptioninfo subscriptioninfo)
        {
            //加入redis
            //先从redis取出联系人数据
            var list = new List<Subscriptioninfo>();

            list = RedisHelper.Get<List<Subscriptioninfo>>("Subscr");

            if (list == null)
            {
                var newlist = new List<Subscriptioninfo>();
                newlist.Add(subscriptioninfo);

                RedisHelper.Set("Subscr", newlist);
                return true;
            }
            else
            {
                list.Add(subscriptioninfo);
                RedisHelper.Set("Subscr", list);
                return true;
            }
        }

        /// <summary>
        /// 从Redis中取出缓存的合同签约信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<Subscriptioninfo> GetRedisSubscr(string cusid)
        {
            var list = RedisHelper.Get<List<Subscriptioninfo>>("Subscr");

            if (list==null)
            {
                return null;
            }
            else
            {
                list = list.Where(a => a.ContractId == cusid).ToList();

                return list;
            }
        }


        /// <summary>
        /// 合同收费列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageModel<Subscriptioninfo>> GetPerson(string agreementName, int pageindex, int pagesize)
        {
            var predicate = PredicateBuilder.New<Subscriptioninfo>(true);
            if (!string.IsNullOrWhiteSpace(agreementName))
            {
                predicate.And(t => t.AgreementName.Contains(agreementName));
            }

            var data = await _subscriptionInfoRepository.GetAllListAsync(predicate);
            int i = data.Count;
            PageModel<Subscriptioninfo> datalist = new PageModel<Subscriptioninfo>();
            datalist.DataCount = i;
            datalist.PageCount = (int)Math.Ceiling(data.Count() * 1.0 / pagesize);
            datalist.Data = data.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            return datalist;
        }

        /// <summary>
        /// 收费列表详细
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageModel<ContractCharges>> GetContract(int pageindex, int pagesize)
        {
            var predicate = PredicateBuilder.New<ContractCharges>(true);
            var data = await _contractChargesRepository.GetAllListAsync(predicate);
            int i = data.Count;
            PageModel<ContractCharges> datalist = new PageModel<ContractCharges>();
            datalist.DataCount = i;
            datalist.PageCount = (int)Math.Ceiling(data.Count() * 1.0 / pagesize);
            datalist.Data = data.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            return datalist;
        }

        /// <summary>
        /// 批量添加合同签约信息到数据库
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> InsertFileInfo(List<Subscriptioninfo> list)
        {
            try
            {
                int i = list.Count;
                int n = 0;
                foreach (var item in list)
                {
                    var res = await _subscriptionInfoRepository.InsertAsync(item);
                    if (res)
                    {
                        n++;
                    }
                }
                if (n == i)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// 获取合同编号
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetDateTimeCode()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss");
        }
    }
}
