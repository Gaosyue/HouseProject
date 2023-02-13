using House.Dto;
using House.IRepository.NoticeManage;
using House.Model.ContractManagement;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using House.Model;
using System.Linq;
using House.Model.CustomerManagement;

namespace House.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Notice")]
    public class NoticeController : ControllerBase
    {
        private readonly INoticeRepository _noticeRepository;
        public NoticeController(INoticeRepository noticeRepository)
        {
            _noticeRepository = noticeRepository;
        }

        /// <summary>
        /// 公告
        /// </summary>
        /// <param name="title"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageModel<Notice>> GetCust(string title, int pageindex, int pagesize)
        {
            var predicate = PredicateBuilder.New<Notice>(true);
            if (!string.IsNullOrWhiteSpace(title))
            {
                predicate.And(t => t.Title.Contains(title));
            }

            var data = await _noticeRepository.GetAllListAsync(predicate);

            int i = data.Count();
            PageModel<Notice> datalist = new PageModel<Notice>();
            datalist.DataCount = i;
            datalist.PageCount = (int)Math.Ceiling(data.Count() * 1.0 / pagesize);
            datalist.Data = data.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            return datalist;
        }

        /// <summary>
        /// 根据Id删除公告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> DelNotice(int id)
        {
            try
            {
                var predicate = PredicateBuilder.New<Notice>(true);

                predicate.And(t => t.Id == id);

                var model = await _noticeRepository.FirstOrDefaultAsync(predicate);

                var res = await _noticeRepository.DeleteAsync(model);

                return res;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// 公告列表的反填
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<PageModel<Notice>> ReverContractInfo(int id)
        {
            var contra = PredicateBuilder.New<Notice>(m => m.Id == id);
            Notice notice = await _noticeRepository.FirstOrDefaultAsync(contra);
            var pagemodel = new PageModel<Notice>()
            {
                Item = notice,
            };
            return pagemodel;
        }

        /// <summary>
        /// 公告的修改
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageModel<Notice>> UpContractInfo(Notice notice)
        {
            var cont = await _noticeRepository.UpdateAsync(notice);
            var pagemodel = new PageModel<Notice>()
            {
                Code = cont ? "1" : "0"
            };
            return pagemodel;
        }

        /// <summary>
        /// 公告的录入
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> CustAdd(Notice notice)
        {
            try
            {
                var res = await _noticeRepository.InsertAsync(notice);
                return res;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
