namespace shadowBasic.BasicAPI
{
    public interface IAPIPlayer
    {
        int GetHealth();
        int GetArmor();
        bool InInterior();
        bool InVehicle();
        bool IsDriver();
        float GetYaw();
        int GetInteriorId();
        void GetPosition(out float x, out float y, out float z);
        string GetCity();
        string GetDistrict();
        bool InRadius(float x, float y, float z, float radius);
    }
}
