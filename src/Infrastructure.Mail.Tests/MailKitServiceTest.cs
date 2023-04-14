namespace Infrastructure.Mail.Tests;

public class MailKitServiceTest
{
    [Fact]
    public void TestSendEmail()
    {
        string username = Environment.GetEnvironmentVariable("SMTP_CREDENCIAL_USERNAME") ?? "";
        string password = Environment.GetEnvironmentVariable("SMTP_CREDENCIAL_PASSWORD") ?? "";
        string host = Environment.GetEnvironmentVariable("SMTP_HOST") ?? "";
        int port = Int32.Parse(Environment.GetEnvironmentVariable("SMTP_PORT") ?? "0");

        var mailService = new MailKitService(host, port, username, password);

        mailService.Send("mat.esperandio@gmail.com", "Título teste 1", "Conteúdo teste1");
        mailService.Send("mat.esperandio@gmail.com", "Título teste 2", "Conteúdo teste2");
    }
}