using Domain;

namespace Handlers;

public class MessageBus : IMessageBus
{
    private IMailService _mailService;

    public MessageBus(IMailService mailService)
    {
        _mailService = mailService;
    }

    public void DispatchDomainEvent(Event @event)
    {
        switch (@event)
        {
            case OutOfStockEvent:
                new SendOutOfStockNotification(_mailService).Handle((OutOfStockEvent) @event);
                break;
        }
    }
}