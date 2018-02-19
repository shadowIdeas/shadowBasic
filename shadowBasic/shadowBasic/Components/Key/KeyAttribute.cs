using System;
using static shadowBasic.Interop.UtilInterop;

namespace shadowBasic.Components.Key
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class KeyAttribute : BaseAttribute
    {
        private Keys _key;
        private Keys _modifierKey;

        public Keys Key
        {
            get { return _key; }
            protected set { _key = value; }
        }

        public Keys ModifierKey
        {
            get { return _modifierKey; }
            protected set { _modifierKey = value; }
        }

        public KeyAttribute(Keys key = Keys.VK_NONE, Keys modifierKey = Keys.VK_NONE, bool isAffectedByPause = true)
            : base(isAffectedByPause)
        {
            _key = key;
            _modifierKey = modifierKey;
        }
    }
}