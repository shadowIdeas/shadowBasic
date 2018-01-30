using System;

namespace shadowBasic.Components.Chat
{
    public class ChatAttribute : BaseAttribute
    {
        private readonly string _regex;

        public string Regex
        {
            get { return _regex; }
        }

        public ChatAttribute(string regex)
        {
            _regex = regex;
        }
    }
}
