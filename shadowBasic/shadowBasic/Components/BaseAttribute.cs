using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace shadowBasic.Components
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public abstract class BaseAttribute : Attribute
    {
        private bool? _isAsync;

        public bool IsAsync(MethodInfo method)
        {
            if(_isAsync == null)
                _isAsync = method.GetCustomAttribute<AsyncStateMachineAttribute>() != null && method.ReturnType == typeof(Task);

            return _isAsync == true;
        }
    }
}
