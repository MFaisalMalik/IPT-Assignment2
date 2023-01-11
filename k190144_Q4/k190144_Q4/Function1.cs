using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace k190144_Q4
{
    public static class Function1
    {
        [FunctionName("myAzureFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];
            name += " .json";

            var inputPath = Environment.GetEnvironmentVariable("InputPath");

            DirectoryInfo di = new DirectoryInfo(inputPath);
            DirectoryInfo[] diArr = di.GetDirectories();

            bool flag = false;
            string responseMessage = "";

            foreach (DirectoryInfo dri in diArr)
            {
                if (File.Exists(dri.FullName.ToString() + "\\" + name))
                {
                    responseMessage = File.ReadAllText(dri.FullName.ToString() + "\\" + name);
                    flag = true;
                }

            }

            if (!flag)
            {
                responseMessage =  "File does not exists";
            }

            return new OkObjectResult(responseMessage);
        }
    }
}
