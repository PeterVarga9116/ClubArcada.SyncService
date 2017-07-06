using System;
using System.IO;
using System.ServiceProcess;
using System.Timers;
using ClubArcada.Common;

namespace ClubArcada.SyncService
{
    public partial class Service1 : ServiceBase
    {
        private double _20Seconds = 20000;

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
        private Timer _timerCashPlayers;
        private Timer _timerCashStates;
        private Timer _timerDocuments;

        private Timer _timerLiveTournamentPlayers;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _timerDocuments = new Timer();
            _timerDocuments.Interval = _5Minutes;
            _timerDocuments.Elapsed += _timerDocuments_Elapsed;
            _timerDocuments.Enabled = true;
            _timerDocuments.Start();

            _timerLiveTournamentPlayers = new Timer();
            _timerLiveTournamentPlayers.Interval = _20Seconds;
            _timerLiveTournamentPlayers.Elapsed += _timerLiveTournamentPlayers_Elapsed;
            _timerLiveTournamentPlayers.Enabled = true;
            _timerLiveTournamentPlayers.Start();

            _timerCashGames = new Timer();
            _timerCashGames.Interval = _5Minutes;
            _timerCashGames.Elapsed += _timerCashGames_Elapsed;
            _timerCashGames.Enabled = true;
            _timerCashGames.Start();

            _timerCashPlayers = new Timer();
            _timerCashPlayers.Interval = _5Minutes;
            _timerCashPlayers.Elapsed += _timerCashPlayers_Elapsed;
            _timerCashPlayers.Enabled = true;
            _timerCashPlayers.Start();

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

        private void _timerDocuments_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                _timerDocuments.Stop();
                SyncData.SyncData.GenerateAndSendTournamentReport();
            }
            catch(Exception exp)
            {
                ClubArcada.Common.Mailer.Mailer.SendErrorMail("Sync Error - Documents", exp.GetExceptionDetails());
            }
            finally
            {
                _timerDocuments.Start();
            }
        }

        private void _timerLiveTournamentPlayers_Elapsed(object sender, ElapsedEventArgs e)
        {
            Sync(_timerLiveTournamentPlayers, () => SyncData.SyncData.SyncLiveTournamentPlayers(), _20Seconds, "Live Tournament Players");
        }

        private void _timerCashPlayers_Elapsed(object sender, ElapsedEventArgs e)
        {
            Sync(_timerCashPlayers, () => SyncData.SyncData.SyncCashPlayers(), _2Minutes, "Cash Players");
        }

        private void _timerCashGames_Elapsed(object sender, ElapsedEventArgs e)
        {
            Sync(_timerCashGames, () => SyncData.SyncData.SyncCashGames(), _5Minutes, "Cash Games");
        }

        private void _timerCashStates_Elapsed(object sender, ElapsedEventArgs e)
        {
            Sync(_timerCashStates, () => SyncData.SyncData.SyncCashStates(), _5Minutes, "Cash States");
        }

        private void _timerTickets_Elapsed(object sender, ElapsedEventArgs e)
        {
            Sync(_timerTickets, () => SyncData.SyncData.SyncTickets(), _1Minute, "Tickets", () => SyncData.SyncData.SyncTicketItems());
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
            Sync(_timerSyncTournamentCashouts, () => SyncData.SyncData.SyncTournamentCashouts(), _1Minute, "TournamentCashouts");
        }

        private void _timerSyncLeagues_Elapsed(object sender, ElapsedEventArgs e)
        {
            Sync(_timerSyncLeagues, () => SyncData.SyncData.SyncLeagues(), _1Minute, "Leagues");
        }

        private void _timerSyncRequests_Elapsed(object sender, ElapsedEventArgs e)
        {
            Sync(_timerSyncRequests, () => SyncData.SyncData.SyncRequests(), _1Minute, "Requests");
        }

        private void _timerTransactions_Elapsed(object sender, ElapsedEventArgs e)
        {
            Sync(_timerTransactions, () => SyncData.SyncData.SyncTransactions(), _1Minute, "Transactions");
        }

        private void _timerTournamentResults_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                _timerTournamentResults.Stop();
                SyncData.SyncData.SyncTournaments();
                SyncData.SyncData.SyncTournamentResulst();
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
            Sync(_timerSyncUsers, () => SyncData.SyncData.SyncUsers(), _1Minute, "Users");
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

        private void Sync(Timer timer, Action action, double interval, string name, Action callback = null)
        {
            try
            {
                timer.Stop();
                action();
                if(callback.IsNotNull())
                {
                    callback();
                }
            }
            catch (Exception exp)
            {
                ClubArcada.Common.Mailer.Mailer.SendErrorMail("Sync Error - " + name, exp.GetExceptionDetails());
            }
            finally
            {
                timer.Interval = interval;
                timer.Start();
            }
        }

    }
}