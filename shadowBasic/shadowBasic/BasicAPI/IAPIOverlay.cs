using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shadowBasic.BasicAPI
{
    public interface IAPIOverlay
    {
        int TextCreate();
        void TextDelete(int id);
        void TextSetText(int id, string text);
        void TextSetSize(int id, int size);
        void TextSetColor(int id, uint color);
        void TextSetX(int id, int x);
        void TextSetY(int id, int y);
        void TextSetActive(int id, bool active);
        void TextSetUseMaxWidth(int id, bool useMaxWidth);
        void TextSetUseMaxHeight(int id, bool useMaxHeight);
        void TextSetMaxWidth(int id, int maxWidth);
        void TextSetMaxHeight(int id, int maxHeight);
        void TextGetTextExtent(int id, out int width, out int height);

        int BoxCreate();
        void BoxDelete(int id);
        void BoxSetColor(int id, uint color);
        void BoxSetX(int id, int x);
        void BoxSetY(int id, int y);
        void BoxSetWidth(int id, int width);
        void BoxSetHeight(int id, int height);
        void BoxSetActive(int id, bool active);
    }
}
