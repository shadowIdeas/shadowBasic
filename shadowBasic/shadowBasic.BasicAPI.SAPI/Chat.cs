using SAPI;

namespace shadowBasic.BasicAPI.SAPI
{
    internal class Chat : IAPIChat
    {
        public void AddBufferMessage(string message)
        {
            ChatAPI.Instance.AddBufferMessage(message);
        }

        public void AddMessage(string message)
        {
            ChatAPI.Instance.AddMessage(message);
        }

        public void Clear()
        {
            ChatAPI.Instance.Clear();
        }

        public string GetText()
        {
            return ChatAPI.Instance.GetText();
        }

        public bool IsOpen()
        {
            return ChatAPI.Instance.IsOpen();
        }

        public void Send(string message)
        {
            ChatAPI.Instance.Send(message);
        }

        public void SetCursorPosition(int begin, int end)
        {
            ChatAPI.Instance.SetCursorPosition(begin, end);
        }

        public void SetText(string text)
        {
            ChatAPI.Instance.SetText(text);
        }

        public void Toggle(bool open)
        {
            ChatAPI.Instance.Toggle(open);
        }
    }
}
