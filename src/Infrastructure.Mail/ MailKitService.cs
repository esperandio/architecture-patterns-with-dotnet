using MailKit.Net.Smtp;
using MimeKit;

namespace Infrastructure.Mail;

public class MailKitService : IDisposable
{
    private readonly MailboxAddress _fromAddress;
    private readonly SmtpClient _smtpClient;

    public MailKitService(string host, int port, string username, string password)
    {
        _smtpClient = new SmtpClient();

        _smtpClient.Connect(host, port);
        _smtpClient.Authenticate(username, password);

        _fromAddress = new MailboxAddress("", username);
    }

    public void Dispose()
    {
        _smtpClient.Disconnect(true);
    }

    public void Send(string to, string title, string message)
    {
        var toAddress = new MailboxAddress("", to);

        var mimeMessage = new MimeMessage();

        mimeMessage.From.Add(_fromAddress);
        mimeMessage.To.Add(toAddress);
        mimeMessage.Subject = title;
        mimeMessage.Body = new TextPart("plain") 
        {
            Text = message
        };

        _smtpClient.Send(mimeMessage);
    }
}
