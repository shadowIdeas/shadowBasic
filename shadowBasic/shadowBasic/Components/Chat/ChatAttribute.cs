using System;

namespace shadowBasic.Components.Chat
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ChatAttribute : Attribute
    {
        private readonly string _regex;

        public ChatAttribute(string regex)
        {
            _regex = regex;
        }
    }
}
