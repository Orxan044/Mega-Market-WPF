using System.Net.Mail;
using System.Net;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using System.Windows;
using ToastNotifications.Messages;
using System.IO;
namespace Mega_Market_App.Services.Mail;

public class MailServices
{
    public static void SendMail(string to, string subject, string body, string attachmentPath = null)
    {
        #region Notifier
        ToastNotifications.Notifier notifier = new ToastNotifications.Notifier(cfg =>
        {
            cfg.PositionProvider = new WindowPositionProvider(
                parentWindow: Application.Current.MainWindow,
                corner: Corner.TopLeft,
                offsetX: 5,
                offsetY: 5);

            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromSeconds(2),
                maximumNotificationCount: MaximumNotificationCount.FromCount(1));

            cfg.Dispatcher = Application.Current.Dispatcher;
        });
        #endregion

        var smtp = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential("logmanquliyev33@gmail.com", "rykn paby iqnf madd"),
            EnableSsl = true,
        };

        var mailMessage = new MailMessage()
        {
            Subject = subject,
            Body = body,
            From = new MailAddress("logmanquliyev33@gmail.com"),
            IsBodyHtml = true,
        };

        mailMessage.To.Add(to);

        // PDF dosyasını e-posta ekine ekle
        if (!string.IsNullOrEmpty(attachmentPath))
        {
            if (File.Exists(attachmentPath))
            {
                Attachment pdfAttachment = new Attachment(attachmentPath);
                mailMessage.Attachments.Add(pdfAttachment);
            }
            else
            {
                notifier.ShowError("The attachment file does not exist.");
                return;
            }
        }

        try
        {
            smtp.Send(mailMessage);
        }
        catch (Exception)
        {
            notifier.ShowError("Something went wrong with the mail. Please try again.");
        }
    }
}
