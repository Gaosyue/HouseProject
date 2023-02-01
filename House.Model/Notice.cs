using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Model
{
    /// <summary>
    /// 通知表
    /// </summary>
    public class Notice : EntityBase
    {
        
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime ReleaseTime { get; set; }
        public string PublishUser { get; set; }
        public int State { get; set; }
        public int AcceptRole { get; set; }
        public int AcceptClass { get; set; }
    }
}
