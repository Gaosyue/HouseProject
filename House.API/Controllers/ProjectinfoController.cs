using House.Dto;
using House.IRepository.ContractManagement;
using House.IRepository.CustomerManagement;
using House.Model.ContractManagement;
using House.Model.CustomerManagement;
using House.Model.OperationsManagement;
using House.Repository.ContractManagement;
using House.Repository.Customer;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace House.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Projectinfo")]
    public class ProjectinfoController : ControllerBase
    {
        private readonly IContractChargesRepository _contractChargesRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IContractInfoRepository _infoRepository;


        public ProjectinfoController( IProjectRepository projectRepository , IContractInfoRepository infoRepository)
        {
            _projectRepository = projectRepository;
            _infoRepository = infoRepository;

        }

        /// <summary>
        /// 获取合同信息 (绑定下拉框数据)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageModel<ContractInfo>> GetContractInfo()
        {
            var contra=await _infoRepository.GetAllListAsync();
            return new PageModel<ContractInfo> { Data=contra};
        }

        /// <summary>
        /// 根据选中数据获取合同详情
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageModel<ContractInfo>> GetContractChargesId(string id)
        {
            var contra = await _infoRepository.FirstOrDefaultAsync(m => m.ContractId == id);
            return new PageModel<ContractInfo> { Item = contra };
        }

        /// <summary>
        /// 添加项目立项
        /// </summary>
        /// <param name="projectinfo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageModel<Projectinfo>> Add_project(Projectinfo projectinfo)
        {
            projectinfo.EntryTime = DateTime.Now;
            var flag = await _projectRepository.InsertAsync(projectinfo);
            var pageModel = new PageModel<Projectinfo>()
            {
                Code = flag ? "1" : "0"
            };
            return pageModel;
        }


        /// <summary>
        /// 项目立项
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageModel<Projectinfo>> GetPerson(int pageindex=1, int pagesize=10)
        {
            var predicate = PredicateBuilder.New<Projectinfo>(true);
            
            var data = await _projectRepository.GetAllListAsync(predicate);
            int i = data.Count;
            PageModel<Projectinfo> datalist = new PageModel<Projectinfo>();
            datalist.DataCount = i;
            datalist.PageCount = (int)Math.Ceiling(data.Count() * 1.0 / pagesize);
            datalist.Data = data.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            return datalist;
        }

        /// <summary>
        /// 项目立项的反填
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<PageModel<Projectinfo>> ReverProject(int id)
        {
            var contra = PredicateBuilder.New<Projectinfo>(m => m.Id == id);
            Projectinfo contractInfo = await _projectRepository.FirstOrDefaultAsync(contra);
            var pagemodel = new PageModel<Projectinfo>()
            {
                Item = contractInfo,
            };
            return pagemodel;
        }

        /// <summary>
        /// 项目立项的修改
        /// </summary>
        /// <param name="projectinfo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageModel<Projectinfo>> UpContractInfo(Projectinfo projectinfo)
        {
            var cont = await _projectRepository.UpdateAsync(projectinfo);
            var pagemodel = new PageModel<Projectinfo>()
            {
                Code = cont ? "1" : "0"
            };
            return pagemodel;
        }

        /// <summary>
        /// 根据Id删除合同信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> DelProject(int id)
        {
            try
            {
                var predicate = PredicateBuilder.New<Projectinfo>(true);

                predicate.And(t => t.Id == id);

                var model = await _projectRepository.FirstOrDefaultAsync(predicate);

                var res = await _projectRepository.DeleteAsync(model);

                return res;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }

}
