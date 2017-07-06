using ClubArcada.Common.BusinessObjects.DataClasses;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ClubArcada.SyncService.Documents
{
    public class Generator
    {
        public static void GenerateTournamentReport()
        {
            var tournaments = GetTournamentsForGeneration();

            foreach (var t in tournaments)
            {
                Generate(t);
            }
        }

        private static void Generate(Tournament t)
        {
            var tickets = Common.BusinessObjects.Data.TicketData.GetByTournamentId(Settings.CR, t.Id);
            var cashout = Common.BusinessObjects.Data.TournamentCashoutData.GetByTournamentId(Settings.CR, t.Id);
            var players = Common.BusinessObjects.Data.TournamentPlayerData.GetListByTournamentId(Settings.CR, t.Id,true).OrderBy(p => p.Rank).ToList();

            if (!tickets.Any())
                return;

            var places = cashout.Places.Split('|');

            Application xlApp = new Application();
            xlApp.Visible = false;

            var path = AppDomain.CurrentDomain.BaseDirectory;
            var templateExcelFile = path + "\\Documents\\tournament_report_template.xlsx";
            Workbook wb = xlApp.Workbooks.Open(templateExcelFile, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            Worksheet ws = (Worksheet)wb.Worksheets[1];

            ws.get_Range("A2", "A2").Cells.Value2 = "Reference number: " + t.Id.ToString();
            ws.get_Range("P1", "P1").Cells.Value2 = string.Format("generated at {0}", DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
            ws.get_Range("I4", "I4").Cells.Value2 = t.Date.ToString("dd.MM.yyyy HH:mm");
            ws.get_Range("I5", "I5").Cells.Value2 = string.Empty;
            ws.get_Range("I6", "I6").Cells.Value2 = string.Empty;
            ws.get_Range("I7", "I7").Cells.Value2 = players.Count;
            ws.get_Range("I8", "I8").Cells.Value2 = players.Count + players.Sum(i => i.ReEntryCount);
            ws.get_Range("I9", "I9").Cells.Value2 = tickets.Sum(i => i.Stack);

            ws.get_Range("X4", "X4").Cells.Value2 = t.GTD;
            ws.get_Range("X5", "X5").Cells.Value2 = tickets.Sum(i => i.Amount);
            ws.get_Range("X6", "X6").Cells.Value2 = cashout.Dotation;
            ws.get_Range("X7", "X7").Cells.Value2 = cashout.Rake;
            ws.get_Range("X8", "X8").Cells.Value2 = cashout.Prizepool;
            ws.get_Range("X9", "X9").Cells.Value2 = cashout.Food;

            var startRowNumber = 12;

            foreach(var p in players)
            {
                ws.get_Range("A" + startRowNumber, "A" + startRowNumber).Cells.Value2 = p.Rank.ToString("00") + ".";
                ws.get_Range("C" + startRowNumber, "C" + startRowNumber).Cells.Value2 = p.User.DisplyNameWithNickname;
                ws.get_Range("P" + startRowNumber, "P" + startRowNumber).Cells.Value2 = 1 + p.ReEntryCount;
                ws.get_Range("R" + startRowNumber, "R" + startRowNumber).Cells.Value2 = p.ReBuyCount;
                ws.get_Range("T" + startRowNumber, "T" + startRowNumber).Cells.Value2 = p.AddOnCount;
                ws.get_Range("V" + startRowNumber, "V" + startRowNumber).Cells.Value2 = p.SpecialAddOnCount;
                ws.get_Range("X" + startRowNumber, "X" + startRowNumber).Cells.Value2 = p.Points;
                ws.get_Range("Z" + startRowNumber, "Z" + startRowNumber).Cells.Value2 = p.DateCreated.ToString("HH:mm");
                ws.get_Range("AB" + startRowNumber, "AB" + startRowNumber).Cells.Value2 = p.DateDeleted.Value.ToString("HH:mm");

                //ws.get_Range("AD" + startRowNumber, "AD" + startRowNumber).Cells.Value2 = places.Count() > p.Rank ? int.Parse(places[p.Rank - 1]) : 0;

                startRowNumber++;
            }

            var pdfdoc = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\FileSn.pdf";
            wb.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, pdfdoc);

            wb.Close(SaveChanges: false);
        }

        private static List<Tournament> GetTournamentsForGeneration()
        {
           return Common.BusinessObjects.Data.TournamentData.GetTournamentsForDocumentGeneration(Settings.CR);
        }
    }
}
