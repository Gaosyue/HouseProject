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
using House.Model.SystemSettings;
using House.Repository.Noticement;
using System.Collections.Generic;

namespace House.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Notice")]
    public class NoticeController : ControllerBase
    {
        private readonly INoticeRepository _INoticeRepository;
        private readonly IHumanResourcesRepository _IHumanResourcesRepository;
        public NoticeController(INoticeRepository noticeRepository, IHumanResourcesRepository iHumanResourcesRepository)
        {
            _INoticeRepository = noticeRepository;
            _IHumanResourcesRepository = iHumanResourcesRepository;
        }

        /// <summary>
        /// 获取人员所有信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<PerSonLIst>> GetHumen()
        {
            var predicate = PredicateBuilder.New<Humanresources>(true);

            var data = await _IHumanResourcesRepository.GetAllListAsync(predicate);
            List<PerSonLIst> list = new List<PerSonLIst>();
            foreach (var item in data)
            {
                PerSonLIst peritem = new PerSonLIst();
                peritem.key = item.Id;
                peritem.label = "     " + item.Name;
                list.Add(peritem);
            }
            return list;
        }

        /// <summary>
        /// 公告数据添加
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> CreateAdd(Notice notice)
        {
            var state = await _INoticeRepository.InsertAsync(notice);
            return state;
        }


        /// <summary>
        /// 显示合同信息列表
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

            var data = await _INoticeRepository.GetAllListAsync(predicate);

            int i = data.Count();
            PageModel<Notice> datalist = new PageModel<Notice>();
            datalist.DataCount = i;
            datalist.PageCount = (int)Math.Ceiling(data.Count() * 1.0 / pagesize);
            datalist.Data = data.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            return datalist;
        }

        /// <summary>
        /// 返回一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageModel<NoticeDto>> Recoil(int id)
        {
            var predicate = PredicateBuilder.New<Notice>(true);
            predicate.And(t => t.Id == id);
            var data = await _INoticeRepository.FirstOrDefaultAsync(predicate);

            NoticeDto Dtp = new NoticeDto();
            Dtp.Title = data.Title;
            Dtp.Content = data.Content;
            Dtp.ReleaseTime = data.ReleaseTime;
            Dtp.PublishUser = data.PublishUser;
            Dtp.State = data.State;



            List<int> i = new List<int>();
            var item = data.AcceptRole.Split(',');
            foreach (var a in item)
            {
                int d = Convert.ToInt32(a);
                i.Add(d);
            }
            var ids = PredicateBuilder.New<Humanresources>(true);
            ids.And(t => i.Contains(t.Id));

            var list = await _IHumanResourcesRepository.GetAllListAsync(ids);

            var name = "";
            foreach (var nameshow in list)
            {
                name += nameshow.Name + "    ";
            }
            name = name.Substring(0, name.Length - 1);
            Dtp.NameShow = name;
            PageModel<NoticeDto> Notice = new PageModel<NoticeDto>();
            Notice.Item = Dtp;
            return Notice;
        }


        /// <summary>
        /// 合同信息列表的反填
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<PageModel<Notice>> ReverNotice(int id)
        {
            var contra = PredicateBuilder.New<Notice>(m => m.Id == id);
            Notice contractInfo = await _INoticeRepository.FirstOrDefaultAsync(contra);
            var pagemodel = new PageModel<Notice>()
            {
                Item = contractInfo,
            };
            return pagemodel;
        }

        /// <summary>
        /// 合同信息的修改
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageModel<Notice>> UpReverNotice(Notice notice)
        {
            var cont = await _INoticeRepository.UpdateAsync(notice);
            var pagemodel = new PageModel<Notice>()
            {
                Code = cont ? "1" : "0"
            };
            return pagemodel;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<bool> Delete(int id)
        {
            try
            {
                var predicate = PredicateBuilder.New<Notice>(true);
                predicate.And(t => t.Id == id);
                return await _INoticeRepository.DeleteAsync(predicate);
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
    }
}
