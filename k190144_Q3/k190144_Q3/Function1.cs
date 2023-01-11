using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using System.Xml;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace k190144_Q3
{
    public class Function1
    {
        [FunctionName("Function1")]
        public void Run([TimerTrigger("0 */20 * * * *")]TimerInfo myTimer, ILogger log)
        {
            string inputPath = Environment.GetEnvironmentVariable("InputPath");
            string outputPath = Environment.GetEnvironmentVariable("OutputPath");

            Directory.CreateDirectory(outputPath);

            DirectoryInfo di = new DirectoryInfo(inputPath);
            DirectoryInfo[] diArr = di.GetDirectories();


            foreach (DirectoryInfo dri in diArr)
            {
                Directory.CreateDirectory(outputPath + dri.Name);
                var directory = new DirectoryInfo(dri.ToString());
                var fileName = directory.GetFiles()
                 .OrderByDescending(f => f.LastWriteTime)
                 .First();

                XmlTextReader readFile = new XmlTextReader(fileName.ToString());
                string script = "", price = "";

                while (readFile.Read())
                {
                    if (readFile.NodeType == XmlNodeType.Element && readFile.Name == "Script")
                    {
                        script = readFile.ReadElementString();

                    }
                    if (readFile.NodeType == XmlNodeType.Element && readFile.Name == "Price")
                    {
                        price = readFile.ReadElementString();

                        string filePath = outputPath + dri.Name + "\\" + script + ".json";

                        if (!File.Exists(filePath))
                        {
                            JsonObjectClass obj = new JsonObjectClass();
                            obj.lastUpdated = DateTime.Now;
                            obj.dateAndPrice.Add(new DateAndPrice() { Price = float.Parse(price), Date = DateTime.Now });

                            using (StreamWriter file = File.CreateText(filePath))
                            {
                                JsonSerializer serializer = new JsonSerializer();
                                serializer.Formatting = Formatting.Indented;
                                serializer.Serialize(file, obj);
                            }
                        }
                        else
                        {
                            JsonObjectClass obj2;
                            using (StreamReader file = File.OpenText(filePath))
                            {
                                JsonSerializer serializer = new JsonSerializer();
                                obj2 = (JsonObjectClass)serializer.Deserialize(file, typeof(JsonObjectClass));
                            }
                            obj2.lastUpdated = DateTime.Now;
                            obj2.dateAndPrice.Add(new DateAndPrice() { Price = float.Parse(price), Date = DateTime.Now });

                            File.Delete(filePath);

                            using (StreamWriter file = File.CreateText(filePath))
                            {
                                JsonSerializer serializer = new JsonSerializer();
                                serializer.Formatting = Formatting.Indented;
                                serializer.Serialize(file, obj2);
                            }
                        }
                    }
                }
                readFile.Close();
                File.Delete(fileName.ToString());
            }
        }
    }
}
