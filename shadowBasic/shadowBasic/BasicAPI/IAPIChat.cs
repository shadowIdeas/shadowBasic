namespace shadowBasic.BasicAPI
{
    public interface IAPIChat
    {
        void AddBufferMessage(string message);
        void AddMessage(string message);
        void Clear();
        void Send(string message);
        void SetCursorPosition(int begin, int end);
        void SetText(string text);
        void Toggle(bool open);

        bool IsOpen();
        string GetText();
    }
}