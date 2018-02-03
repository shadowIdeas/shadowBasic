using SAPI;

namespace shadowBasic.BasicAPI.SAPI
{
    internal class Player : IAPIPlayer
    {
        public int GetArmor()
        {
            return PlayerAPI.Instance.GetArmor();
        }

        public string GetCity()
        {
            return PlayerAPI.Instance.GetCity();
        }

        public string GetDistrict()
        {
            return PlayerAPI.Instance.GetDistrict();
        }

        public int GetHealth()
        {
            return PlayerAPI.Instance.GetHealth();
        }

        public int GetInteriorId()
        {
            return PlayerAPI.Instance.GetInteriorId();
        }

        public void GetPosition(out float x, out float y, out float z)
        {
            PlayerAPI.Instance.GetPosition(out x, out y, out z);
        }

        public float GetYaw()
        {
            return PlayerAPI.Instance.GetYaw();
        }

        public bool InInterior()
        {
            return PlayerAPI.Instance.InInterior();
        }

        public bool InRadius(float x, float y, float z, float radius)
        {
            return PlayerAPI.Instance.InRadius(x, y, z, radius);
        }

        public bool InVehicle()
        {
            return PlayerAPI.Instance.InVehicle();
        }

        public bool IsDriver()
        {
            return PlayerAPI.Instance.IsDriver();
        }
    }
}
