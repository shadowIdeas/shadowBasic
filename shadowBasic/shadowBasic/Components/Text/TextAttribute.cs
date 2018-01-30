using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shadowBasic.Components.Text
{
    public class TextAttribute : BaseAttribute
    {
        private readonly string _command;
        private readonly string _arguments;
        private readonly int _argumentCount;

        public string Command
        {
            get { return _command; }
        }

        public string Arguments
        {
            get { return _arguments; }
        }

        public int ArgumentCount
        {
            get { return _argumentCount; }
        }

        public TextAttribute(string command, string arguments = "", int argumentCount = 0, bool isAffectedByPause = true) 
            : base(isAffectedByPause)
        {
            if (command.Length == 0)
                throw new ArgumentException("Need to be at least one character long.", nameof(command));

            _command = command;
            _arguments = arguments;
            _argumentCount = argumentCount;
        }
    }
}
