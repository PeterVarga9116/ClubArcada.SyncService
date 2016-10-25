using System;
using System.Linq;
using ClubArcada.Common;
using ClubArcada.Common.BusinessObjects.DataClasses;

namespace ClubArcada.SyncService.SyncData
{
    public class SyncData
    {
        private static string NewCS = "Data Source=82.119.117.77;Initial Catalog=ACDB_DEV;User ID=ACDB_user; Password=ULwEsjcpDxjKLbL5";
        private static Guid ServiceID = new Guid("4EBB10F7-1CB5-41C1-8051-3328B7FC5A55");

        private static Credentials CR = new Credentials(ServiceID, 4, NewCS);

        public static void SyncUsers()
        {
            try
            {
                var oldUsers = OldData.Data.UserData.GetList();

                var newUsers = Common.BusinessObjects.Data.UserData.GetList(CR);

                foreach (var u in oldUsers)
                {
                    if (newUsers.Select(x => x.Id).Contains(u.UserId))
                    {
                        var toUpdate = newUsers.SingleOrDefault(y => y.Id == u.UserId);

                        if (toUpdate.FirstName != u.FirstName || toUpdate.LastName != u.LastName || toUpdate.NickName != u.NickName)
                        {
                            toUpdate.LastName = u.LastName;
                            toUpdate.FirstName = u.FirstName;
                            toUpdate.NickName = u.NickName;

                            Common.BusinessObjects.Data.UserData.Save(CR, toUpdate);
                        }
                    }
                    else
                    {
                        Common.BusinessObjects.Data.UserData.Save(CR, new User() { DateCreated = u.DateCreated, AdminLevel = u.AdminLevel, AutoReturnType = 1, CreatedByUserId = ServiceID, Email = u.Email, FirstName = u.FirstName, Id = u.UserId, IsAdmin = u.IsAdmin, IsBlocked = u.IsBlocked, IsPersonal = u.IsPersonal, IsTestUser = false, IsWallet = u.IsWallet.True(), LastName = u.LastName, NickName = u.NickName, Password = u.Password, PhoneNumber = u.PhoneNumber });
                    }
                }
            }
            catch (Exception exp)
            {
            }
            finally
            {
            }
        }

        public static void SyncRequests()
        {
            var oldList = OldData.Data.UserData.GetRequestList();
            var newList = Common.BusinessObjects.Data.RequestData.GetList(CR);

            foreach (var r in oldList)
            {
                if (newList.Select(x => x.Id).Contains(r.RequestId))
                {
                    var toUpdate = newList.SingleOrDefault(y => y.Id == r.RequestId);

                    if(toUpdate.DateCompleted != r.DateCompleted || toUpdate.Status != toUpdate.Status)
                    {
                        Common.BusinessObjects.Data.RequestData.SetResolved(CR, toUpdate.Id);
                    }
                }
                else
                {
                    Common.BusinessObjects.Data.RequestData.Create(CR, new Request() { AssignedTo = r.AssignedTo, DateCompleted = r.DateCompleted, DateCreated = r.DateCreated, DateDeleted = r.DateDeleted, Description = r.Description, Id = r.RequestId, UserId = r.UserId, Status = r.Status });
                }
            }
        }

        public static void SynTournaments()
        {
            var oldTours = OldData.Data.UserData.GetTournaments();

            var filtered = oldTours.Where(t => !t.DateDeleted.HasValue && t.Detail.IsNotNull()).ToList();
            var newTours = Common.BusinessObjects.Data.TournamentData.GetList(CR);

            foreach (var u in filtered)
            {
                if (newTours.Select(nt => nt.Id).Contains(u.TournamentId))
                {
                }
                else
                {
                    var newReq = new Tournament()
                    {
                        Id = u.TournamentId,
                        Date = u.Date,
                        DateCreated = u.DateCreated,
                        DateDeleted = u.DateDeleted,
                        DateEnded = u.DateEnded.HasValue ? u.DateEnded.Value : DateTime.Now.AddYears(-1),
                        Description = u.Description.IsNullOrEmpty() ? string.Empty : u.Description,
                        Name = u.Name,
                        LeagueId = u.LeagueId,
                        IsLeague = u.Detail.IsLeague,
                        IsFullPointed = u.Detail.IsFullPointed,
                        IsFood = u.Detail.IsFood,
                        IsHidden = u.IsHidden.True(),
                        IsPercentageBonus = u.Detail.IsPercentageBonus,
                        IsRunning = u.IsRunning.True(),
                        BountyDesc = u.Detail.BountyDesc.IsNullOrEmpty() ? string.Empty : u.Detail.BountyDesc,
                        BuyInPrize = u.Detail.BuyInPrize,
                        BuyInStack = u.Detail.BuyInStack,
                        CreatedByUserId = u.CreatedByUserId,
                        FullStackBonus = u.Detail.FullStackBonus.HasValue ? u.Detail.FullStackBonus.Value : 0,
                        GameType = (int)u.GameType.ToGameType(),
                        GTD = u.Detail.GTD.HasValue ? u.Detail.GTD.Value : 0,
                        LogicType = 0,
                        ReBuyCount = u.Detail.ReBuyCount.HasValue ? u.Detail.ReBuyCount.Value : 0,
                        RebuyPrize = u.Detail.ReBuyPrize,
                        RebuyStack = u.Detail.ReBuyStack,
                        StructureId = u.Detail.StructureId,
                        SpecialAddonPrize = u.Detail.SpecialAddonPrize.HasValue ? u.Detail.SpecialAddonPrize.Value : 0,
                        SpecialAddonStack = u.Detail.SpecialAddonStack.HasValue ? u.Detail.SpecialAddonStack.Value : 0,
                        AddOnPrize = u.Detail.AddOnPrize,
                        AddOnStack = u.Detail.AddOnStack,
                        BonusStack = u.Detail.BonusStack,
                    };

                    ClubArcada.Common.BusinessObjects.Data.TournamentData.Save(CR, newReq);
                }
            }
        }
    }

    public static class Ext
    {
        public static eGameType ToGameType(this char gametypeChar)
        {
            switch (gametypeChar)
            {
                case 'X':
                    return eGameType.NotSet;

                case 'F':
                    return eGameType.FreezeOut;

                case 'R':
                    return eGameType.RebuyUnlimited;

                case 'D':
                    return eGameType.DoubleChance;

                case 'T':
                    return eGameType.TripleChance;

                case 'C':
                    return eGameType.CashGame;

                case 'E':
                    return eGameType.Freeroll;

                case 'L':
                    return eGameType.RebuyLimited;

                case 'A':
                    return eGameType.DoubleTrouble;

                case 'Y':
                    return eGameType.Final;

                case 'Q':
                    return eGameType.Qualification;

                case 'Z':
                    return eGameType.QualFinal;

                default:
                    return eGameType.NotSet;
            }
        }
    }
}