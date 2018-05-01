namespace Common
{
    public interface IReceivedHandler<T> where T : class
    {
        void Process(T t);
    }
}
