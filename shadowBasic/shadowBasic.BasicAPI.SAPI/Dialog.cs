using SAPI;

namespace shadowBasic.BasicAPI.SAPI
{
    internal class Dialog : IAPIDialog
    {
        public void Block(int dialogID, string text)
        {
            DialogAPI.Instance.Block(dialogID, text);
        }

        public string BlockGetCaption()
        {
            return DialogAPI.Instance.BlockGetCaption();
        }

        public string BlockGetText()
        {
            return DialogAPI.Instance.GetText();
        }

        public bool BlockHasBlockedDialog()
        {
            return DialogAPI.Instance.BlockHasBlockedDialog();
        }

        public bool BlockNeedBlocking()
        {
            return DialogAPI.Instance.BlockNeedBlocking();
        }

        public void Close(int reason)
        {
            DialogAPI.Instance.Close(reason);
        }

        public int GetId()
        {
            return DialogAPI.Instance.GetID();
        }

        public string GetStringByIndex(int index)
        {
            return DialogAPI.Instance.GetStringByIndex(index);
        }

        public int GetStringCount()
        {
            return DialogAPI.Instance.GetStringCount();
        }

        public string GetText()
        {
            return DialogAPI.Instance.GetText();
        }

        public bool IsOpen()
        {
            return DialogAPI.Instance.IsOpen();
        }

        public void SelectIndex(int index)
        {
            DialogAPI.Instance.SelectIndex(index);
        }

        public void SetText(string text)
        {
            DialogAPI.Instance.SetText(text);
        }
    }
}
