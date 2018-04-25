namespace CQRS.Core
{
    public interface ISender<T>
    {
        void Send(T command);
    }
}
