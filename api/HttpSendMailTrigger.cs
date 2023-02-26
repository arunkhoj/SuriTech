using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using Newtonsoft.Json;


//             string name = req.Query["name"];

//             string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
//             dynamic data = JsonConvert.DeserializeObject(requestBody);
//             name = name ?? data?.name;

//             string responseMessage = string.IsNullOrEmpty(name)
//                 ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
//                 : $"Hello, {name}. This HTTP triggered function executed successfully.";

//             return new OkObjectResult(responseMessage);

//  string name = req.Query["name"];

//             string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
//             dynamic data = JsonConvert.DeserializeObject(requestBody);
            

//             log.LogInformation("C# HTTP trigger function processed a request.");
//             return new OkObjectResult("Welcome to Azure Functions!" + HttpStatusCode.OK + name + data.topic + data.topic1 + data.topic3);


namespace SuriTech.Function
{
    public class Sendmail
    {
        [FunctionName("Sendmail")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            // Read query string
            string orgname = req.Query["oname"];

            // Read body json
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            //orgname + data.sub + data.body + data.to + data.from
            try
            {
                String userName = "support@suritechs.com";
                String password = "Sector84@vietngocman2023";

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var message = new MailMessage
                {
                    From = new MailAddress("support@suritechs.com", orgname), //("support@suritechs.com", "Suri Support")
                    Body = data.body.ToString(),
                    Subject = data.sub.ToString(),
                    To = { data.to.ToString() }
                    //CC = {"arunkhoj@gmail.com"}
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
                string responseMessage = HttpStatusCode.OK.ToString() + " Thanks!";
                return new OkObjectResult(responseMessage);
            }
            catch(Exception ex)
            {
                string responseMessage = HttpStatusCode.InternalServerError.ToString();
                return new OkObjectResult($"Email has not been sent: {ex.GetType()} " + responseMessage);
            }
        }
    }
}