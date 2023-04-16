using Domain;

namespace UseCases;

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
                SendOutOfStockNotification((OutOfStockEvent) @event);
                break;
        }
    }

    private void SendOutOfStockNotification(OutOfStockEvent outOfStockEvent)
    {
        _mailService.Send(
            "mat.esperandio@gmail.com", 
            "Out of stock",
            $"Out of stock for {outOfStockEvent.Sku}"
        );
    }
}