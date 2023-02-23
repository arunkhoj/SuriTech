using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


using System.Net;
using System.Net.Mail;
using System.Collections.Generic;
// namespace SuriTech.Function
// {
//     public static class HttpSendMailTrigger
//     {
//         [FunctionName("HttpSendMailTrigger")]
//         public static async Task<IActionResult> Run(
//             [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
//             ILogger log)
//         {
//             log.LogInformation("C# HTTP trigger function processed a request.");

//             string name = req.Query["name"];

//             string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
//             dynamic data = JsonConvert.DeserializeObject(requestBody);
//             name = name ?? data?.name;

//             string responseMessage = string.IsNullOrEmpty(name)
//                 ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
//                 : $"Hello, {name}. This HTTP triggered function executed successfully.";

//             return new OkObjectResult(responseMessage);
//         }
//     }
// }



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
 
    // 1: Get request body + validate required content is available
    var fromEmail = req.Query["fromEmail"];
    var message = req.Query["message"];
    // var missingFields = new List<string>();
    // if(postData["fromEmail"] == null)
    //     missingFields.Add("fromEmail");
    // if(postData["message"] == null)
    //     missingFields.Add("message");
    
    // if(missingFields.Any())
    // {
    //     var missingFieldsSummary = String.Join(", ", missingFields);
    //     return req.CreateResponse(HttpStatusCode.BadRequest, $"Missing field(s): {missingFieldsSummary}");
    // }
 
    // 2: Site settings
    var smtpHost = "smtp.office365.com";
    var smtpPort = Convert.ToInt32("587");
    var smtpEnableSsl = Boolean.Parse("True");
    var smtpUser = "support@suritechs.com";
    var smtpPass = "Me@hochiminh2023";
    var toEmail = "arunkhoj@gmail.com";
 
    // 3: Build + Send the email
    MailMessage mailObj = new MailMessage("arunkhoj@gmail.com", toEmail, "Site Contact Form", "I wish to join");
    SmtpClient client = new SmtpClient();
    client.Host = smtpHost;
    client.Port = smtpPort;
    client.EnableSsl = smtpEnableSsl;
    client.DeliveryMethod = SmtpDeliveryMethod.Network;
    client.UseDefaultCredentials = false;
    client.Credentials = new System.Net.NetworkCredential(smtpUser, smtpPass);
 
 
    try
    {
        client.Send(mailObj);
        string responseMessage = HttpStatusCode.OK.ToString();
        return new OkObjectResult(responseMessage);
        //return req.CreateResponse(HttpStatusCode.OK, "Thanks!");
    }
    catch (Exception ex)
    {
        string responseMessage = HttpStatusCode.InternalServerError.ToString();
        return new OkObjectResult($"Email has not been sent: {ex.GetType()}" + responseMessage);
        // return req.CreateResponse(HttpStatusCode.InternalServerError, new {
        //     status = false,
        //     message = $"Email has not been sent: {ex.GetType()}"            
        // });
    }
}
    }
}