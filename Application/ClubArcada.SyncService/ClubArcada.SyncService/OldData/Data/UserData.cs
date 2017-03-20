﻿using System;
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
                var tours = appDC.Tournaments.ToList();

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
    }
}
