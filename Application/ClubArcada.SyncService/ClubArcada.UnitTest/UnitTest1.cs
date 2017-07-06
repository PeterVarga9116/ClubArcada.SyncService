using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClubArcada.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            try
            {
                //SyncService.SyncData.SyncData.SncTournamentResulst();
                //SyncService.SyncData.SyncData.SyncCashGames();
                //SyncService.SyncData.SyncData.SyncCashStates();
                SyncService.Documents.Generator.GenerateTournamentReport();
            }
            catch (Exception exp)
            {

            }

        }
    }
}
