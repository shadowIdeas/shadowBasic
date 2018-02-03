using SAPI;

namespace shadowBasic.BasicAPI.SAPI
{
    internal class Vehicle : IAPIVehicle
    {
        public float GetHealth()
        {
            return VehicleAPI.Instance.GetHealth();
        }

        public int GetModelId()
        {
            return VehicleAPI.Instance.GetModelID();
        }

        public float GetSpeed()
        {
            return VehicleAPI.Instance.GetSpeed();
        }

        public bool IsEngineRunning()
        {
            return VehicleAPI.Instance.IsEngineRunning();
        }

        public bool IsLightActive()
        {
            return VehicleAPI.Instance.IsLightActive();
        }

        public bool IsLocked()
        {
            return VehicleAPI.Instance.IsLocked();
        }

        public bool UseHorn()
        {
            return VehicleAPI.Instance.UseHorn();
        }

        public bool UseSiren()
        {
            return VehicleAPI.Instance.UseSiren();
        }
    }
}
