using System.Net;
using System.Net.Mail;

namespace BTLWeb.Constants
{
    public class SendMail
    {
        public static bool SendEmail(string to, string subject, string body, string attachFile)
        {
            try
            {
                MailMessage msg = new MailMessage(CommonConstants.emailSender, to, subject, body);

                using(var client = new SmtpClient(CommonConstants.hostEmail, CommonConstants.portEmail))
                {
                    client.EnableSsl = true;
                
                    if(!string.IsNullOrEmpty(attachFile))
                    {
                        Attachment attachment = new Attachment(attachFile);
                        msg.Attachments.Add(attachment);
                    }
                    
                    NetworkCredential credential = new NetworkCredential(CommonConstants.emailSender, CommonConstants.passwordSenderr);
                    client.UseDefaultCredentials = true;
                    client.Credentials = credential;
                    client.Send(msg);

                }
            } catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
