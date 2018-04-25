namespace CQRS.Core
{
    public interface IEventPublisher: IPublisher<IEvent>
    {
        void Publish(IEvent @event);
    }
}
