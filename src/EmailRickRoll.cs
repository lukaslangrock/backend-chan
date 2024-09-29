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
            var dieter = new SmtpClient("smtp.tem.scw.cloud", 587);
            dieter.EnableSsl = true;
            dieter.UseDefaultCredentials = false;
            var creds = new System.Net.NetworkCredential(Environment.GetEnvironmentVariable("SMTP_USER"), Environment.GetEnvironmentVariable("SMTP_PASS"));
            dieter.Credentials = creds;
            Console.WriteLine("[Email] ENV SMTP_USER: " + Environment.GetEnvironmentVariable("SMTP_USER"));
            Console.WriteLine("[Email] ENV SMTP_PASS: " + Environment.GetEnvironmentVariable("SMTP_PASS"));
            Console.WriteLine("[Email] Using SMTP_USER: " + creds.UserName);
            Console.WriteLine("[Email] Using SMTP_PASS: " + creds.Password);
            dieter.Send("no-reply@libre.moe", shouldBeAnEmail, "Very your Email", contentHTML);
            Console.WriteLine("[Email] \"Verification\" email was sent to email: " + shouldBeAnEmail);
        }
        catch (Exception ex)
        {
            Console.WriteLine("[Email] Error sending email: " + ex.ToString());
        }

    }
}
