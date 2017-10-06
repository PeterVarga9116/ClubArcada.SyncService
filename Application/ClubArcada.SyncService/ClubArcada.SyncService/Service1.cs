using ClubArcada.Common;
using System;
using System.IO;
using System.ServiceProcess;
using System.Timers;

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
        private Timer _timerBanners;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            CreateNewTimer(_timerBanners, _30Minutes, "banner", () => SyncData.SyncData.SyncBanners());
            CreateNewTimer(_timerCashGames, _10Minutes, "cashgames", () => SyncData.SyncData.SyncCashGames(), () => SyncData.SyncData.SyncCashPlayers());
            CreateNewTimer(_timerCashStates, _10Minutes, "cash states", () => SyncData.SyncData.SyncCashStates());
            CreateNewTimer(_timerStructures, _10Minutes, "structures", () => SyncData.SyncData.SyncStructures(), () => SyncData.SyncData.SyncStructureDetails());
            CreateNewTimer(_timerTickets, _1Minute, "tickets", () => SyncData.SyncData.SyncTickets(), () => SyncData.SyncData.SyncTicketItems());
            CreateNewTimer(_timerSyncUsers, _5Minutes, "users", () => SyncData.SyncData.SyncUsers());
            CreateNewTimer(_timerSyncLeagues, _10Minutes, "leagues", () => SyncData.SyncData.SyncLeagues());
            CreateNewTimer(_timerSyncRequests, _1Minute, "requests", () => SyncData.SyncData.SyncRequests());
            CreateNewTimer(_timerTournamentResults, _1Minute, "tournamentResults", () => SyncData.SyncData.SyncTournaments(), () => SyncData.SyncData.SncTournamentResulst());
            CreateNewTimer(_timerSyncTournamentCashouts, _1Minute, "cashouts", () => SyncData.SyncData.SyncTournamentCashouts());
            CreateNewTimer(_timerTransactions, _1Minute, "transactions", () => SyncData.SyncData.SyncTransactions());

            WriteErrorLog("SyncService started");
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

        private void CreateNewTimer(Timer timer, double interval, string name, params Action[] actions)
        {
            timer = new Timer();
            timer.Interval = _1Minute;
            timer.Elapsed += delegate
            {
                foreach (var a in actions)
                {
                    Sync(timer, a, interval, name);
                }
            };
            timer.Enabled = true;
            timer.Start();
        }

        private void Sync(Timer timer, Action action, double interval, string name)
        {
            try
            {
                timer.Stop();
                action();
            }
            catch (Exception exp)
            {
                ClubArcada.Common.Mailer.Mailer.SendErrorMail("Sync Error - " + name, exp.GetExceptionDetails());
            }
            finally
            {
                timer.Interval = _1Minute;
                timer.Start();
            }
        }

    }
}