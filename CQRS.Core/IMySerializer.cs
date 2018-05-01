namespace CQRS.Core
{
    public interface IMySerializer
    {
        string Serializ(object value);
        T DeserializeObject<T>(string value);
    }
}