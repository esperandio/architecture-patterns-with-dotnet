using Domain;

namespace Handlers;

class SendOutOfStockNotification
{
    private IMailService _mailService;

    public SendOutOfStockNotification(IMailService mailService)
    {
        _mailService = mailService;
    }

    public void Handle(OutOfStockEvent outOfStockEvent)
    {
        _mailService.Send(
            "mat.esperandio@gmail.com", 
            "Out of stock",
            $"Out of stock for {outOfStockEvent.Sku}"
        );
    }
}