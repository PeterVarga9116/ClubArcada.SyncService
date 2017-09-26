using ClubArcada.Common;
using ClubArcada.Common.BusinessObjects.DataClasses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClubArcada.SyncService.SyncData
{
    public class SyncData
    {
        private static string NewCS = "Data Source=82.119.117.77;Initial Catalog=ACDB_DEV;User ID=ACDB_user; Password=ULwEsjcpDxjKLbL5";
        private static Guid ServiceID = new Guid("4EBB10F7-1CB5-41C1-8051-3328B7FC5A55");

        private static Credentials CR = new Credentials(ServiceID, 4, NewCS);

        public static void SyncUsers()
        {
            var oldUsers = OldData.Data.OldDbData.GetList();

            var newUsers = Common.BusinessObjects.Data.UserData.GetList(CR);

            foreach (var u in oldUsers)
            {
                if (newUsers.Select(x => x.Id).Contains(u.UserId))
                {
                    var toUpdate = newUsers.SingleOrDefault(y => y.Id == u.UserId);

                    if (toUpdate.FirstName != u.FirstName || toUpdate.LastName != u.LastName || toUpdate.NickName != u.NickName || toUpdate.Email != u.Email || toUpdate.AdminLevel != u.AdminLevel)
                    {
                        toUpdate.LastName = u.LastName;
                        toUpdate.FirstName = u.FirstName;
                        toUpdate.NickName = u.NickName;
                        toUpdate.AdminLevel = u.AdminLevel;
                        toUpdate.Email = u.Email;
                        toUpdate.Password = u.Password;

                        Common.BusinessObjects.Data.UserData.Save(CR, toUpdate);
                    }
                }
                else
                {
                    Common.BusinessObjects.Data.UserData.Save(CR, new User() { DateCreated = u.DateCreated, AdminLevel = u.AdminLevel, AutoReturnType = 1, CreatedByUserId = ServiceID, Email = u.Email, FirstName = u.FirstName, Id = u.UserId, IsAdmin = u.IsAdmin, IsBlocked = u.IsBlocked, IsPersonal = u.IsPersonal, IsTestUser = false, IsWallet = u.IsWallet.True(), LastName = u.LastName, NickName = u.NickName, Password = u.Password, PhoneNumber = u.PhoneNumber });
                }
            }
        }

        public static void SyncRequests()
        {
            var oldList = OldData.Data.OldDbData.GetRequestList();
            var newList = Common.BusinessObjects.Data.RequestData.GetList(CR);

            foreach (var r in oldList)
            {
                if (newList.Select(x => x.Id).Contains(r.RequestId))
                {
                    var toUpdate = newList.SingleOrDefault(y => y.Id == r.RequestId);

                    if (toUpdate.DateCompleted != r.DateCompleted || toUpdate.Status != toUpdate.Status)
                    {
                        Common.BusinessObjects.Data.RequestData.SetResolved(CR, toUpdate.Id);
                    }
                }
                else
                {
                    Common.BusinessObjects.Data.RequestData.Save(CR, new Request() { AssignedTo = r.AssignedTo, DateCompleted = r.DateCompleted, DateCreated = r.DateCreated, DateDeleted = r.DateDeleted, Description = r.Description, Id = r.RequestId, CreatedByUserId = r.UserId, Status = r.Status });
                }
            }
        }

        public static void SyncLeagues()
        {
            var oldList = OldData.Data.OldDbData.GetLeagues();
            var newList = Common.BusinessObjects.Data.LeagueData.GetList(CR);

            foreach (var r in oldList)
            {
                if (newList.Select(x => x.Id).Contains(r.LeagueId))
                {
                    //var toUpdate = newList.SingleOrDefault(y => y.Id == r.LeagueId);
                }
                else
                {
                    Common.BusinessObjects.Data.LeagueData.Save(CR, new League()
                    {
                        Id = r.LeagueId,
                        CreatedByUserId = CR.UserId,
                        DateCreated = DateTime.Now,
                        IsActive = r.IsActive,
                        DateDeleted = null,
                        Name = r.Name
                    });
                }
            }
        }

        public static void SyncTournaments()
        {
            var oldTours = OldData.Data.OldDbData.GetTournaments().ToList();

            var filtered = oldTours.Where(t => t.Detail.IsNotNull()).ToList();
            var newTours = Common.BusinessObjects.Data.TournamentData.GetList(CR, false, false);

            foreach (var u in filtered)
            {
                if (newTours.Select(nt => nt.Id).Contains(u.TournamentId))
                {
                    var tu = newTours.SingleOrDefault(n => n.Id == u.TournamentId);

                    tu.DateDeleted = u.DateDeleted;
                    tu.DateEnded = u.DateEnded;
                    tu.ReEntryCount = u.Detail.ReEntryCount;
                    tu.Name = u.Name;
                    tu.IsRunning = u.IsRunning.True();

                    ClubArcada.Common.BusinessObjects.Data.TournamentData.Save(CR, tu);
                }
                else
                {
                    var newTournament = new Tournament()
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
                        ReEntryCount = u.Detail.ReEntryCount,
                    };

                    ClubArcada.Common.BusinessObjects.Data.TournamentData.Save(CR, newTournament);
                }
            }
        }

        public static void SyncTournamentCashouts()
        {
            var oldList = OldData.Data.OldDbData.GetTournamentCashouts();
            var newList = Common.BusinessObjects.Data.TournamentCashoutData.GetList(CR);

            foreach (var r in oldList)
            {
                if (newList.Select(x => x.Id).Contains(r.TournamentCashoutId))
                {
                    //var toUpdate = newList.SingleOrDefault(y => y.Id == r.TournamentCashoutId);
                }
                else
                {
                    var tournament = Common.BusinessObjects.Data.TournamentData.GetById(CR, r.TournamentId);
                    var cashGame = Common.BusinessObjects.Data.CashGameData.GetById(CR, r.TournamentId);

                    if (tournament.IsNotNull() || cashGame.IsNotNull())
                    {
                        Common.BusinessObjects.Data.TournamentCashoutData.Save(CR, new TournamentCashout()
                        {
                            Id = r.TournamentCashoutId,
                            TournamentId = r.TournamentId,
                            CreatedByUserId = CR.UserId,
                            Tips = r.Tips.HasValue ? r.Tips.Value : 0,
                            Comment = r.Comment,
                            Dotation = (decimal)r.Dotation,
                            APCBank = (decimal)r.APCBank,
                            CGBank = (decimal)r.CGBank,
                            DateCreated = DateTime.Now,
                            DateDeleted = null,
                            Dealer = r.Dealer.HasValue ? (decimal)r.Dealer.Value : 0,
                            Floor = (decimal)r.Floor,
                            Food = (decimal)r.Food,
                            Prizepool = (decimal)r.PrizePool,
                            Rake = (decimal)r.Rake,
                            RunnerHelp = r.RunnerHelp.HasValue ? (decimal)r.RunnerHelp.Value : 0,
                            Places = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}", r.Place_01, r.Place_02, r.Place_03, r.Place_04, r.Place_05, r.Place_06, r.Place_07, r.Place_08, r.Place_09, r.Place_10)
                        });
                    }
                }
            }
        }

        public static void SncTournamentResulst()
        {
            var oldList = OldData.Data.OldDbData.GetTournamentResults().Where(p => p.DateAdded > DateTime.Now.AddMonths(-10)).ToList();

            var newList = Common.BusinessObjects.Data.TournamentPlayerData.GetList(CR);

            foreach (var o in oldList)
            {
                if (newList.Select(tp => tp.Id).Contains(o.TournamentResultId))
                {
                    var toUpdate = newList.SingleOrDefault(tp => tp.Id == o.TournamentResultId);
                    if (toUpdate.State != o.State || toUpdate.DateDeleted != o.DateDeleted || toUpdate.ReEntryCount != o.ReEntryCount || toUpdate.ReBuyCount != o.ReBuyCount || toUpdate.AddOnCount != o.AddOnCount || toUpdate.PokerCount != o.PokerCount || toUpdate.StraightFlushCount != o.StraightFlushCount || toUpdate.RoyalFlushCount != o.RoyalFlushCount || toUpdate.SpecialAddOnCount != o.SpecialAddOnCount || toUpdate.Points != (decimal)o.Points || toUpdate.Rank != o.Rank)
                    {
                        toUpdate.State = o.State.HasValue ? o.State.Value : 0;
                        toUpdate.AddOnCount = o.AddOnCount;
                        toUpdate.DateDeleted = o.DateDeleted;
                        toUpdate.Points = (decimal)o.Points;
                        toUpdate.PokerCount = o.PokerCount;
                        toUpdate.Rank = o.Rank;
                        toUpdate.ReBuyCount = o.ReBuyCount;
                        toUpdate.ReEntryCount = o.ReEntryCount.HasValue ? o.ReEntryCount.Value : 0;
                        toUpdate.RoyalFlushCount = o.RoyalFlushCount;
                        toUpdate.SpecialAddOnCount = o.SpecialAddOnCount.HasValue ? o.SpecialAddOnCount.Value : 0;
                        toUpdate.StraightFlushCount = o.StraightFlushCount;

                        ClubArcada.Common.BusinessObjects.Data.TournamentPlayerData.Save(CR, toUpdate);
                    }
                    else
                    {
                    }
                }
                else
                {
                    var newPlayer = new TournamentPlayer()
                    {
                        Id = o.TournamentResultId,
                        TournamentId = o.TournamentId,
                        UserId = o.UserId,
                        AddOnCount = o.AddOnCount,
                        DateCreated = o.DateAdded,
                        DateDeleted = o.DateDeleted,
                        Points = (decimal)o.Points,
                        PokerCount = o.PokerCount,
                        Rank = o.Rank,
                        ReBuyCount = o.ReBuyCount,
                        ReEntryCount = o.ReEntryCount.HasValue ? o.ReEntryCount.Value : 0,
                        RoyalFlushCount = o.RoyalFlushCount,
                        SpecialAddOnCount = o.SpecialAddOnCount.HasValue ? o.SpecialAddOnCount.Value : 0,
                        StackBonusSum = 0,
                        State = o.State.HasValue ? o.State.Value : 0,
                        StraightFlushCount = o.StraightFlushCount
                    };

                    ClubArcada.Common.BusinessObjects.Data.TournamentPlayerData.Save(CR, newPlayer);
                }
            }
        }

        public static void SyncTransactions()
        {
            var oldItems = OldData.Data.OldDbData.GetTransactions().ToList();
            var newItems = Common.BusinessObjects.Data.TransactionData.GetList(CR);

            foreach (var i in oldItems)
            {
                if (newItems.Select(n => n.Id).Contains(i.TransactionId))
                {
                }
                else
                {
                    var newT = new Common.BusinessObjects.DataClasses.Transaction()
                    {
                        Amount = (decimal)i.Amount,
                        Amount2 = i.Amount2.HasValue ? (decimal)i.Amount2.Value : 0,
                        AttachedTransactionId = i.AttachedTransactionId,
                        CreatedByApp = i.CratedByApplication.HasValue ? i.CratedByApplication.Value : (int)eApplication.SyncService,
                        CreatedByUserId = i.CratedByUserId,
                        DateAddedToDB = i.DateAddedToDB.HasValue ? i.DateAddedToDB.Value : DateTime.Now,
                        DateCreated = i.DateCreated,
                        DateDeleted = i.DateDeleted,
                        DatePayed = i.DatePayed,
                        DateUsed = i.DateUsed,
                        Description = i.Description == null ? string.Empty : i.Description,
                        Id = i.TransactionId,
                        TransactionType = i.TransactionType,
                        UserId = i.UserId
                    };

                    ClubArcada.Common.BusinessObjects.Data.TransactionData.Save(CR, newT);
                }
            }
        }

        public static void SyncStructures()
        {
            var oldItems = OldData.Data.OldDbData.GetStructures();
            var newItems = Common.BusinessObjects.Data.StructureData.GetList(CR);

            foreach (var r in oldItems)
            {
                if (newItems.Select(x => x.Id).Contains(r.StructureId))
                {
                    var toUpdate = newItems.SingleOrDefault(y => y.Id == r.StructureId);

                    toUpdate.Name = r.Name;
                    Common.BusinessObjects.Data.StructureData.Save(CR, toUpdate);
                }
                else
                {
                    Common.BusinessObjects.Data.StructureData.Save(CR, new Structure()
                    {
                        Id = r.StructureId,
                        Name = r.Name,
                        DateDeleted = r.DateDeleted,
                        DateCreated = r.DateCreated,
                    });
                }
            }
        }

        public static void SyncStructureDetails()
        {
            var oldItems = OldData.Data.OldDbData.GetStructureDetails();
            var newItems = Common.BusinessObjects.Data.StructureDetailData.GetList(CR);
            var structures = Common.BusinessObjects.Data.StructureData.GetList(CR);

            foreach (var r in oldItems)
            {
                if (structures.Select(s => s.Id).Contains(r.StructureId))
                {
                    if (newItems.Select(x => x.Id).Contains(r.StructureDetailId))
                    {
                        var toUpdate = newItems.SingleOrDefault(y => y.Id == r.StructureDetailId);

                        toUpdate.Ante = r.Ante;
                        toUpdate.BB = r.BigBlind;
                        toUpdate.Level = r.Level;
                        toUpdate.Number = r.Number;
                        toUpdate.SB = r.SmallBlind;
                        toUpdate.StructureId = r.StructureId;
                        toUpdate.Time = r.Time;
                        toUpdate.Type = r.Type;

                        Common.BusinessObjects.Data.StructureDetailData.Save(CR, toUpdate);
                    }
                    else
                    {
                        var newStructureDetail = new StructureDetail()
                        {
                            Id = r.StructureDetailId,
                            Ante = r.Ante,
                            BB = r.BigBlind,
                            Level = r.Level,
                            Number = r.Number,
                            SB = r.SmallBlind,
                            StructureId = r.StructureId,
                            Time = r.Time,
                            Type = r.Type
                        };

                        Common.BusinessObjects.Data.StructureDetailData.Save(CR, newStructureDetail);
                    }
                }
            }
        }

        public static void SyncTickets()
        {
            var oldList = OldData.Data.OldDbData.GetTickets();
            var newList = Common.BusinessObjects.Data.TicketData.GetList(CR);

            foreach (var r in oldList)
            {
                if (newList.Select(x => x.Id).Contains(r.Id))
                {
                }
                else
                {
                    Common.BusinessObjects.Data.TicketData.Save(CR, new Ticket()
                    {
                        Id = r.Id,
                        TournamentId = r.TournamentId,
                        DateCreated = r.DateCreated,
                        UserId = r.UserId,
                        CreatedByUserId = r.CreatedByUserId,
                        Identification = r.Identification,
                        DateDeleted = null
                    });
                }
            }
        }

        public static void SyncTicketItems()
        {
            var oldList = OldData.Data.OldDbData.GetTicketItems();
            var newList = Common.BusinessObjects.Data.TicketItemData.GetList(CR);

            foreach (var r in oldList)
            {
                if (newList.Select(x => x.Id).Contains(r.Id))
                {
                }
                else
                {
                    Common.BusinessObjects.Data.TicketItemData.Save(CR, new TicketItem()
                    {
                        Id = r.Id,
                        Amount = r.Amount,
                        Name = r.Name,
                        Stack = r.Stack,
                        TicketId = r.TicketId,
                        Type = r.Type,
                    });
                }
            }
        }

        public static void SyncCashGames()
        {
            var oldTours = OldData.Data.OldDbData.GetCashGames().ToList();

            var newTours = Common.BusinessObjects.Data.CashGameData.GetList(CR, false, false);

            foreach (var u in oldTours)
            {
                if (newTours.Select(nt => nt.Id).Contains(u.TournamentId))
                {
                    var tu = newTours.SingleOrDefault(n => n.Id == u.TournamentId);

                    if (tu.Date != u.Date || tu.DateEnded != u.DateEnded || tu.IsRunning != u.IsRunning || tu.Name != u.Name)
                    {
                        tu.DateDeleted = u.DateDeleted;
                        tu.DateEnded = u.DateEnded;
                        tu.IsRunning = u.IsRunning.True();
                        tu.Name = u.Name;

                        ClubArcada.Common.BusinessObjects.Data.CashGameData.Save(CR, tu);
                    }
                }
                else
                {
                    var newCashGame = new CashGame()
                    {
                        Id = u.TournamentId,
                        Date = u.Date,
                        DateCreated = u.DateCreated,
                        DateDeleted = u.DateDeleted,
                        DateEnded = u.DateEnded.HasValue ? u.DateEnded.Value : DateTime.Now.AddYears(-1),
                        Description = u.Description.IsNullOrEmpty() ? string.Empty : u.Description,
                        Name = u.Name,
                        LeagueId = u.LeagueId,
                        IsRunning = u.IsRunning.True(),
                        IsPublic = false,
                        CreatedByUserId = u.CreatedByUserId,
                        GameType = (int)u.GameType.ToGameType(),
                    };

                    ClubArcada.Common.BusinessObjects.Data.CashGameData.Save(CR, newCashGame);
                }
            }
        }

        public static void SyncCashStates()
        {
            var oldList = OldData.Data.OldDbData.GetCashStates();
            var newList = Common.BusinessObjects.Data.CashStateData.GetList(CR, false, false);

            foreach (var r in oldList)
            {
                if (newList.Select(x => x.Id).Contains(r.Id))
                {
                    var toUpdate = newList.SingleOrDefault(y => y.Id == r.Id);

                    if (toUpdate.ModifiedByUserId != r.ModifiedByUserId || toUpdate.Jackpot != r.Jackpot || toUpdate.Rake != r.Rake || toUpdate.State != r.State)
                    {
                        toUpdate.ModifiedByUserId = r.ModifiedByUserId;
                        toUpdate.Jackpot = r.Jackpot;
                        toUpdate.Rake = r.Rake;
                        toUpdate.State = r.State;

                        Common.BusinessObjects.Data.CashStateData.Save(CR, toUpdate);
                    }
                }
                else
                {
                    var cashGame = Common.BusinessObjects.Data.CashGameData.GetById(CR, r.TournamentId);

                    if (cashGame.IsNotNull())
                    {
                        Common.BusinessObjects.Data.CashStateData.Save(CR, new CashState()
                        {
                            Id = r.Id,
                            CreatedByUserId = r.CreatedByUserId,
                            DateCreated = r.DateCreated,
                            Input = r.Input,
                            Jackpot = r.Jackpot,
                            LastInput = r.LastInput,
                            ModifiedByUserId = r.ModifiedByUserId,
                            Rake = r.Rake,
                            State = r.State,
                            CashGameId = r.TournamentId,
                            DateDeleted = null,
                        });
                    }
                }
            }
        }

        public static void SyncCashPlayers()
        {
            var oldList = OldData.Data.OldDbData.GetCashResults();
            var newList = Common.BusinessObjects.Data.CashPlayerData.GetList(CR, false, false);

            List<CashPlayer> listToCreate = new List<CashPlayer>();
            var cashGames = Common.BusinessObjects.Data.CashGameData.GetList(CR, false, false);

            foreach (var o in oldList)
            {
                if (newList.Select(tp => tp.Id).Contains(o.CashResultId))
                {
                    var toUpdate = newList.SingleOrDefault(tp => tp.Id == o.CashResultId);
                    if (toUpdate.State != o.State || toUpdate.Points != o.Duration)
                    {
                        toUpdate.State = o.State.HasValue ? o.State.Value : 0;
                        toUpdate.Points = o.Duration;

                        ClubArcada.Common.BusinessObjects.Data.CashPlayerData.Save(CR, toUpdate);
                    }
                }
                else
                {
                    if (cashGames.Select(c => c.Id).Contains(o.CashResultId))
                    {
                        var newPlayer = new CashPlayer()
                        {
                            Id = o.CashResultId,
                            CashGameId = o.TournamentId,
                            CashTableId = o.CashTableId.HasValue ? o.CashTableId.Value : Guid.Empty,
                            DateCreated = o.StartTime.HasValue ? o.StartTime.Value : DateTime.UtcNow,
                            UserId = o.UserId,
                            State = o.State.HasValue ? o.State.Value : 0,
                            StartTime = o.StartTime.HasValue ? o.StartTime.Value : DateTime.UtcNow,
                            EndTime = o.EndTime.HasValue ? o.EndTime.Value : DateTime.UtcNow,
                            Points = o.Duration,
                        };

                        listToCreate.Add(newPlayer);
                    }
                }
            }

            if (listToCreate.Any())
            {
                ClubArcada.Common.BusinessObjects.Data.CashPlayerData.SaveAll(CR, listToCreate);
            }

        }

        public static void SendErrorMail(string subject, string message)
        {
            var o = new Common.Mailer.MailObject()
            {
                Subject = subject,
                Body = message,
                To = "petervarga@arcade-group.sk".CreateList(),
                CC = new System.Collections.Generic.List<string>(),
                From = "service@arcade-group.sk",
                Password = "vape6931",
                SmtpClient = "smtp.websupport.sk",
                Port = 25,
                UserName = "service@arcade-group.sk"
            };

            ClubArcada.Common.Mailer.Mailer.SendMail(o);
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