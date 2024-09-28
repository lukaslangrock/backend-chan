using System.Net.Mail;

namespace backend;

public class EmailRickRoll()
{
    public static void RickRoll(string shouldBeAnEmail)
    {
        string contentHTML = "Hi user, please verify your email by clicking <a href=\"https://www.youtube.com/watch?v=dQw4w9WgXcQ\">here</a> in order to activate your account.";
        var dieter = new SmtpClient("smtp.tem.scw.cloud", 25);
        dieter.UseDefaultCredentials = false;
        dieter.Credentials = new System.Net.NetworkCredential("yourusername", "yourpassword");
        dieter.Send("no-reply@libre.moe", shouldBeAnEmail, "Very your Email", contentHTML);
    }
}
