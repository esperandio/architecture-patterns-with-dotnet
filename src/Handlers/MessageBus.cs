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

    public async Task Handle(IMessage @event)
    {
        await DispatchDomainEvent(@event);

        foreach (var domainEvent in _unitOfWork.CollectNewEvents())
        {
            await DispatchDomainEvent(domainEvent);
        }
    }

    private async Task DispatchDomainEvent(IMessage @event)
    {
        switch (@event)
        {
            case OutOfStockEvent:
                new OutOfStockHandler(_mailService).Handle((OutOfStockEvent) @event);
                break;
            case CreateBatchCommand:
                _results.Add(await new AddBatchHandler(_unitOfWork).Handle((CreateBatchCommand) @event));
                break;
            case AllocateCommand:
                _results.Add(await new AllocateHandler(_unitOfWork).Handle((AllocateCommand) @event));
                break;
            case ChangeBatchQuantityCommand:
                await new BatchQuantityChangedHandler(_unitOfWork).Handle((ChangeBatchQuantityCommand) @event);
                break;
        }
    }
}