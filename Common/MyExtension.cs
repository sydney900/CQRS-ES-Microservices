using System;
using System.Reflection;

namespace Common
{
    public static class MyExtension
    {
        public static void RemoveAllEventHandlers<T>(this T t)
        {
            foreach (var ei in t.GetType().GetEvents(BindingFlags.Default))
            {
                var declaringType = ei.DeclaringType;
                var field = declaringType.GetField(ei.Name, BindingFlags.Default);
                if (field != null)
                {
                    var del = field.GetValue(t) as Delegate;
                    if (del != null)
                    {
                        foreach (var sub in del.GetInvocationList())
                        {
                            ei.RemoveEventHandler(t, sub);
                        }
                    }
                }
            }
        }

    }
}
