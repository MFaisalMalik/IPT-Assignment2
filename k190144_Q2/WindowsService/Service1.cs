using System.ComponentModel;
using System.ServiceProcess;
using k190144_Q2_ClassLibrary;
using System.Timers;
using System.Configuration;


namespace WindowsService
{
    [RunInstaller(true)]

    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var appSettings = ConfigurationManager.AppSettings;
            var InputPath = appSettings["InputPath"];

            ParsingLogic.ParseHtmlFile(InputPath);
            Timer timer = new Timer();
            timer.Interval = 10 * 60 * 1000; // 10 minutes
            timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
            timer.Start();
        }

        public void OnTimer(object sender, ElapsedEventArgs args)
        {
            var appSettings = ConfigurationManager.AppSettings;
            var InputPath = appSettings["InputPath"];

            ParsingLogic.ParseHtmlFile(InputPath);
        }

        protected override void OnStop()
        {
        }
    }
}