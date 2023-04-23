namespace Handlers.Tests.Doubles;

class FakeMailService : IMailService
{
    public void Send(string to, string title, string message)
    {
        return;
    }
}