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
                SyncService.SyncData.SyncData.SyncUsers();
            }
            catch (Exception exp)
            {
                
            }

        }
    }
}
