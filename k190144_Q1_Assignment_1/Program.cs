using System.Net;

namespace k190144_Q1
{
    internal class Program
    {
        public void DownloadWebPage(string url, string outputFolder)
        {

            WebClient Client = new WebClient();
            Client.DownloadFile(url, outputFolder);
        }
        public static void Main(string[] args)
        {
            Program obj = new Program();

            Directory.CreateDirectory(args[1]);

            string fileName = "\\Summary" + DateTime.Today.ToString("ddMMMyy") + ".html";
            args[1] += fileName;

            obj.DownloadWebPage(args[0], args[1]);

        }

    }
}
