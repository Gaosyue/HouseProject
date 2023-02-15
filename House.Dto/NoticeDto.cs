using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Dto
{
    public class NoticeDto
    {

        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime ReleaseTime { get; set; }
        public string PublishUser { get; set; }
        public int State { get; set; }
       

        public string NameShow { get; set; }
    }
}
