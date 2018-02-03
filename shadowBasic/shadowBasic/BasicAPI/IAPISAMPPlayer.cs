namespace shadowBasic.BasicAPI
{
    public interface IAPISAMPPlayer
    {
        string GetFullName(string name);
        int GetIdByName(string name);
        string GetNameById(int id);
        int GetLocalId();
        string GetLocalName();
    }
}
