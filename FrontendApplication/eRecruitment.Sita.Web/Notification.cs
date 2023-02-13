using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace eRecruitment.Sita.Web
{
    public class Notification
    {
        public bool SendEmail(string To, string Subject, string BodyMessage)
        {
            bool Status = true;
            try

            {
                Task t = Task.Run(async () =>
                {
                    // You should use using block so .NET can clean up resources
                    using (var client = new SmtpClient())
                    {
                        MailMessage msg = new MailMessage();
                        //string  fromEmail = string.Format("Agro Industry <{0}>", System.Configuration.ConfigurationManager.AppSettings["AgroEmail"]);

                        msg.From = new MailAddress(string.Format("E-Recruitment <{0}>", System.Configuration.ConfigurationManager.AppSettings["eEmail"]));
                        msg.To.Add(new MailAddress(To));
                        msg.Body = BodyMessage;
                        msg.Subject = Subject;

                        //client.Host = "smtp.naroba.co.za";
                        client.Host = System.Configuration.ConfigurationManager.AppSettings["SMTPServer"];
                        client.Port = 25;
                        client.Credentials = new NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["SMTPUserName"]
                            , System.Configuration.ConfigurationManager.AppSettings["SMTPPassword"]);
                        msg.IsBodyHtml = true;

                        await client.SendMailAsync(msg);
                        Status = true;
                    }
                });
                t.Wait(); // Wait until the above task is complete, email is sent
            }
            catch (Exception ex)
            {
                string Message = ex.Message.ToString();
                Status = false;
            }

            return Status;
        }

        public bool SendSMS(string CellNo, string sMessage)
        {
            try
            {
                //string CellNo = "0725365413";
                //string sMessage = "Test Message";
                bool Status = false;
                WebRequest request = WebRequest.Create("http://10.123.56.201:8080/comms/api/comms/" + CellNo + "/" + sMessage);
                request.Timeout = 100000;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusDescription != "OK")
                {
                    Status = true;
                }
                else
                {
                    Status = false;
                }
                return Status;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return true;
        }
    }
}