using SAPI;

namespace shadowBasic.BasicAPI.SAPI
{
    public class SAPI : API
    {
        public SAPI()
        {
            Chat = new Chat();
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
