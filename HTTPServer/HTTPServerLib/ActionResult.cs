using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPServerLib
{
    public class ActionResult
    {
        public object Result { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
