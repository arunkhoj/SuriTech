using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;


//             string name = req.Query["name"];

//             string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
//             dynamic data = JsonConvert.DeserializeObject(requestBody);
//             name = name ?? data?.name;

//             string responseMessage = string.IsNullOrEmpty(name)
//                 ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
//                 : $"Hello, {name}. This HTTP triggered function executed successfully.";

//             return new OkObjectResult(responseMessage);




namespace SuriTech.Function
{
    public static class HttpSendMailTrigger
    {
        [FunctionName("HttpSendMailTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //Credentials
            String userName = "support@suritechs.com";
            String password = "Me@hochiminh2023";
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {
                //Parameters
                String fromEmail = req.Query["from"]; //Sender
                String bodyEmail = req.Query["body"]; //Notes
                String subjectEmail = req.Query["subject"]; //"SuriTech Website-Enquiry"
                String toEmail = req.Query["to"]; //To Email
                var message = new MailMessage
                {
                    From = new MailAddress(fromEmail, subjectEmail),
                    Body = bodyEmail,
                    Subject = subjectEmail,
                    To = { toEmail }
                };

                using (var client = new SmtpClient())
                {
                    client.EnableSsl = true;
                    client.Host = "smtp.office365.com";
                    client.Port = 587;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.Credentials = new NetworkCredential(userName, password);

                    await client.SendMailAsync(message);
                }
                string responseMessage = HttpStatusCode.OK.ToString() + "Thanks!";
                return new OkObjectResult(responseMessage);
            }
            catch (Exception ex)
            {
                string responseMessage = HttpStatusCode.InternalServerError.ToString();
                return new OkObjectResult($"Email has not been sent: {ex.GetType()}" + responseMessage);               
            }
        }

    }
}