using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House.Model
{
    /// <summary>
    /// 水表
    /// </summary>
    public class WaterMeter : EntityBase
    {
        /// <summary>
        /// 楼号
        /// </summary>
        public int Building { get; set; }

        /// <summary>
        /// 单元号
        /// </summary>
        public int UnitNum { get; set; }

        /// <summary>
        /// 层号
        /// </summary>
        public int FloorNum { get; set; }

        /// <summary>
        /// 剩余水费
        /// </summary>
        public int SurplusWater { get; set; }

        /// <summary>
        /// 累计水费
        /// </summary>
        public int SumWater { get; set; }

        /// <summary>
        /// 水表状态
        /// </summary>
        public bool WaterState { get; set; }
    }
}