using Domain;
using Handlers.Abstractions;

namespace Handlers;

public class MessageBus : IMessageBus
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMailService _mailService;
    private readonly IMessageBroker _messageBroker;
    private readonly List<string> _results;

    public IReadOnlyCollection<string> Results => _results.AsReadOnly();

    public MessageBus(IUnitOfWork unitOfWork, IMailService mailService, IMessageBroker messageBroker)
    {
        _unitOfWork = unitOfWork;
        _mailService = mailService;
        _messageBroker = messageBroker;
        _results = new List<string>();
    }

    public async Task Handle(Command command)
    {
        await DispatchCommand(command);

        foreach (var domainMessage in _unitOfWork.CollectNewMessages())
        {
            if (domainMessage is Event @event)
            {
                DispatchDomainEvent(@event);
            }

            if (domainMessage is Command command1)
            {
                await DispatchCommand(command1);
            }
        }
    }

    private void DispatchDomainEvent(Event @event)
    {
        switch (@event)
        {
            case OutOfStockEvent:
                new OutOfStockHandler(_mailService).Handle((OutOfStockEvent) @event);
                break;
            case AllocatedEvent:
                new PublishAllocatedEventHandler(_messageBroker).Handle((AllocatedEvent) @event);
                break;
        }
    }

    private async Task DispatchCommand(Command command)
    {
        switch (command)
        {
            case CreateBatchCommand:
                _results.Add(await new AddBatchHandler(_unitOfWork).Handle((CreateBatchCommand) command));
                break;
            case AllocateCommand:
                _results.Add(await new AllocateHandler(_unitOfWork).Handle((AllocateCommand) command));
                break;
            case ChangeBatchQuantityCommand:
                await new BatchQuantityChangedHandler(_unitOfWork).Handle((ChangeBatchQuantityCommand) command);
                break;
        }
    }
}