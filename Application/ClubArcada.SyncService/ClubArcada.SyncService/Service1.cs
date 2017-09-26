using System;
using System.IO;
using System.ServiceProcess;
using System.Timers;
using ClubArcada.Common;

namespace ClubArcada.SyncService
{
    public partial class Service1 : ServiceBase
    {
        private double _1Minute = 60000;
        private double _2Minutes = 120000;
        private double _5Minutes = 300000;
        private double _10Minutes = 600000;
        private double _30Minutes = 1800000;
        private double _1Hour = 3600000;

        private Timer _timerSyncLeagues;
        private Timer _timerSyncUsers;
        private Timer _timerSyncRequests;
        private Timer _timerTournamentResults;
        private Timer _timerSyncTournamentCashouts;
        private Timer _timerTransactions;
        private Timer _timerStructures;
        private Timer _timerTickets;
        private Timer _timerCashGames;
        private Timer _timerCashStates;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _timerCashGames = new Timer();
            _timerCashGames.Interval = _1Minute;
            _timerCashGames.Elapsed += _timerCashGames_Elapsed; ;
            _timerCashGames.Enabled = true;
            _timerCashGames.Start();

            _timerCashStates = new Timer();
            _timerCashStates.Interval = _1Minute;
            _timerCashStates.Elapsed += _timerCashStates_Elapsed;
            _timerCashStates.Enabled = true;
            _timerCashStates.Start();

            _timerStructures = new Timer();
            _timerStructures.Interval = _1Minute;
            _timerStructures.Elapsed += _timerStructures_Elapsed;
            _timerStructures.Enabled = true;
            _timerStructures.Start();

            _timerTickets = new Timer();
            _timerTickets.Interval = _1Minute;
            _timerTickets.Elapsed += _timerTickets_Elapsed;
            _timerTickets.Enabled = true;
            _timerTickets.Start();

            _timerSyncUsers = new Timer();
            _timerSyncUsers.Interval = _1Minute;
            _timerSyncUsers.Elapsed += _timerSyncUsers_Elapsed;
            _timerSyncUsers.Enabled = true;
            _timerSyncUsers.Start();

            _timerSyncLeagues = new Timer();
            _timerSyncLeagues.Interval = _1Minute;
            _timerSyncLeagues.Elapsed += _timerSyncLeagues_Elapsed;
            _timerSyncLeagues.Enabled = true;
            _timerSyncLeagues.Start();

            _timerSyncRequests = new Timer();
            _timerSyncRequests.Interval = _1Minute;
            _timerSyncRequests.Elapsed += _timerSyncRequests_Elapsed;
            _timerSyncRequests.Enabled = true;
            _timerSyncRequests.Start();

            _timerTournamentResults = new Timer();
            _timerTournamentResults.Interval = _1Minute;
            _timerTournamentResults.Elapsed += _timerTournamentResults_Elapsed;
            _timerTournamentResults.Enabled = true;
            _timerTournamentResults.Start();

            _timerSyncTournamentCashouts = new Timer();
            _timerSyncTournamentCashouts.Interval = _1Minute;
            _timerSyncTournamentCashouts.Elapsed += _timerSyncTournamentCashouts_Elapsed;
            _timerSyncTournamentCashouts.Enabled = true;
            _timerSyncTournamentCashouts.Start();

            _timerTransactions = new Timer();
            _timerTransactions.Interval = _1Minute;
            _timerTransactions.Elapsed += _timerTransactions_Elapsed;
            _timerTransactions.Enabled = true;
            _timerTransactions.Start();

            WriteErrorLog("SyncService started");
        }

        private void _timerCashGames_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                _timerCashStates.Stop();
                SyncData.SyncData.SyncCashGames();
                SyncData.SyncData.SyncCashPlayers();
            }
            catch (Exception exp)
            {
                ClubArcada.Common.Mailer.Mailer.SendErrorMail("Sync Error - Cash States", exp.GetExceptionDetails());
            }
            finally
            {
                _timerCashStates.Interval = _5Minutes;
                _timerCashStates.Start();
            }
        }

        private void _timerCashStates_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                _timerCashStates.Stop();
                SyncData.SyncData.SyncCashStates();
            }
            catch (Exception exp)
            {
                ClubArcada.Common.Mailer.Mailer.SendErrorMail("Sync Error - Cash States", exp.GetExceptionDetails());
            }
            finally
            {
                _timerCashStates.Interval = _5Minutes;
                _timerCashStates.Start();
            }
        }

        private void _timerTickets_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                _timerTickets.Stop();
                SyncData.SyncData.SyncTickets();
                SyncData.SyncData.SyncTicketItems();
            }
            catch (Exception exp)
            {
                ClubArcada.Common.Mailer.Mailer.SendErrorMail("Sync Error - Tickets", exp.GetExceptionDetails());
            }
            finally
            {
                _timerTickets.Interval = _5Minutes;
                _timerTickets.Start();
            }
        }

        private void _timerStructures_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                _timerStructures.Stop();
                SyncData.SyncData.SyncStructures();
                SyncData.SyncData.SyncStructureDetails();
            }
            catch (Exception exp)
            {
                ClubArcada.Common.Mailer.Mailer.SendErrorMail("Sync Error - Structures", exp.GetExceptionDetails());
            }
            finally
            {
                _timerStructures.Interval = _5Minutes;
                _timerStructures.Start();
            }
        }

        private void _timerSyncTournamentCashouts_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                _timerSyncTournamentCashouts.Stop();
                SyncData.SyncData.SyncTournamentCashouts();
            }
            catch (Exception exp)
            {
                ClubArcada.Common.Mailer.Mailer.SendErrorMail("Sync Error - TournamentCashouts", exp.GetExceptionDetails());
            }
            finally
            {
                _timerSyncTournamentCashouts.Interval = _30Minutes;
                _timerSyncTournamentCashouts.Start();
            }
        }

        private void _timerSyncLeagues_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                _timerSyncLeagues.Stop();
                SyncData.SyncData.SyncLeagues();
            }
            catch (Exception exp)
            {
                ClubArcada.Common.Mailer.Mailer.SendErrorMail("Sync Error - Leagues", exp.GetExceptionDetails());
            }
            finally
            {
                _timerSyncLeagues.Interval = _1Hour;
                _timerSyncLeagues.Start();
            }
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
                _timerSyncRequests.Interval = _1Minute;
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
                _timerTransactions.Interval = _1Minute;
                _timerTransactions.Start();
            }
        }

        private void _timerTournamentResults_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                _timerTournamentResults.Stop();
                SyncData.SyncData.SyncTournaments();
                SyncData.SyncData.SncTournamentResulst();
            }
            catch (Exception exp)
            {
                ClubArcada.Common.Mailer.Mailer.SendErrorMail("Sync Error - Tournaments", exp.GetExceptionDetails());
            }
            finally
            {
                _timerTournamentResults.Interval = _1Minute;
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
                _timerSyncUsers.Interval = _1Minute;
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