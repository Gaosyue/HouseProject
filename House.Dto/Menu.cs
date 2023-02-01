using System.Collections.Generic;

namespace House.Dto
{
    public class Menu
    {
        /// <summary>
        /// 路径
        /// </summary>
        public string path { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        /// 关联的上级
        /// </summary>
        public int PId { get; set; }

        /// <summary>
        /// 下菜单
        /// </summary>
        public List<Menu> children { get; set; }
    }
}