using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace HTTPServerLib
{
    public class ServiceModule
    {
        public bool SearchRoute(ServiceRoute route)
        {
            return true;
        }

        public ActionResult ExecuteRoute(ServiceRoute route)
        {
            var type = this.GetType();
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            methods = methods.Where(m => m.ReturnType == typeof(ActionResult)).ToArray();
            if (methods == null || methods.Length <= 0) return null;
            var method = methods.FirstOrDefault(m =>
            {
                var attribute = m.GetCustomAttribute<RouteAttribute>(true);
                if (attribute == null) return false;
                if (attribute.Method == route.Method && attribute.RoutePath == route.RoutePath)
                    return true;
                return false;
            });

            if (method == null) return null;
            return (ActionResult)method.Invoke(this, new object[] { });

        }
    }
}
