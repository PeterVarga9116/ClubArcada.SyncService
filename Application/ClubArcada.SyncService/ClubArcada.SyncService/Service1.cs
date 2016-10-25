using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ClubArcada.SyncService
{
    public partial class Service1 : ServiceBase
    {
        private Timer _timer;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _timer = new Timer();
            _timer.Interval = 60000;
            _timer.Elapsed += _timer_Elapsed;
            _timer.Enabled = true;

            WriteErrorLog("SyncService started");
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                _timer.Stop();
                SyncData.SyncData.SyncUsers();
                SyncData.SyncData.SyncRequests();

            }
            catch (Exception exp)
            {
                WriteErrorLog(exp.GetBaseException().ToString());
            }
            finally
            {
                _timer.Start();
            }
        }

        protected override void OnStop()
        {
        }

        private static void WriteErrorLog(string message)
        {
            var sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
            sw.WriteLine(message);
            sw.Flush();
            sw.Close();
        }
    }
}
