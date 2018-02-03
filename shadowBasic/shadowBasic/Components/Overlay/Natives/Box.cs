using shadowBasic.BasicAPI;
using System;

namespace shadowBasic.Components.Overlay.Natives
{
    public class Box
    {
        private int _id;

        private uint _color;
        private int _x;
        private int _y;
        private int _width;
        private int _height;

        private bool active;

        public int ID
        {
            get { return _id; }
        }

        public uint Color
        {
            get { return _color; }
            set
            {
                _color = value;
                if (IsValidID)
                    API.Instance.Overlay.BoxSetColor(_id, _color);
            }
        }

        public int X
        {
            get { return _x; }
            set
            {
                _x = value;
                if (IsValidID)
                    API.Instance.Overlay.BoxSetX(_id, _x);
            }
        }

        public int Y
        {
            get { return _y; }
            set
            {
                _y = value;
                if (IsValidID)
                    API.Instance.Overlay.BoxSetY(_id, _y);
            }
        }

        public int Width
        {
            get { return _width; }
            set
            {
                _width = value;
                if (IsValidID)
                    API.Instance.Overlay.BoxSetWidth(_id, _width);
            }
        }

        public int Height
        {
            get { return _height; }
            set
            {
                _height = value;
                if (IsValidID)
                    API.Instance.Overlay.BoxSetHeight(_id, _height);
            }
        }

        public bool Active
        {
            get { return active; }
            set
            {
                active = value;
                if (IsValidID)
                    API.Instance.Overlay.BoxSetActive(_id, active);
            }
        }

        public bool IsValidID
        {
            get { return _id != -1; }
        }

        public Box()
        {
            _id = -1;
            _color = 0xFFFFFFFF;
            _x = 0;
            _y = 0;
            active = false;
        }


        public void Validate()
        {
            if (!IsValidID)
            {
                _id = API.Instance.Overlay.BoxCreate();
                if (IsValidID)
                {
                    Color = _color;
                    X = _x;
                    Y = _y;
                    Width = _width;
                    Height = _height;
                    Active = active;
                }
                else
                    throw new ArgumentException();
            }
            else
                throw new ArgumentException();
        }

        public void Invalidate(bool explicitRemove = false)
        {
            if (explicitRemove && IsValidID)
                API.Instance.Overlay.BoxDelete(_id);

            _id = -1;
        }
    }
}