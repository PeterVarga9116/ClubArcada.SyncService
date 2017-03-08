using System;
using System.IO;
using System.ServiceProcess;
using System.Timers;

namespace ClubArcada.SyncService
{
    public partial class Service1 : ServiceBase
    {
        private Timer _timer;
        private Timer _timerTournamentResults;
        private Timer _timerTransactions;

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

            _timerTournamentResults = new Timer();
            _timerTournamentResults.Interval = 60000;
            _timerTournamentResults.Elapsed += _timerTournamentResults_Elapsed; ;
            _timerTournamentResults.Enabled = true;

            _timerTransactions = new Timer();
            _timerTransactions.Interval = 60000;
            _timerTransactions.Elapsed += _timerTransactions_Elapsed;
            _timerTransactions.Enabled = true;

            WriteErrorLog("SyncService started");
        }

        private void _timerTransactions_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                _timerTransactions.Stop();
                SyncData.SyncData.SyncTransactions();
            }
            catch (Exception exp)
            {
                WriteErrorLog(exp.GetBaseException().ToString());
            }
            finally
            {
                _timerTransactions.Start();
            }
        }

        private void _timerTournamentResults_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                _timerTournamentResults.Stop();
                SyncData.SyncData.SynTournaments();
                SyncData.SyncData.SncTournamentResulst();
            }
            catch (Exception exp)
            {
                WriteErrorLog(exp.GetBaseException().ToString());
            }
            finally
            {
                _timerTournamentResults.Start();
            }
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
            WriteErrorLog("SyncService stopped");
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