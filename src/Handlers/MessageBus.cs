using Domain;

namespace Handlers;

public class MessageBus : IMessageBus
{
    private IUnitOfWork _unitOfWork;
    private IMailService _mailService;

    public MessageBus(IUnitOfWork unitOfWork, IMailService mailService)
    {
        _unitOfWork = unitOfWork;
        _mailService = mailService;
    }

    public void Handle(Event @event)
    {
        DispatchDomainEvent(@event);

        foreach (var domainEvent in _unitOfWork.CollectNewEvents())
        {
            DispatchDomainEvent(domainEvent);
        }
    }

    private async void DispatchDomainEvent(Event @event)
    {
        switch (@event)
        {
            case OutOfStockEvent:
                new OutOfStockHandler(_mailService).Handle((OutOfStockEvent) @event);
                break;
            case BatchCreatedEvent:
                await new AddBatchHandler(_unitOfWork).Handle((BatchCreatedEvent) @event);
                break;
            case AllocationRequiredEvent:
                await new AllocateHandler(_unitOfWork).Handle((AllocationRequiredEvent) @event);
                break;
        }
    }
}