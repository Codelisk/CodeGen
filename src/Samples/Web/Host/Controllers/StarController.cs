using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
//using StarMicronics.CloudPrnt;
//using StarMicronics.CloudPrnt.CpMessage;
using System.IO;
using System.Net.Http.Headers;
using System.Text;

namespace Host.Controllers
{
    //[ApiController]
    //[Route("[controller]")]
    //public class StarController : ControllerBase
    //{
    //    [HttpPost]
    //    public async Task<string> HandleCloudPRNTPoll([FromBody] object request)
    //    {
    //        PollRequest pollRequest = PollRequest.FromJson(request + string.Empty);
    //        Console.WriteLine(
    //            String.Format("Received CloudPRNT request from {0}, status: {1}",
    //            pollRequest.printerMAC,
    //            pollRequest.statusCode
    //        ));

    //        // Create a response object
    //        PollResponse pollResponse = new PollResponse();

    //        pollResponse.jobReady = true;

    //        pollResponse.mediaTypes = new List<string>();
    //        pollResponse.mediaTypes.AddRange(Document.GetOutputTypesFromType("text/vnd.star.markup"));


    //        return pollResponse.ToJson();
    //    }
    //    [HttpGet]
    //    public async Task<IActionResult> PrintJob()
    //    {
    //        // Make a simple markup language job
    //        StringBuilder job = new StringBuilder();
    //        job.Append("Hello World!\n");
    //        job.Append("[barcode: type code39; data 12345; height 10mm]\n");
    //        job.Append("[cut]");
    //        byte[] jobData = Encoding.UTF8.GetBytes(job.ToString());

    //        jobData = await CallUrl();
    //        // Get the requested output media type from the query string
    //        string outputFormat = this.HttpContext.Request.Query["type"];

    //        using (MemoryStream outputMemoryStream = new MemoryStream())
    //        {
    //            // Set the response media type
    //            this.HttpContext.Response.ContentType = outputFormat;

    //            // Perform the conversion asynchronously
    //            await Task.Run(() => Document.Convert(jobData, "text/vnd.star.markup", outputMemoryStream, outputFormat, null));

    //            // Copy the output to the response body
    //            outputMemoryStream.Position = 0;
    //            await outputMemoryStream.CopyToAsync(this.HttpContext.Response.Body);
    //        }

    //        return new EmptyResult();
    //    }

    //    private async Task<byte[]> CallUrl()
    //    {
    //        string url = "https://orderlyzeservicedev.azurewebsites.net/api/PrintJob/GetPrintAble";
    //        string accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjIxNDMiLCJlbWFpbCI6InN0YXJAZHJ1Y2tlci5hdCIsIlVzZXJOYW1lIjoiRHJ1Y2tlciIsIlNlbGxlcklkIjoiMTY0NSIsIkNhc2hib3hJZCI6IjE2NzIiLCJDdXJyZW5jeUNvZGUiOiJFVVIiLCJDb3VudHJ5Q29kZSI6IkFUIiwiRXhlbXB0VGF4IjoiRmFsc2UiLCJTZWxsZXJUeXBJZCI6IjIiLCJTaWduYXR1cmVUeXBlIjoiMCIsIlByaW50ZXJJZCI6IjE0IiwiQ3VsdHVyZUNvZGUiOiJkZS1BVCIsIlNlbGxlclN0YXRlIjoiMCIsIm5iZiI6MTY5MTk5Mjg0NSwiZXhwIjoxNjk0ODcyODQ1LCJpYXQiOjE2OTE5OTI4NDV9.tYn0uOX2u11C-_4l2DwnvaqBMicgW4JQk8LIJhQtfKw"; // Replace with your actual access token

    //        using (HttpClient client = new HttpClient())
    //        {
    //            // Set the authorization header with the access token
    //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

    //            try
    //            {
    //                HttpResponseMessage response = await client.GetAsync(url);
    //                if (response.IsSuccessStatusCode)
    //                {
    //                    string responseBody = await response.Content.ReadAsStringAsync();
    //                    var printJob = JsonConvert.DeserializeObject<List<PrintJob>>(responseBody);
    //                    var bytes = Convert.FromBase64String(printJob.First().Data);
    //                    return bytes;
    //                }
    //                else
    //                {
    //                    return null;
    //                    Console.WriteLine("Request failed with status code: " + response.StatusCode);
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                return null;
    //                Console.WriteLine("An error occurred: " + ex.Message);
    //            }
    //        }
    //    }
    //}
    //public class PrintJob
    //{
    //    public int Id { get; set; }
    //    public int InvoiceId { get; set; }
    //    public int? InvoicePaymentPositionId { get; set; }
    //    public string Data { get; set; }
    //    public int SellerId { get; set; }
    //    public int InternalNumber { get; set; }
    //    public int PrintCount { get; set; }
    //    public string Name { get; set; }
    //    public int Kind { get; set; }
    //    public int State { get; set; }
    //    public int PrintToUserId { get; set; }
    //    public int? PrinterId { get; set; }
    //    public DateTime CreateTimeStamp { get; set; }
    //    public DateTime LastChangeTimeStamp { get; set; }
    //    public int CreateByUserId { get; set; }
    //    public int LastChangeUserId { get; set; }
    //    public object OrderDetailIds { get; set; }
    //}
}
