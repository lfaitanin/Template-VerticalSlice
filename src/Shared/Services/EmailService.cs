using System.Net;
using System.Net.Mail;

namespace Shared.Services;

public class EmailService : IEmailService
{
    private readonly string _smtpHost;
    private readonly int _smtpPort;
    private readonly string _username;
    private readonly string _password;
    private readonly string _fromEmail;
    private readonly string _fromName;

    public EmailService(string smtpHost, int smtpPort, string username, string password, string fromEmail, string fromName)
    {
        _smtpHost = smtpHost;
        _smtpPort = smtpPort;
        _username = username;
        _password = password;
        _fromEmail = fromEmail;
        _fromName = fromName;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var smtpClient = new SmtpClient(_smtpHost)
        {
            Port = _smtpPort,
            Credentials = new NetworkCredential(_username, _password),
            EnableSsl = true // SSL ativo por padrão
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_fromEmail, _fromName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true // Suporte para HTML
        };

        mailMessage.To.Add(toEmail);

        try
        {
            await smtpClient.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao enviar e-mail: {ex.Message}");
        }
    }
}
