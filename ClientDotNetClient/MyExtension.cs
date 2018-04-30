using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Common
{
    public static class MyExtension
    {
        public static void RemoveAllEventHandlers(this Type self)
        {
            foreach (var ei in self.GetEvents(BindingFlags.Default))
            {
                var declaringType = ei.DeclaringType;
                var field = declaringType.GetField(ei.Name, BindingFlags.Default);
                if (field != null)
                {
                    var del = field.GetValue(null) as Delegate;
                    if (del != null)
                    {
                        foreach (var sub in del.GetInvocationList())
                        {
                            ei.RemoveEventHandler(null, sub);
                        }
                    }
                }
            }
        }

    }
}
