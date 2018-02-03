using shadowBasic.Components.Overlay.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace shadowBasic.Components.Overlay
{
    public class OverlayComponent : Component
    {
        private readonly bool _ingameEditing;
        private readonly List<OverlayItem> _overlays;

        private MouseHook _mouseHook;
        private bool _editActive;

        private Task _updateTask;
        private volatile bool _updateTaskRunning;

        public List<OverlayItem> Overlays
        {
            get { return _overlays; }
        }

        public bool EditActive
        {
            get { return _editActive; }
            set
            {
                if(!_ingameEditing)
                {
                    _editActive = false;
                    return;
                }

                _editActive = value;

                if (_editActive)
                {
                    _mouseHook.Start();
                }
                else
                {
                    _mouseHook.Stop();
                    SaveSettings();
                }

                foreach (var item in _overlays)
                {
                    if (item.GetType().IsSubclassOf(typeof(OverlayItemText)))
                    {
                        var textOverlay = item as OverlayItemText;
                        if (textOverlay.Overlay.Active)
                        {
                            textOverlay.BoundingBox.Color = 0x66000000;
                            textOverlay.BoundingBox.Active = value;
                            textOverlay.Overlay.SwitchEditMode(value);
                        }
                    }
                }
            }
        }

        public OverlayComponent(KeybinderCore core, bool ingameEditing)
            : base(core)
        {
            _ingameEditing = ingameEditing;
            _overlays = new List<OverlayItem>();

            _editActive = false;
        }

        public override void Start()
        {
            _mouseHook = new MouseHook(Core);

            ReloadSettings();

            _updateTaskRunning = true;
            _updateTask = new Task(UpdateProcedure, TaskCreationOptions.LongRunning);
            _updateTask.Start();
        }

        public override void Stop()
        {
            _mouseHook.Stop();

            _updateTaskRunning = false;
            _updateTask.Wait();

            InvalidateAll(true);
        }

        protected override void InitializeAssembly(Assembly assembly)
        {
            var items = assembly.GetTypes().
                Where(i => i.IsSubclassOf(typeof(OverlayItem)) && !i.IsAbstract).
                ToList();

            foreach (var item in items)
                _overlays.Add(Activator.CreateInstance(item, Core) as OverlayItem);
        }

        public override void ProcessStarted()
        {
            ValidateAll();
        }

        public override void ProcessStopped()
        {
            InvalidateAll();
        }

        public T GetOverlay<T>() where T : OverlayItem
        {
            foreach (var item in _overlays)
            {
                if (item.GetType() == typeof(T))
                    return (T)item;
            }

            return default(T);
        }

        public void ReloadSettings()
        {
            foreach (var item in _overlays)
                item.LoadSettings();
        }

        public void SaveSettings()
        {
            foreach (var item in _overlays)
                item.SaveSettings();
        }

        private void ValidateAll()
        {
            foreach (var item in _overlays)
                item.Validate();
        }

        private void InvalidateAll(bool explicitRemove = false)
        {
            foreach (var item in _overlays)
                item.Invalidate(explicitRemove);
        }

        private void UpdateProcedure()
        {
            while (_updateTaskRunning)
            {
                foreach (var item in _overlays)
                {
                    if (_editActive && item.GetType().IsSubclassOf(typeof(OverlayItemText)))
                    {
                        var textOverlay = item as OverlayItemText;
                        textOverlay.BoundingBox.X = textOverlay.Overlay.X;
                        textOverlay.BoundingBox.Y = textOverlay.Overlay.Y;

                        if (textOverlay.Overlay.Value != String.Empty)
                        {
                            if (textOverlay.Overlay.UseMaxWidth)
                                textOverlay.BoundingBox.Width = textOverlay.Overlay.MaxWidth;
                            else
                                textOverlay.BoundingBox.Width = textOverlay.Overlay.Width;

                            if (textOverlay.Overlay.UseMaxHeight)
                                textOverlay.BoundingBox.Height = textOverlay.Overlay.MaxHeight;
                            else
                                textOverlay.BoundingBox.Height = textOverlay.Overlay.Height;
                        }
                        else
                        {
                            textOverlay.BoundingBox.Width = 0;
                            textOverlay.BoundingBox.Height = 0;
                        }
                    }

                    item.Update();
                }

                Thread.Sleep(TimeSpan.FromMilliseconds(50.0));
            }
        }
    }
}
