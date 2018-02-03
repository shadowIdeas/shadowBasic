using shadowBasic.Components.Overlay.Natives;
using shadowBasic.Components.Settings;

namespace shadowBasic.Components.Overlay.Items
{
    public abstract class OverlayItemText : OverlayItem
    {
        private string _identifier;
        private Natives.Text _overlay;
        private Box _boundingBox;

        public string Identifier
        {
            get { return _identifier; }
        }

        public Natives.Text Overlay
        {
            get { return _overlay; }
        }

        public Box BoundingBox
        {
            get { return _boundingBox; }
        }

        public string ColorString
        {
            get { return "{" + _overlay.Color.ToString("X8") + "}"; }
        }

        public OverlayItemText(KeybinderCore core, string identifier)
            : base(core)
        {
            _identifier = identifier;
            _overlay = new Natives.Text(core);
            _boundingBox = new Box();
        }

        public override void Validate()
        {
            _overlay.Validate();
            _boundingBox.Validate();
        }

        public override void Invalidate(bool explicitRemove = false)
        {
            if (_overlay != null)
                _overlay.Invalidate(explicitRemove);

            if (_boundingBox != null)
                _boundingBox.Invalidate(explicitRemove);
        }

        public override void LoadSettings()
        {
            var settingComponent = Core.GetComponent<SettingComponent>();
            if (settingComponent == null)
                return;

            _overlay.Active = settingComponent.GetPermanentSetting<bool>($"OverlayItemText{_identifier}Active");
            _overlay.Color = (uint)settingComponent.GetPermanentSetting<int>($"OverlayItemText{_identifier}Color");
            _overlay.Size = settingComponent.GetPermanentSetting<int>($"OverlayItemText{_identifier}Size");
            _overlay.X = settingComponent.GetPermanentSetting<int>($"OverlayItemText{_identifier}X");
            _overlay.Y = settingComponent.GetPermanentSetting<int>($"OverlayItemText{_identifier}Y");
            _overlay.UseMaxWidth = settingComponent.GetPermanentSetting<bool>($"OverlayItemText{_identifier}UseMaxWidth");
            _overlay.UseMaxHeight = settingComponent.GetPermanentSetting<bool>($"OverlayItemText{_identifier}UseMaxHeight");
            _overlay.MaxWidth = settingComponent.GetPermanentSetting<int>($"OverlayItemText{_identifier}MaxWidth");
            _overlay.MaxHeight = settingComponent.GetPermanentSetting<int>($"OverlayItemText{_identifier}MaxHeight");
        }

        public override void SaveSettings()
        {
            var settingComponent = Core.GetComponent<SettingComponent>();
            if (settingComponent == null)
                return;

            settingComponent.SetPermanentSetting<bool>($"OverlayItemText{_identifier}Active", _overlay.Active);
            settingComponent.SetPermanentSetting<int>($"OverlayItemText{_identifier}Color", (int)_overlay.Color);
            settingComponent.SetPermanentSetting<int>($"OverlayItemText{_identifier}Size", _overlay.Size);
            settingComponent.SetPermanentSetting<int>($"OverlayItemText{_identifier}X", _overlay.X);
            settingComponent.SetPermanentSetting<int>($"OverlayItemText{_identifier}Y", _overlay.Y);
            settingComponent.SetPermanentSetting<bool>($"OverlayItemText{_identifier}UseMaxWidth", _overlay.UseMaxWidth);
            settingComponent.SetPermanentSetting<bool>($"OverlayItemText{_identifier}UseMaxHeight", _overlay.UseMaxHeight);
            settingComponent.SetPermanentSetting<int>($"OverlayItemText{_identifier}MaxWidth", _overlay.MaxWidth);
            settingComponent.SetPermanentSetting<int>($"OverlayItemText{_identifier}MaxHeight", _overlay.MaxHeight);
        }
    }
}
