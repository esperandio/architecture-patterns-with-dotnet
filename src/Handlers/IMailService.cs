namespace Handlers;

public interface IMailService
{
    public void Send(string to, string title, string message);
}
