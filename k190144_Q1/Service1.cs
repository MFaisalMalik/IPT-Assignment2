using System;
using System.ComponentModel;
using System.ServiceProcess;
using System.Timers;
using System.Diagnostics;
using System.Configuration;

namespace k190144_Q1
{
    [RunInstaller(true)]
    public partial class Service1 : ServiceBase
    {
        private Process process;
        private int minutes = 5;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            process = new Process();
            var appSettings = ConfigurationManager.AppSettings;
            var InputPath = appSettings["InputPath"];
            var OutputPath = appSettings["OutputPath"];
            var Url = appSettings["Url"];

            process.StartInfo.FileName = InputPath;
            process.StartInfo.Arguments = $"{Url} {OutputPath}";
            process.Start();


            Timer timer = new Timer();
            timer.Interval =  minutes*60*1000;
            timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
            timer.Start();

        }

        public void OnTimer(object sender, ElapsedEventArgs args)
        {
            process.Start();
        }

        protected override void OnStop()
        {
            
        }
    }
}
