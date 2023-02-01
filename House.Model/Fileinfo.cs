using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Model
{
    /// <summary>
    /// 附件表
    /// </summary>
    public class Fileinfo : EntityBase
    {
   
        public string Cus_Id { get; set; }
        public string FileName { get; set; }
        public DateTime UploadTime { get; set; }
        public string FileSize { get; set; }
        public string FileType { get; set; }
        public string Enteredby { get; set; }
        public string Url { get; set; }
        public string FIleCategroy { get; set; }
    }
}
