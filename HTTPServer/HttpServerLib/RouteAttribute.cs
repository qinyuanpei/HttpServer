using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPServerLib
{
    [AttributeUsage(AttributeTargets.Method)]
    class RouteAttribute:Attribute
    {
        public RouteMethod Method { get; set; }
        public string RoutePath { get; set; }
    }
}
