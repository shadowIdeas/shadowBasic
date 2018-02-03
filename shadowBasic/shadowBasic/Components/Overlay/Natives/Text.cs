using shadowBasic.BasicAPI;
using System;

namespace shadowBasic.Components.Overlay.Natives
{
    public class Text
    {
        private readonly KeybinderCore _core;
        private readonly OverlayComponent _overlayComponent;

        private int _id;
        private string _value;
        private string _oldValue;

        private string _valueBeforeEdit;
        private string _exampleValue;

        private uint _color;
        private int _size;
        private int _x;
        private int _y;

        private bool _active;
        private bool _useMaxWidth;
        private bool _useMaxHeight;

        private int _maxWidth;
        private int _maxHeight;

        public int ID
        {
            get { return _id; }
        }

        public string Value
        {
            get { return _value; }
            set
            {
                if (!_overlayComponent.EditActive || (_overlayComponent.EditActive && value != String.Empty))
                {
                    _value = value;

                    if (IsValidID)
                    {
                        if (_oldValue != _value)
                        {
                            _oldValue = value;
                            API.Instance.Overlay.TextSetText(_id, _value);
                        }
                    }
                }
            }
        }

        public string ExampleValue
        {
            get { return _exampleValue; }
            set { _exampleValue = value; }
        }

        public uint Color
        {
            get { return _color; }
            set
            {
                _color = value;
                if (IsValidID)
                    API.Instance.Overlay.TextSetColor(_id, _color);
            }
        }

        public int Size
        {
            get { return _size; }
            set
            {
                _size = value;
                if (IsValidID)
                    API.Instance.Overlay.TextSetSize(_id, _size);
            }
        }

        public int X
        {
            get { return _x; }
            set
            {
                _x = value;
                if (IsValidID)
                    API.Instance.Overlay.TextSetX(_id, _x);
            }
        }

        public int Y
        {
            get { return _y; }
            set
            {
                _y = value;
                if (IsValidID)
                    API.Instance.Overlay.TextSetY(_id, _y);
            }
        }

        public int Width
        {
            get
            {
                int width = 0;
                int height = 0;
                if (IsValidID)
                    API.Instance.Overlay.TextGetTextExtent(_id, out width, out height);

                return width;
            }
        }

        public int Height
        {
            get
            {
                int width = 0;
                int height = 0;
                if (IsValidID)
                    API.Instance.Overlay.TextGetTextExtent(_id, out width, out height);

                return height;
            }
        }

        public bool Active
        {
            get { return _active; }
            set
            {
                _active = value;
                if (IsValidID)
                    API.Instance.Overlay.TextSetActive(_id, _active);
            }
        }

        public bool UseMaxWidth
        {
            get { return _useMaxWidth; }
            set
            {
                _useMaxWidth = value;
                if (IsValidID)
                    API.Instance.Overlay.TextSetUseMaxWidth(_id, _useMaxWidth);
            }
        }

        public bool UseMaxHeight
        {
            get { return _useMaxHeight; }
            set
            {
                _useMaxHeight = value;
                if (IsValidID)
                    API.Instance.Overlay.TextSetUseMaxHeight(_id, _useMaxHeight);
            }
        }

        public int MaxWidth
        {
            get { return _maxWidth; }
            set
            {
                _maxWidth = value;
                if (IsValidID)
                    API.Instance.Overlay.TextSetMaxWidth(_id, _maxWidth);
            }
        }

        public int MaxHeight
        {
            get { return _maxHeight; }
            set
            {
                _maxHeight = value;
                if (IsValidID)
                    API.Instance.Overlay.TextSetMaxHeight(_id, _maxHeight);
            }
        }

        public bool IsValidID
        {
            get { return _id != -1; }
        }

        public Text(KeybinderCore core)
        {
            _core = core;
            _overlayComponent = core.GetComponent<OverlayComponent>();

            _id = -1;
            _value = String.Empty;
            _oldValue = String.Empty;
            _valueBeforeEdit = String.Empty;
            _exampleValue = String.Empty;
            _color = 0xFFFFFFFF;
            _size = 12;
            _x = 0;
            _y = 0;
            _active = false;
            _useMaxWidth = false;
            _useMaxHeight = false;

            _maxWidth = 0;
            _maxHeight = 0;
        }

        internal void Validate()
        {
            if (!IsValidID)
            {
                _id = API.Instance.Overlay.TextCreate();
                if (IsValidID)
                {
                    Value = _value;
                    Color = _color;
                    Size = _size;
                    X = _x;
                    Y = _y;
                    Active = _active;
                    UseMaxWidth = _useMaxWidth;
                    UseMaxHeight = _useMaxHeight;
                    MaxWidth = _maxWidth;
                    MaxHeight = _maxHeight;
                }
                else
                    throw new ArgumentException();
            }
            else
                throw new ArgumentException();
        }

        internal void Invalidate(bool explicitRemove = false)
        {
            if (explicitRemove && IsValidID)
                API.Instance.Overlay.TextDelete(_id);

            _id = -1;
            _oldValue = String.Empty;
        }

        internal void SwitchEditMode(bool edit)
        {
            if (edit && _exampleValue != String.Empty)
            {
                _valueBeforeEdit = _value;
                Value = _exampleValue;
            }
            else if (!edit && Value == _exampleValue)
            {
                Value = _valueBeforeEdit;
            }
        }
    }
}
