namespace DependencyInversionInjection
{
    public interface IDataStorage
    {
        void Persist(List<UserData> users);
    }
}