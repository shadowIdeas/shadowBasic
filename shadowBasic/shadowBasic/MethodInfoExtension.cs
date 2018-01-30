using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace shadowBasic
{
    internal static class MethodInfoExtension
    {
        public static async Task InvokeAsync(this MethodInfo method, object obj, params object[] parameters)
        {
            dynamic invoke = method.Invoke(obj, parameters);
            await invoke;
        }

        public static async Task<T> InvokeAsync<T>(this MethodInfo method, object obj, params object[] parameters)
        {
            dynamic invoke = method.Invoke(obj, parameters);
            await invoke;

            return invoke.GetAwaiter().GetResult();
        }
    }
}
