using Core.Cache;
using ExportImportExcle;
using House.Dto;
using House.IRepository.ContractManagement;
using House.Model.ContractManagement;
using House.Model.CustomerManagement;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
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
            _contractChargesRepository = contractChargesRepository;
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
        /// 显示收费详情列表
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageModel<ContractCharges>> GetContractChargesList(string contractId, int pageindex = 1, int pagesize = 10)
        {
            ExpressionStarter<ContractCharges> predicate = PredicateBuilder.New<ContractCharges>(true);
            if (!string.IsNullOrWhiteSpace(contractId))
            {
                predicate.And(t => t.ContractId == contractId);
            }

            var data = await _contractChargesRepository.GetAllListAsync(predicate);

            int i = data.Count();
            PageModel<ContractCharges> datalist = new PageModel<ContractCharges>();
            datalist.DataCount = i;
            datalist.PageCount = (int)Math.Ceiling(data.Count() * 1.0 / pagesize);
            datalist.Data = data.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            return datalist;
        }


        /// <summary>
        /// 根据Id删除收费信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> DelContractCharges(int id)
        {
            try
            {
                var predicate = PredicateBuilder.New<ContractCharges>(true);

                predicate.And(t => t.Id == id);

                var model = await _contractChargesRepository.FirstOrDefaultAsync(predicate);

                var res = await _contractChargesRepository.DeleteAsync(model);

                return res;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        /// <summary>
        /// 收费列表的录入
        /// </summary>
        /// <param name="contractCharges"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ContractAdd(ContractCharges contractCharges)
        {
            try
            {
                var res = await _contractChargesRepository.InsertAsync(contractCharges);

                var result = UpdateContract(contractCharges.ContractId, contractCharges.AmountRecorded);

                if (res && result)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// 修改合同总收费金额 每次收费+=金额，然后更新
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="amountRecorded"></param>
        /// <returns></returns>
        private bool UpdateContract(string contractId, decimal amountRecorded)
        {
            ContractInfo model = _contractInfoRepository.FirstOrDefault(a => a.ContractId == contractId);

            model.Percentage += amountRecorded;

            var res = _contractInfoRepository.Update(model);

            return res;
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

            if (list == null)
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


        /// <summary>
        /// 合同信息列表的反填
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<PageModel<ContractInfo>> ReverContractInfo(int id)
        {
            var contra = PredicateBuilder.New<ContractInfo>(m => m.Id == id);
            ContractInfo contractInfo = await _contractInfoRepository.FirstOrDefaultAsync(contra);
            var pagemodel = new PageModel<ContractInfo>()
            {
                Item = contractInfo,
            };
            return pagemodel;
        }

        /// <summary>
        /// 合同信息的修改
        /// </summary>
        /// <param name="contractInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageModel<ContractInfo>> UpContractInfo(ContractInfo contractInfo)
        {
            var cont = await _contractInfoRepository.UpdateAsync(contractInfo);
            var pagemodel = new PageModel<ContractInfo>()
            {
                Code = cont ? "1" : "0"
            };
            return pagemodel;
        }

        /// <summary>
        /// 导出数据到Excel中
        /// </summary>
        [HttpGet]
        public async Task<FileResult> PersonNpoiExportExcel()
        {
            //定义工作簿
            HSSFWorkbook workbook = new HSSFWorkbook();
            //创建Sheet表单
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet("合同信息");
            //设置表单列的宽度
            sheet.DefaultColumnWidth = 20;

            //新建标题行
            HSSFRow dataRow = (HSSFRow)sheet.CreateRow(0);
            dataRow.CreateCell(0).SetCellValue("合同编号");
            dataRow.CreateCell(1).SetCellValue("合同名称");
            dataRow.CreateCell(2).SetCellValue("建设单位");
            dataRow.CreateCell(3).SetCellValue("合同额");
            dataRow.CreateCell(4).SetCellValue("实际合同额");
            dataRow.CreateCell(5).SetCellValue("签约日期");
            dataRow.CreateCell(6).SetCellValue("工程负责人");
            dataRow.CreateCell(7).SetCellValue("时间");

            var row = 1;
            var data = await _contractInfoRepository.GetAllListAsync();
            data.ForEach(m =>
            {
                dataRow = (HSSFRow)sheet.CreateRow(row);//新建数据行
                dataRow.CreateCell(0).SetCellValue(m.Id);
                dataRow.CreateCell(1).SetCellValue(m.ContractId);
                dataRow.CreateCell(2).SetCellValue(m.ContractNum);
                dataRow.CreateCell(3).SetCellValue(m.ConstructionUnit);
                dataRow.CreateCell(4).SetCellValue(m.OriginalAmount.ToString());
                dataRow.CreateCell(5).SetCellValue(m.ActualAmount.ToString());
                dataRow.CreateCell(6).SetCellValue(m.ProjectLeader);
                dataRow.CreateCell(7).SetCellValue(m.SigningDate.ToString());
                row++;
            });
            var fs = new MemoryStream();
            workbook.Write(fs);
            byte[] b = fs.ToArray();
            //关键语句
            return File(b, System.Net.Mime.MediaTypeNames.Application.Octet, "合同信息数据.xls"); 
        }
    }
}
