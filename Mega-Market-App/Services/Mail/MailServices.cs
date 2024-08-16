using System.Net.Mail;
using System.Net;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using System.Windows;
using ToastNotifications.Messages;
namespace Mega_Market_App.Services.Mail;

public class MailServices
{
    public static void SendMail(string to, string subject, string body)
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
            Credentials = new NetworkCredential("n.resul11@gmail.com", "ljgb fqte vzss ncxk"),
            //Accounts Password - Go To Google Account
            EnableSsl = true,
        };

        var mailMessage = new MailMessage()
        {
            Subject = subject,
            Body = body,
            From = new MailAddress("n.resul11@gmail.com"),
            IsBodyHtml = true,
        };

        mailMessage.To.Add(to);

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
