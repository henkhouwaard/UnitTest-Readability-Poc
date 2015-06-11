namespace UnitTestReadabilityPoc.Interfaces
{
    public interface IRepository
    {
        void SaveString(string stringToSave);
        string GetStringById(int id);
    }
}