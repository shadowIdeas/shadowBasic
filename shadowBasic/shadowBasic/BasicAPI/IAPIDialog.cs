using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shadowBasic.BasicAPI
{
    public interface IAPIDialog
    {
        bool IsOpen();
        void Close(int reason);
        int GetStringCount();
        string GetStringByIndex(int index);
        void SelectIndex(int index);
        void SetText(string text);
        string GetText();
        int GetID();
        void Block(int dialogID, string text);
        string BlockGetCaption();
        string BlockGetText();
        bool BlockHasBlockedDialog();
        bool BlockNeedBlocking();
    }
}
