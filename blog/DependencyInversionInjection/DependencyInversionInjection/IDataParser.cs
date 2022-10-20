namespace DependencyInversionInjection
{
    public interface IDataParser
    {
        List<UserData> Parse(List<string> data);
    }
}