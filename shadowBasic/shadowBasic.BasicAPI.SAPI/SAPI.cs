using SAPI;

namespace shadowBasic.BasicAPI.SAPI
{
    public class SAPI : API
    {
        public SAPI()
        {
            Overlay = new Overlay();
            Chat = new Chat();
            Dialog = new Dialog();
            Player = new Player();
            Vehicle = new Vehicle();
            SAMPPlayer = new SAMP.Player();
            SAMPVehicle = new SAMP.Vehicle();
        }

        public override void Initialize()
        {
            GeneralAPI.Instance.Initialize();
        }

        public override void Uninitialize()
        {
            GeneralAPI.Instance.ResetInitialize();
        }
    }
}
