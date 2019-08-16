using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace StationeryStore_ADTeam11.Util
{
    public class Email
    {
        public void SendEmail()
        {

        }

        public void SendEmail(string receiver, string subject, string message)
        {
            try
            {
                {
                    var senderEmail = new MailAddress("isslaps.hr@gmail.com", "ISS Laps");
                    var receiverEmail = new MailAddress(receiver, "Receiver");

                    var password = "laps12345";
                    var sub = subject;
                    var body = message;
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = subject,
                        Body = body
                    })
                    {
                        smtp.Send(mess);
                    }

                }
            }
            catch (Exception)
            {

            }

        }
    }
}