using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubArcada.SyncService.OldData.DataClasses;

namespace ClubArcada.SyncService.OldData.Data
{
    public class OldDbData
    {
        public static string CS = "Data Source=82.119.117.77;Initial Catalog=PokerSystem;Integrated Security=False;User Id=PokerTimer;Password=Poker1969;MultipleActiveResultSets=True";

        public static List<User> GetList()
        {
            using (var appDC = new ODBDataContext(CS))
            {
                return appDC.Users.ToList();
            }
        }

        public static List<Request> GetRequestList()
        {
            using (var appDC = new ODBDataContext(CS))
            {
                return appDC.Requests.ToList();
            }
        }

        public static List<Tournament> GetTournaments()
        {
            using (var appDC = new ODBDataContext(CS))
            {
                var tours = appDC.Tournaments.Where(t => t.Date > DateTime.Now.AddMonths(-1)).ToList();

                foreach (var t in tours)
                {
                    t.Detail = appDC.TournamentDetails.SingleOrDefault(td => td.TournamentId == t.TournamentId);
                }

                return tours;
            }
        }

        public static List<Tournament> GetCashGames()
        {
            using (var appDC = new ODBDataContext(CS))
            {
                var tours = appDC.Tournaments.Where(t => t.GameType == 'C').ToList();

                foreach (var t in tours)
                {
                    t.Detail = appDC.TournamentDetails.SingleOrDefault(td => td.TournamentId == t.TournamentId);
                }

                return tours;
            }
        }

        public static List<TournamentCashout> GetTournamentCashouts()
        {
            using (var appDC = new ODBDataContext(CS))
            {
                var cashouts = appDC.TournamentCashouts.ToList();

                return cashouts;
            }
        }

        public static List<League> GetLeagues()
        {
            using (var appDC = new ODBDataContext(CS))
            {
                var items = appDC.Leagues.ToList();

                return items;
            }
        }

        public static List<TournamentResult> GetTournamentResults()
        {
            using (var appDC = new ODBDataContext(CS))
            {
                return appDC.TournamentResults.ToList();
            }
        }

        public static List<Transaction> GetTransactions()
        {
            using (var appDC = new ODBDataContext(CS))
            {
                return appDC.Transactions.ToList();
            }
        }

        public static List<Structure> GetStructures()
        {
            using (var appDC = new ODBDataContext(CS))
            {
                return appDC.Structures.ToList();
            }
        }

        public static List<StructureDetail> GetStructureDetails()
        {
            using (var appDC = new ODBDataContext(CS))
            {
                return appDC.StructureDetails.ToList();
            }
        }

        public static List<Ticket> GetTickets()
        {
            using (var appDC = new ODBDataContext(CS))
            {
                var items = appDC.Tickets.ToList();

                return items;
            }
        }

        public static List<TicketItem> GetTicketItems()
        {
            using (var appDC = new ODBDataContext(CS))
            {
                var items = appDC.TicketItems.ToList();

                return items;
            }
        }

        public static List<CashState> GetCashStates()
        {
            using (var appDC = new ODBDataContext(CS))
            {
                var items = appDC.CashStates.ToList();

                return items;
            }
        }

        public static List<CashResult> GetCashResults()
        {
            using (var appDC = new ODBDataContext(CS))
            {
                return appDC.CashResults.ToList();
            }
        }
    }
}
