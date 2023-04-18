namespace Handlers.Tests;

class FakeMailService : IMailService
{
    public void Send(string to, string title, string message)
    {
        return;
    }
}