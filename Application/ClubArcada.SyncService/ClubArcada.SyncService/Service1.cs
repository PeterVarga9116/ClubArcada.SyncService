using System;
using System.IO;
using System.ServiceProcess;
using System.Timers;
using ClubArcada.Common;

namespace ClubArcada.SyncService
{
    public partial class Service1 : ServiceBase
    {
        private Timer _timerSyncUsers;
        private Timer _timerSyncRequests;
        private Timer _timerTournamentResults;
        private Timer _timerTransactions;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _timerSyncUsers = new Timer();
            _timerSyncUsers.Interval = 600000;
            _timerSyncUsers.Elapsed += _timerSyncUsers_Elapsed;
            _timerSyncUsers.Enabled = true;
            _timerSyncUsers.Start();

            _timerSyncRequests = new Timer();
            _timerSyncRequests.Interval = 300000;
            _timerSyncRequests.Elapsed += _timerSyncRequests_Elapsed; ;
            _timerSyncRequests.Enabled = true;
            _timerSyncRequests.Start();

            _timerTournamentResults = new Timer();
            _timerTournamentResults.Interval = 60000;
            _timerTournamentResults.Elapsed += _timerTournamentResults_Elapsed; ;
            _timerTournamentResults.Enabled = true;
            _timerTournamentResults.Start();

            _timerTransactions = new Timer();
            _timerTransactions.Interval = 600000;
            _timerTransactions.Elapsed += _timerTransactions_Elapsed;
            _timerTransactions.Enabled = true;
            _timerTransactions.Start();

            WriteErrorLog("SyncService started");
        }

        private void _timerSyncRequests_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                _timerSyncRequests.Stop();
                SyncData.SyncData.SyncRequests();
            }
            catch (Exception exp)
            {
                ClubArcada.Common.Mailer.Mailer.SendErrorMail("Sync Error - Requests", exp.GetExceptionDetails());
            }
            finally
            {
                _timerSyncRequests.Start();
            }
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
                ClubArcada.Common.Mailer.Mailer.SendErrorMail("Sync Error - Transactions", exp.GetExceptionDetails());
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
                ClubArcada.Common.Mailer.Mailer.SendErrorMail("Sync Error - Tournaments", exp.GetExceptionDetails());
            }
            finally
            {
                _timerTournamentResults.Start();
            }
        }

        private void _timerSyncUsers_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                _timerSyncUsers.Stop();
                SyncData.SyncData.SyncUsers();
            }
            catch (Exception exp)
            {
                ClubArcada.Common.Mailer.Mailer.SendErrorMail("Sync Error - Users", exp.GetExceptionDetails());
            }
            finally
            {
                _timerSyncUsers.Start();
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