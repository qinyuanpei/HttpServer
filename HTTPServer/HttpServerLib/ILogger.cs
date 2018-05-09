using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPServerLib
{
    //基本日志接口
    public interface ILogger
    {
        void Log(object message);
    }
}
