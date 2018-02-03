using SAPI.SAMP;

namespace shadowBasic.BasicAPI.SAPI.SAMP
{
    internal class Vehicle : IAPISAMPVehicle
    {
        public void ToggleSiren(bool state)
        {
            VehicleAPI.Instance.ToggleSiren(state);
        }
    }
}
