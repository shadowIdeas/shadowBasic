namespace shadowBasic.BasicAPI
{
    public interface IAPIVehicle
    {
        float GetHealth();
        int GetModelId();
        float GetSpeed();
        bool IsEngineRunning();
        bool IsLightActive();
        bool IsLocked();
        bool UseHorn();
        bool UseSiren();
    }
}
