using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shadowBasic
{
    public interface IDisposableEx : IDisposable
    {
        void Dispose(bool disposing);
    }
}
