using shadowBasic.Components.Overlay.Natives;

namespace shadowBasic.Components.Overlay.Items
{
    public class OverlayItemBox : OverlayItem
    {
        private Box _overlay;

        public Box Overlay
        {
            get { return _overlay; }
        }

        public OverlayItemBox(KeybinderCore core)
            : base(core)
        {
            _overlay = new Box();
        }

        public override void Validate()
        {
            _overlay.Validate();
        }

        public override void Invalidate(bool explicitRemove = false)
        {
            if (_overlay != null)
                _overlay.Invalidate(explicitRemove);
        }

        public override void LoadSettings()
        {

        }

        public override void SaveSettings()
        {
        }

        public override void Update()
        {
        }
    }
}
