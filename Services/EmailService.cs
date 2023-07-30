using System.Net;
using System.Net.Mail;

namespace Blog.Services;

public class EmailService
{
    public bool Send(
        string toName,
        string toEmail,
        string subject,
        string body,
        string fromName = "Gleiton",
        string fromEmail = "gestao@farid.com.br")
    {
        var smtpClient = new SmtpClient(Configuration.Smtp.Host, Configuration.Smtp.Port)
        {
            Credentials = new NetworkCredential(Configuration.Smtp.UserName, Configuration.Smtp.Password),
            DeliveryMethod = SmtpDeliveryMethod.Network,
            EnableSsl = true
        };

        var mail = new MailMessage()
        {
            From = new MailAddress(fromEmail, fromName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
        };

        mail.To.Add(new MailAddress(toEmail, toName));


        try
        {
            smtpClient.Send(mail);
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
            return false;
        }







        return true;
    }
}