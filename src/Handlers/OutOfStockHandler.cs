using Domain;

namespace Handlers;

class OutOfStockHandler
{
    private IMailService _mailService;

    public OutOfStockHandler(IMailService mailService)
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