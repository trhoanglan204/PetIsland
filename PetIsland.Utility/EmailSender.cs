using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

//using SendGrid.Helpers.Mail;
//using SendGrid;

#pragma warning disable IDE0290

namespace PetIsland.Utility;

public class EmailSender : IEmailSender
{
    //private string SendGridSecret { get; set; }

    //public EmailSender(IConfiguration _config)
    //{
    //    SendGridSecret = _config.GetValue<string>("SendGrid:SecretKey")!;
    //}

    //public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    //{
    //    var client = new SendGridClient(SendGridSecret);

    //    var from = new EmailAddress("PetIslandSupport@gmail.com", "Pet Island Support Service");
    //    var to = new EmailAddress(email);
    //    var message = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);

    //    await client.SendEmailAsync(message);
    //}

    private string SMTP_Email { get; set; }
    private string SMTP_Password { get; set; }

    public EmailSender(IConfiguration config)
    {
        SMTP_Email = config.GetValue<string>("GmailSMTP:Email")!;
        SMTP_Password = config.GetValue<string>("GmailSMTP:AppPassword")!;
    }
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {

        var client = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(SMTP_Email, SMTP_Password)
        };

        var msg = new MailMessage 
        { 
            From =  new MailAddress(SMTP_Email),
            Body = htmlMessage,
            IsBodyHtml = true,
            Subject = subject
        };
        msg.To.Add(new MailAddress(email));
        return client.SendMailAsync(msg);
    }
}
