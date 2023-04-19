using Domain;

namespace Handlers;

public class MessageBus : IMessageBus
{
    private IUnitOfWork _unitOfWork;
    private IMailService _mailService;
    private readonly List<string> _results;

    public IReadOnlyCollection<string> Results => _results.AsReadOnly();

    public MessageBus(IUnitOfWork unitOfWork, IMailService mailService)
    {
        _unitOfWork = unitOfWork;
        _mailService = mailService;
        _results = new List<string>();
    }

    public async Task Handle(Event @event)
    {
        await DispatchDomainEvent(@event);

        foreach (var domainEvent in _unitOfWork.CollectNewEvents())
        {
            await DispatchDomainEvent(domainEvent);
        }
    }

    private async Task DispatchDomainEvent(Event @event)
    {
        switch (@event)
        {
            case OutOfStockEvent:
                new OutOfStockHandler(_mailService).Handle((OutOfStockEvent) @event);
                break;
            case BatchCreatedEvent:
                _results.Add(await new AddBatchHandler(_unitOfWork).Handle((BatchCreatedEvent) @event));
                break;
            case AllocationRequiredEvent:
                _results.Add(await new AllocateHandler(_unitOfWork).Handle((AllocationRequiredEvent) @event));
                break;
            case BatchQuantityChangedEvent:
                await new BatchQuantityChangedHandler(_unitOfWork).Handle((BatchQuantityChangedEvent) @event);
                break;
        }
    }
}