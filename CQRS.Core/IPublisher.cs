namespace CQRS.Core
{
    public interface IPublisher<T>
    {
        void Publish(T @event);
    }
}
