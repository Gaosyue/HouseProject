using House.Dto;
using House.IRepository;
using House.IRepository.User;
using House.Model;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using SecretDemo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace House.API.Controllers

{
    /// <summary>
    /// 用户登录控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Login")]
    public class LoginController : ControllerBase
    {
        private readonly IPersonnelRepository _IPersonnelRepository;
        private readonly IPersonnelRoleRepository _IPersonnelRoleRepository;
        private readonly IRoleRepository _IRoleRepository;
        private readonly IRolePowerRepository _IRolePowerRepository;
        private readonly IPowerRepository _IPowerRepository;

        public LoginController(IPersonnelRepository iPersonnelRepository, IPersonnelRoleRepository iPersonnelRoleRepository, IRoleRepository iRoleRepository, IRolePowerRepository iRolePowerRepository, IPowerRepository iPowerRepository)
        {
            _IPersonnelRepository = iPersonnelRepository;
            _IPersonnelRoleRepository = iPersonnelRoleRepository;
            _IRoleRepository = iRoleRepository;
            _IRolePowerRepository = iRolePowerRepository;
            _IPowerRepository = iPowerRepository;
        }

        /// <summary>
        /// 登录功能
        /// </summary>
        /// <returns></returns>
        /// <returns></returns>
        [HttpGet]
        public async Task<Token<Personnel>> UserLogin(string account, string password)
        {
            //使用DES加密
            var key = "1qaz2wsx";
            var iv = "lkj%0987";
            Console.WriteLine(password);
            var DESpwd = DESHelper.DESEncrypt(password, key, iv);
            //数据库中的存储的密码是加密后的
            var predicate = PredicateBuilder.New<Personnel>(true);
            predicate.And(t => t.Account == account && t.Pwd == DESpwd);

            var data = await _IPersonnelRepository.FirstOrDefaultAsync(predicate);



            if (data != null)
            {
                Token<Personnel> d = new Token<Personnel>();
                d.Result = data;

                return d;
            }
            else
            {
                Token<Personnel> d = new Token<Personnel>();
                d.Code = 404;
                d.Message = "你请求的数据不对";
                return d;
            }
        }

        /// <summary>
        /// 权限查询
        /// </summary>
        /// <param name="id"></param>e
        /// <returns></returns>
        [HttpGet]
        public async Task<List<Menu>> GetPermissions(int id)
        {
            var Users = _IPersonnelRepository.GetAllList();
            var UserRoles = _IPersonnelRoleRepository.GetAllList();
            var Roles = _IRoleRepository.GetAllList();
            var RolePermissions = _IRolePowerRepository.GetAllList();
            var Permissions = _IPowerRepository.GetAllList();
            var datalist = (
                        from a in Users
                        join b in UserRoles on a.Id equals b.PersonnelId
                        join c in Roles on b.RoleId equals c.Id
                        join d in RolePermissions on c.Id equals d.RoleId
                        join e in Permissions on d.PowerId equals e.Id
                        where a.Id == id && c.State == true
                        select e).ToList();
            List<Menu> data = new List<Menu>();
            data = await GetMenu(datalist);
            return data;
        }

        /// <summary>
        /// 菜单显示
        /// </summary>
        [HttpGet]
        public async Task<List<Menu>> GetMenu(List<Model.Power> datalist)
        {
            var nodes = datalist.Where(d => d.SuperiorId == 0).ToList();
            var q = from n in nodes
                    select new Menu()
                    {
                        name = n.Name,
                        icon = n.Icon,
                        Id = n.Id,
                        PId = n.SuperiorId,
                        path = n.Url,
                    };
            List<Menu> list = q.ToList();

            GetSon(list, datalist);
            return list;
        }

        /// <summary>
        /// 递归
        /// </summary>
        /// <param name="dtolist"></param>
        private void GetSon(List<Menu> dtolist, List<Model.Power> datalist)
        {
            foreach (var n in dtolist)
            {
                var n_1 = datalist.Where(d => d.SuperiorId == n.Id).ToList();
                var q_1 = from node in n_1
                          select new Menu()
                          {
                              name = node.Name,
                              icon = node.Icon,
                              Id = node.Id,
                              PId = node.SuperiorId,
                              path = node.Url,
                          };

                List<Menu> list = q_1.ToList();
                if (list.Count() > 0)
                {
                    n.children = new List<Menu>();
                    n.children.AddRange(list);
                }

                GetSon(list, datalist);
            }
        }

        /// <summary>
        /// 导出数据到Excel中
        /// </summary>
        //public FileResult NpoiExportExcel()
        //{
        //    //定义工作簿
        //    HSSFWorkbook workbook = new HSSFWorkbook();
        //    //创建Sheet表单
        //    HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet("车辆信息");
        //    //设置表单列的宽度
        //    sheet.DefaultColumnWidth = 20;

        //    //新建标题行
        //    HSSFRow dataRow = (HSSFRow)sheet.CreateRow(0);
        //    dataRow.CreateCell(0).SetCellValue("厂牌型号");
        //    dataRow.CreateCell(1).SetCellValue("车牌号");
        //    dataRow.CreateCell(2).SetCellValue("司机姓名");
        //    dataRow.CreateCell(3).SetCellValue("所属公司");
        //    dataRow.CreateCell(4).SetCellValue("车型长");
        //    dataRow.CreateCell(5).SetCellValue("车型宽");
        //    dataRow.CreateCell(6).SetCellValue("车型高");
        //    dataRow.CreateCell(7).SetCellValue("车身颜色");
        //    dataRow.CreateCell(8).SetCellValue("购买日期");
        //    dataRow.CreateCell(9).SetCellValue("运营证号");
        //    dataRow.CreateCell(10).SetCellValue("保险到期时间");
        //    dataRow.CreateCell(11).SetCellValue("年检到期时间");
        //    dataRow.CreateCell(12).SetCellValue("保养公里设置");
        //    var row = 1;
        //    var persons = _app.GetAll().Result.data;
        //    persons.ForEach(m =>
        //    {
        //        dataRow = (HSSFRow)sheet.CreateRow(row);//新建数据行

        //        dataRow.CreateCell(0).SetCellValue(m.LableModel);
        //        dataRow.CreateCell(1).SetCellValue(m.CarNum);
        //        dataRow.CreateCell(2).SetCellValue(m.DriverName);
        //        dataRow.CreateCell(3).SetCellValue(m.FromCompany);
        //        dataRow.CreateCell(4).SetCellValue(m.Long.ToString());
        //        dataRow.CreateCell(5).SetCellValue(m.Width.ToString());
        //        dataRow.CreateCell(6).SetCellValue(m.Heigth.ToString());
        //        dataRow.CreateCell(7).SetCellValue(m.Color);
        //        dataRow.CreateCell(8).SetCellValue(m.BuyDate.ToString());
        //        dataRow.CreateCell(9).SetCellValue(m.OperationNum);
        //        dataRow.CreateCell(10).SetCellValue(m.InsuranceDate.ToString());
        //        dataRow.CreateCell(11).SetCellValue(m.YearDate.ToString());
        //        dataRow.CreateCell(12).SetCellValue(m.MaintenanceKmSetting);

        //        row++;
        //    });

        //    var fs = new MemoryStream();

        //    workbook.Write(fs);

        //    byte[] b = fs.ToArray();
        //    return File(b, System.Net.Mime.MediaTypeNames.Application.Octet, "车辆数据.xls"); //关键语句
        //}
    }
}