namespace shadowBasic.BasicAPI
{
    public abstract class API
    {
        public static API Instance { get; internal set; }

        public IAPIChat Chat { get; set; }
        public IAPIDialog Dialog { get; set; }
        public IAPIPlayer Player { get; set; }
        public IAPIVehicle Vehicle { get; set; }
        public IAPISAMPPlayer SAMPPlayer { get; set; }
        public IAPISAMPVehicle SAMPVehicle { get; set; }

        /// <summary>
        /// Get called when <see cref="KeybinderCore"/> find an process.
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Get called when <see cref="KeybinderCore"/> lose an process.
        /// </summary>
        public abstract void Uninitialize();
    }
}
