using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetJob.Enum;

public enum JobTriggerTypeEnum
{
    /// <summary>
    ///  简单模式
    /// </summary>
    Simple = 0,

    /// <summary>
    ///  公式模式
    /// </summary>
    Cron = 1
}
