﻿using ClubArcada.Common.BusinessObjects.DataClasses;
using System;

namespace ClubArcada.SyncService
{
    public class Settings
    {
        private static string NewCS = "Data Source=82.119.117.77;Initial Catalog=ACDB_DEV;User ID=ACDB_user; Password=ULwEsjcpDxjKLbL5";
        private static Guid ServiceID = new Guid("4EBB10F7-1CB5-41C1-8051-3328B7FC5A55");

        public static Credentials CR = new Credentials(ServiceID, 4, NewCS);
    }
}
