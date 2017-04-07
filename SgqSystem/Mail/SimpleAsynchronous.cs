using System;
using System.Net.Mail;
using System.ComponentModel;
using System.Net;

namespace Examples.SmptExamples.Async
{
    public class SimpleAsynchronous
    {
        static bool mailSent = false;
        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState;

            if (e.Cancelled)
            {
                Console.WriteLine("[{0}] Send canceled.", token);
            }
            if (e.Error != null)
            {
                Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
            }
            else
            {
                Console.WriteLine("Message sent.");
            }
            mailSent = true;
        }

        public static void SendMail()
        {


            //MailMessage Message = new MailMessage();
            //Message.IsBodyHtml = true;
            //Message.To.Add(new MailAddress(toEmail, toName));
            //Message.From = (new MailAddress(this.emailLogin, this.emailLoginName));
            //Message.Subject = this.subject;
            //Message.Body = this.messageBody;

            //SmtpClient sc = new SmtpClient();
            //sc.Port = 587;
            //sc.Host = "smtp.live.com";
            //sc.EnableSsl = true; // <-- ATENÇÃO.
            //sc.UseDefaultCredentials = false;
            //sc.Credentials = new NetworkCredential(this.emailLogin, this.pass);


            //sc.Send(Message);

            // Command line argument must the the SMTP host.
            SmtpClient client = new SmtpClient("smtp.live.com", 587);
            client.EnableSsl = true; //true Hotmail
            client.Credentials = new NetworkCredential("celsogea@hotmail.com", "Thebost1");

            // Specify the e-mail sender.
            // Create a mailing address that includes a UTF8 character
            // in the display name.
            MailAddress from = new MailAddress("celsogea@hotmail.com",
               "Jane " + (char)0xD8 + " Clayton",
            System.Text.Encoding.UTF8);
            // Set destinations for the e-mail message.
            MailAddress to = new MailAddress("celsogea@hotmail.com");

            // Specify the message content.
            MailMessage message = new MailMessage(from, to);
            message.Body = "This is a test e-mail message sent by an application. ";
            // Include some non-ASCII characters in body and subject.
            string someArrows = new string(new char[] { '\u2190', '\u2191', '\u2192', '\u2193' });
            message.Body += Environment.NewLine + someArrows;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.Subject = "test message 1" + someArrows;
            message.SubjectEncoding = System.Text.Encoding.UTF8;

            // Set the method that is called back when the send operation ends.
            client.SendCompleted += new
            SendCompletedEventHandler(SendCompletedCallback);

            // The userState can be any object that allows your callback 
            // method to identify this send operation.
            // For this example, the userToken is a string constant.
            string userState = "test message1";
            client.SendAsync(message, userState);
            
            //Console.WriteLine("Sending message... press c to cancel mail. Press any other key to exit.");
            //string answer = Console.ReadLine();

            // If the user canceled the send, and mail hasn't been sent yet,
            // then cancel the pending operation.
            //if (mailSent == false)
            //{
            //    client.SendAsyncCancel();
            //}

            // Clean up.
            //message.Dispose();
            //Console.WriteLine("Goodbye.");
        }
    }
}

