using FromLocalsToLocals.Utilities.Helpers;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace FromLocalsToLocals.Utilities
{
    public static  class SendException
    {
        public static async Task ExceptionSender(this Exception e)
        {
            var key = Config.Send_Grid_Key;
            var client = new SendGridClient(key);

            var from = new EmailAddress("fromlocalstolocals@gmail.com", "Exception message");
            var subject = "Exception receiver";
            var to = new EmailAddress("lukasstc223@gmail.com", "Dear User");
            var plainTextContent = "We have this exception:";


            var htmlContent = "<!DOCTYPE html><html><head><meta charset=\"UTF-8\"></head><body> " + e.ToString();
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }

    }
}