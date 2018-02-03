namespace shadowBasic.Components.Overlay.Items
{
    public abstract class OverlayItem
    {
        private readonly KeybinderCore _core;

        protected KeybinderCore Core
        {
            get { return _core; }
        }

        public OverlayItem(KeybinderCore core)
        {
            _core = core;
        }

        public abstract void Validate();
        public abstract void Invalidate(bool explicitRemove = false);
        public abstract void Update();
        public abstract void LoadSettings();
        public abstract void SaveSettings();
    }
}
