using System.Net.Mail;

namespace backend;

public class EmailRickRoll()
{
    public static void RickRoll(string shouldBeAnEmail)
    {
        try
        {
            Console.WriteLine("[Email] Trying to send email...");

            string contentHTML = "Hi user, please verify your email by clicking <a href=\"https://www.youtube.com/watch?v=dQw4w9WgXcQ\">here</a> in order to activate your account.";
            var message = new MailMessage("no-reply@libre.moe", shouldBeAnEmail, "Very your Email", contentHTML);
            message.IsBodyHtml = true;
            var creds = new System.Net.NetworkCredential(Environment.GetEnvironmentVariable("SMTP_USER"), Environment.GetEnvironmentVariable("SMTP_PASS"));
            var dieter = new SmtpClient("smtp.tem.scw.cloud", 587);
            dieter.UseDefaultCredentials = false;
            dieter.Credentials = creds;
            dieter.EnableSsl = true;
            dieter.Timeout = 5000;
            dieter.Send(message);

            Console.WriteLine("[Email] \"Verification\" email was sent to " + shouldBeAnEmail);
        }
        catch (Exception ex)
        {
            Console.WriteLine("[Email] Error sending email: " + ex.ToString());
        }

    }
}
