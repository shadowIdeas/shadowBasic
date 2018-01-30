using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static shadowBasic.Interop.UtilInterop;

namespace shadowBasic.Components.Key
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class KeyAttribute : BaseAttribute
    {
        private readonly Keys _key;
        private readonly Keys _modifierKey;

        public Keys Key
        {
            get { return _key; }
        }

        public Keys ModifierKey
        {
            get { return _modifierKey; }
        }

        public KeyAttribute(Keys key, Keys modifierKey = Keys.VK_NONE, bool isAffectedByPause = true)
            : base(isAffectedByPause)
        {
            if (key == Keys.VK_NONE)
                throw new ArgumentException("Cannot be none.", nameof(key));

            _key = key;
            _modifierKey = modifierKey;
        }
    }
}
