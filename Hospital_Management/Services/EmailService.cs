using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;
using Hospital_Management.Models;
using Hospital_Management.Services.Iservice;

public class EmailService : Iemail
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendAppointmentEmailAsync(string toEmail, string patientName, DateTime appointmentDate, string doctorName)
    {
        var email = new MimeMessage();
        email.Sender = MailboxAddress.Parse(_emailSettings.SenderEmail);
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = $"Your Appointment with Dr. {doctorName}";

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = $@"
                <h3>Dear {patientName},</h3>
                <p>Your appointment has been scheduled with the following details:</p>
                <ul>
                    <li><strong>Doctor:</strong> Dr. {doctorName}</li>
                    <li><strong>Date & Time:</strong> {appointmentDate:f}</li>
                </ul>
                <p>Thank you for choosing our hospital.</p>"
        };

        email.Body = bodyBuilder.ToMessageBody();


        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.Password);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}
