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
using House.IRepository.CustomerManagement;
using House.Model.CustomerManagement;
using Core.Cache;
using System.IO;
using Microsoft.CodeAnalysis.CSharp;
using House.IRepository.ContractManagement;

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
        private readonly IFileinfoRepository _fileinfoRepository;


        public CustomerinfoController(ICustomerinfoRepository customerinfoRepository, IContractInfoRepository contractInfoRepository, IPersonchargeRepository personchargeRepository, IFileinfoRepository fileinfoRepository)
        {
            _customerinfoRepository = customerinfoRepository;
            _contractInfoRepository = contractInfoRepository;
            _personchargeRepository = personchargeRepository;
            _fileinfoRepository = fileinfoRepository;
        }

        /// <summary>
        /// 显示客户信息列表
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

            int i = data.Count();
            PageModel<Customerinfo> datalist = new PageModel<Customerinfo>();
            datalist.DataCount = i;
            datalist.PageCount = (int)Math.Ceiling(data.Count() * 1.0 / pagesize);
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
            try
            {
                var res = await _customerinfoRepository.InsertAsync(customerinfo);
                return res;
            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// 根据Id删除客户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> DelCustomer(int id)
        {
            try
            {
                var predicate = PredicateBuilder.New<Customerinfo>(true);

                predicate.And(t => t.Id==id);

                var model = await _customerinfoRepository.FirstOrDefaultAsync(predicate);

                var res = await _customerinfoRepository.DeleteAsync(model);

                return res;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        /// <summary>
        /// 根据Id删除联系人信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> DelPerson(int id)
        {
            try
            {
                var predicate = PredicateBuilder.New<Personcharge>(true);

                predicate.And(t => t.Id == id);

                var model = await _personchargeRepository.FirstOrDefaultAsync(predicate);

                var res = await _personchargeRepository.DeleteAsync(model);

                return res;
            }
            catch (Exception ex)
            {

                throw;
            }
        }



        /// <summary>
        /// 甲方负责人添加到Redis缓存服务器中 
        /// </summary>
        /// <param name="personcharge"></param>
        /// <returns></returns>
        [HttpPost]
        public bool PersonAddRedis(Personcharge personcharge)
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
                return true;
            }
            else
            {
                list.Add(personcharge);
                RedisHelper.Set("Person", list);
                return true;
            }
        }


        /// <summary>
        /// 批量添加联系人到数据库
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> PersonAdd(List<Personcharge> list)
        {
            int i = list.Count;
            int n = 0;
            foreach (var item in list)
            {
                var res = await _personchargeRepository.InsertAsync(item);
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


        /// <summary>
        /// 批量添加附件信息到数据库
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> InsertFileInfo(List<Fileinfo> list)
        {
            try
            {
                int i = list.Count;
                int n = 0;
                foreach (var item in list)
                {
                    var res = await _fileinfoRepository.InsertAsync(item);
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
        /// 从Redis中取出缓存的联系人表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<Personcharge> GetRedisPersoncharge(string cusid)
        {
            var list  =  RedisHelper.Get<List<Personcharge>>("Person");

            list = list.Where(a=>a.CustomerId==cusid).ToList(); 

            return list;
        }


        /// <summary>
        /// 甲方负责人显示(全部负责人=》负责人表)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageModel<Personcharge>> GetPerson(string name,string entrytime,string endtime, int pageindex, int pagesize)
        {
            var predicate = PredicateBuilder.New<Personcharge>(true);
            if (!string.IsNullOrWhiteSpace(name))
            {
                predicate.And(t => t.Name.Contains(name));
            }
            if (!string.IsNullOrEmpty(entrytime))
            {
                predicate.And(p => p.EntryTime >= Convert.ToDateTime(entrytime));
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                predicate.And(p => p.EntryTime <= Convert.ToDateTime(endtime));
            }
            var data = await _personchargeRepository.GetAllListAsync(predicate);
            int i = data.Count;
            PageModel<Personcharge> datalist = new PageModel<Personcharge>();
            datalist.DataCount = i;
            datalist.PageCount = (int)Math.Ceiling(data.Count() * 1.0 / pagesize);
            datalist.Data = data.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            return datalist;


        }


        /// <summary>
        /// 获取客户编号
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetDateTimeCode()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss");
        }


        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public Fileinfo UpFile(IFormFile file)
        {
            try
            {
                //添加时间戳 生成新文件名
                DateTime now = DateTime.Now;

                string newname = now.ToString("yyyyMMddHHmmss")+file.FileName;
                //截取字符串  获取文件格式
                int num = newname.LastIndexOf(".");
                var filetype = newname.Substring(num + 1);

                var filesize = GetFileSize(file.Length); //得到上传文件大小

                //设置存储路径
                string path = Directory.GetCurrentDirectory() + @"\wwwroot\File\"+ newname;
                //path 是文件地完整路径+文件名+扩展史
                //FileMode.Create 是指当前的行为是创建一个新文件
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                Fileinfo model = new Fileinfo
                {
                    FileName = newname,
                    FileSize = filesize,
                    UploadTime = now,
                    FileType = filetype,
                    Url = "https://localhost:44360/File/" + newname
                };

                return model;
            }
            catch (System.Exception ex)
            {
                Fileinfo model = new Fileinfo();
                return model;
            }
        }


        /// <summary>
        /// 上传的文件信息添加到Redis缓存服务器中 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public bool FileinfoAddRedis(Fileinfo model)
        {
            //加入redis
            //先从redis取出联系人数据
            var list = new List<Fileinfo>();

            list = RedisHelper.Get<List<Fileinfo>>("File");

            if (list == null)
            {
                var newlist = new List<Fileinfo>();
                newlist.Add(model);

                RedisHelper.Set("File", newlist);
                return true;
            }
            else
            {
                list.Add(model);
                RedisHelper.Set("File", list);
                return true;
            }
        }


        /// <summary>
        /// 从Redis中取出缓存的附件表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<Fileinfo> GetRedisFileinfo(string cusid)
        {
            var list = RedisHelper.Get<List<Fileinfo>>("File");

            list = list.Where(a => a.Cus_Id == cusid).ToList();

            return list;
        }


        /// <summary>
        /// 添加文件对象到redis
        /// </summary>
        /// <param name="info"></param>
        [HttpPost]
        public bool AddFileToRedis(Fileinfo info)
        {
            //加入redis
            //先从redis取出联系人数据
            var list = new List<Fileinfo>();

            list = RedisHelper.Get<List<Fileinfo>>("FileInfo");

            if (list == null)
            {
                var newlist = new List<Fileinfo>();
                newlist.Add(info);

                RedisHelper.Set("FileInfo", newlist);
                return true;
            }
            else
            {
                list.Add(info);
                RedisHelper.Set("FileInfo", list);
                return true;
            }
        }


        /// <summary>
        /// 格式化文件大小
        /// </summary>
        /// <param name="filesize">文件传入大小</param>
        /// <returns></returns>
        private static string GetFileSize(long filesize)
        {
            try
            {
                if (filesize < 0)
                {
                    return "0";
                }
                else if (filesize >= 1024 * 1024 * 1024)  //文件大小大于或等于1024MB    
                {
                    return string.Format("{0:0.00} GB", (double)filesize / (1024 * 1024 * 1024));
                }
                else if (filesize >= 1024 * 1024) //文件大小大于或等于1024KB    
                {
                    return string.Format("{0:0.00} MB", (double)filesize / (1024 * 1024));
                }
                else if (filesize >= 1024) //文件大小大于等于1024bytes    
                {
                    return string.Format("{0:0.00} KB", (double)filesize / 1024);
                }
                else
                {
                    return string.Format("{0:0.00} bytes", filesize);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        /// <summary>
        /// 获取全部联系人
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageModel<PersonChargeDto>> GetPersonChargeList(string pname,string cname,int index=1,int size=5)
        {
            var ppredicate = PredicateBuilder.New<Personcharge>(true);
            ppredicate = ppredicate.And(a => a.Name.Contains(pname));
            var plist = new List<Personcharge>();
            if (pname!=null)
            {
                plist = await _personchargeRepository.GetAllListAsync(ppredicate);
            }
            else
            {
                plist = await _personchargeRepository.GetAllListAsync();
            }
            


            var cpredicate = PredicateBuilder.New<Customerinfo>(true);
            cpredicate = cpredicate.And(a => a.CustomerName.Contains(cname));

            var clist = new List<Customerinfo>();
            if (cname!=null)
            {
                clist = await _customerinfoRepository.GetAllListAsync(cpredicate);
            }
            else
            {
                clist = await _customerinfoRepository.GetAllListAsync();
            }
            

            List<PersonChargeDto> list = clist.Join(plist,a=>a.Number, b => b.CustomerId, (a, b) => new PersonChargeDto
            {
                CustomerId = b.CustomerId,
                Name = b.Name,
                Post = b.Post,
                Phone = b.Phone,
                Dep = b.Dep,
                Email = b.Email,
                EntryTime = b.EntryTime,
                CustomerName = a.CustomerName
            }).ToList();

            PageModel<PersonChargeDto> datalist = new PageModel<PersonChargeDto>();
            datalist.DataCount = list.Count(); ;
            datalist.PageCount = (int)Math.Ceiling(list.Count() * 1.0 / size);
            datalist.Data = list.Skip((index - 1) * size).Take(size).ToList();
            return datalist;
        }


        /// <summary>
        /// 用于合同=》客户的信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageModel<Customerinfo>> Customer()
        {
            var cust = await _customerinfoRepository.GetAllListAsync();
            return new PageModel<Customerinfo> { Data = cust };
        }
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageModel<Customerinfo>> GetCusr(string number)
        {
            var predic = PredicateBuilder.New<Customerinfo>(true);
            predic.And(m=>m.Number==number);
            var cust = await _customerinfoRepository.FirstOrDefaultAsync(predic);
            return new PageModel<Customerinfo> { Item = cust };
        }

        ///// <summary>
        ///// 导出数据到Excel 标题 + 合并单元格
        ///// </summary>
        //public static void NpoiExportMergeExcel()
        //{
        //    //定义工作簿
        //    HSSFWorkbook workbook = new HSSFWorkbook();

        //    //创建Sheet表单
        //    HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet("前人种地后人收，说什么龙争虎斗");

        //    //设置表单列的宽度
        //    sheet.DefaultColumnWidth = 20;
        //    //sheet.SetColumnWidth(0, 30);

        //    //标题行
        //    HSSFRow dataRowTitle = (HSSFRow)sheet.CreateRow(0);
        //    dataRowTitle.CreateCell(0).SetCellStyle(workbook, "前人种地后人收，说什么龙争虎斗", ConfigNpoiCell.ConfigStyle.Head);

        //    //合并列 CellRangeAddress()该方法的参数次序是：开始行号，结束行号，开始列号，结束列号。
        //    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 4));

        //    //新建列头行
        //    HSSFRow dataRow = (HSSFRow)sheet.CreateRow(1);
        //    dataRow.CreateCell(0).SetCellStyle(workbook, "编号");
        //    dataRow.CreateCell(1).SetCellStyle(workbook, "姓名");
        //    dataRow.CreateCell(2).SetCellStyle(workbook, "家庭住址");
        //    dataRow.CreateCell(3).SetCellStyle(workbook, "收入");
        //    dataRow.CreateCell(4).SetCellStyle(workbook, "创建日期");
        //    dataRow.Height = 400;

        //    var row = 2;
        //    var persons = Person.GetPersons();
        //    persons.ForEach(m =>
        //    {
        //        dataRow = (HSSFRow)sheet.CreateRow(row);//新建数据行
        //        dataRow.CreateCell(0).SetCellStyle(workbook, m.Id);
        //        dataRow.CreateCell(1).SetCellStyle(workbook, m.Name);
        //        dataRow.CreateCell(2).SetCellStyle(workbook, m.Address);
        //        dataRow.CreateCell(3).SetCellStyle(workbook, m.InCome);
        //        dataRow.CreateCell(4).SetCellStyle(workbook, m.CreateTime);

        //        if (m.Id % 2 == 1)
        //        {
        //            //合并行
        //            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(row, row + 1, 0, 0));
        //        }
        //        row++;
        //    });

        //    var path = _hostingEnvironment.WebRootPath + "/excel/temp/" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

        //    using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
        //    {
        //        workbook.Write(fs);
        //    }
        //}


    }
}
