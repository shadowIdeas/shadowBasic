using SAPI.SAMP;

namespace shadowBasic.BasicAPI.SAPI.SAMP
{
    internal class Player : IAPISAMPPlayer
    {
        public string GetFullName(string name)
        {
            return PlayerAPI.Instance.GetFullName(name);
        }

        public int GetIdByName(string name)
        {
            return PlayerAPI.Instance.GetIDByName(name);
        }

        public int GetLocalId()
        {
            return PlayerAPI.Instance.GetLocalID();
        }

        public string GetLocalName()
        {
            return PlayerAPI.Instance.GetLocalName();
        }

        public string GetNameById(int id)
        {
            return PlayerAPI.Instance.GetNameByID(id);
        }
    }
}
