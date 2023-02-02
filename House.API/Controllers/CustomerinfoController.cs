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
using System.IO;
using Microsoft.AspNetCore.Hosting;
using House.Model;
using NPOI.HPSF;

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
    }
}
