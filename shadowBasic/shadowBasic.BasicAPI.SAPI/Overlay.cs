using SAPI.Overlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shadowBasic.BasicAPI.SAPI
{
    internal class Overlay : IAPIOverlay
    {
        public int BoxCreate()
        {
            return BoxAPI.Instance.Create();
        }

        public void BoxDelete(int id)
        {
            BoxAPI.Instance.Delete(id);
        }

        public void BoxSetActive(int id, bool active)
        {
            BoxAPI.Instance.SetActive(id, active);
        }

        public void BoxSetColor(int id, uint color)
        {
            BoxAPI.Instance.SetColor(id, color);
        }

        public void BoxSetHeight(int id, int height)
        {
            BoxAPI.Instance.SetHeight(id, height);
        }

        public void BoxSetWidth(int id, int width)
        {
            BoxAPI.Instance.SetWidth(id, width);
        }

        public void BoxSetX(int id, int x)
        {
            BoxAPI.Instance.SetX(id, x);
        }

        public void BoxSetY(int id, int y)
        {
            BoxAPI.Instance.SetY(id, y);
        }

        public int TextCreate()
        {
            return TextAPI.Instance.Create();
        }

        public void TextDelete(int id)
        {
            TextAPI.Instance.Delete(id);
        }

        public void TextGetTextExtent(int id, out int width, out int height)
        {
            TextAPI.Instance.GetTextExtent(id, out width, out height);
        }

        public void TextSetActive(int id, bool active)
        {
            TextAPI.Instance.SetActive(id, active);
        }

        public void TextSetColor(int id, uint color)
        {
            TextAPI.Instance.SetColor(id, color);
        }

        public void TextSetMaxHeight(int id, int maxHeight)
        {
            TextAPI.Instance.SetMaxHeight(id, maxHeight);
        }

        public void TextSetMaxWidth(int id, int maxWidth)
        {
            TextAPI.Instance.SetMaxWidth(id, maxWidth);
        }

        public void TextSetSize(int id, int size)
        {
            TextAPI.Instance.SetSize(id, size);
        }

        public void TextSetText(int id, string text)
        {
            TextAPI.Instance.SetText(id, text);
        }

        public void TextSetUseMaxHeight(int id, bool useMaxHeight)
        {
            TextAPI.Instance.SetUseMaxHeight(id, useMaxHeight);
        }

        public void TextSetUseMaxWidth(int id, bool useMaxWidth)
        {
            TextAPI.Instance.SetUseMaxWidth(id, useMaxWidth);
        }

        public void TextSetX(int id, int x)
        {
            TextAPI.Instance.SetX(id, x);
        }

        public void TextSetY(int id, int y)
        {
            TextAPI.Instance.SetY(id, y);
        }
    }
}
