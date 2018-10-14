using AnglersDiary.Models;
using AnglersDiary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace AnglersDiary.CS
{
    class ExcelBook: IDisposable
    {
        Excel.Application app;
        //Excel.Window wnd;
        Excel.Workbook workbook;
        Excel.Worksheet worksheet;

        public ExcelBook()
        {
            app = new Excel.Application();
            app.Workbooks.Add(Type.Missing);
            workbook = app.Workbooks[1];
            worksheet = workbook.ActiveSheet;
        }

        public void CalculateStat(List<Note> notes, string period)
        {
            List<string> dates = new List<string>();

            foreach (var note in notes)
            {
                dates.Add(note.Date.ToString("dd-MM-yy"));
            }

            var ids = from n in notes select n.Id;
            var catchmodel = new ShowAllCatchesViewModel(ids.ToList());
            var trophymodel = new ShowAllTrophiesViewModel(ids.ToList());
            var locationmodel = new LocationViewModel();

            double totallength = catchmodel.GetTotalLength() + trophymodel.GetTotalLength();

            DisplayCommon(period, notes.Count, notes.Sum(p => (p.EndTime - p.StartTime).TotalHours),
                notes.Sum(p => p.CatchCount), notes.Sum(p => p.CatchWeight), totallength);

            int row = 10;

            DrawPointChart("T средняя", row, "дата", "t", dates.ToArray(),
                notes.Select(p => (p.TempMax + p.TempMin) / 2.0).ToArray());
            DrawPointChart("T воды", row=row+=19, "дата", "t", dates.ToArray(), notes.Select(p => (double)p.TempWater).ToArray());
            DrawPointChart("Давление", row = row += 19, "дата", "мм", dates.ToArray(), notes.Select(p => (double)p.Pressure).ToArray());
            DrawPointChart("Глубина", row = row += 19, "дата", "м", dates.ToArray(), notes.Select(p => p.Depth).ToArray());
            DrawPointChart("Дистанция", row = row += 19, "дата", "м", dates.ToArray(), notes.Select(p => (double)p.Distance).ToArray());
            DrawPointChart("Улов", row = row += 19, "дата", "кол-во", dates.ToArray(), notes.Select(p => (double)p.CatchCount).ToArray());

            //LOCATIONS            
            var locations = locationmodel.Top(notes, 5);
            DrawBarChart("ТОП 5 мест", row = row + 20, "Места", "Количество", locations);
            row = row + 25;

            //TACKLES
            ShowAllNoteTacklesViewModel notetackleviewmodel;
            ShowAllTackleCategoriesViewModel tackleCategoryViewModel = new ShowAllTackleCategoriesViewModel();
            List<Tackle> tackleslist = new List<Tackle>();
            foreach (var note in notes)
            {
                notetackleviewmodel = new ShowAllNoteTacklesViewModel(note);
                notetackleviewmodel.NoteTackles.ToList().ForEach(p => tackleslist.Add(p.Tackle));
            }

            var grouptackles = from t in tackleslist
                               from c in tackleCategoryViewModel.Categories
                               where t.TackleCategory_id == c.Id
                               group t by c.Name into group_by_category
                               orderby group_by_category.Key
                               from x in group_by_category
                                         .GroupBy(n => n.Name)
                                         .Select(n => new
                                         {
                                             Name = n.Key,
                                             Count = n.Count()
                                         })
                                         .OrderByDescending(n=>n.Count)
                                         .Take(5)
                               group x by group_by_category.Key;

            foreach (var cat in grouptackles)
            {
                var tackles = cat.Select(n => n).ToDictionary(n=>n.Name,n=>n.Count);
                DrawBarChart($"{cat.Key} ТОП-5", row, "Снасти", "Количество рыбалок", tackles);
                row += 22;
            }

            //GROUP notes count BY WEATHER
            var weather_group = (from n in notes
                                 group n by n.Cloud into g
                                 select new { Name = g.Key, Count = g.Count() }).ToDictionary(t => t.Name, t => t.Count);
            DrawBarChart("По погоде", row, "Условия", "Количество рыбалок", weather_group);

            //Group notes count by daytime
            var daytime_group = (from n in notes
                                 group n by new { Flag = n.StartTime.Hour < 12 ? "утро" : "вечер" } into g
                                 select new { Name = g.Key, Count = g.Count() }).ToDictionary(t => t.Name.Flag, t => t.Count);

            DrawBarChart("По времени суток", row+=22, "Условия", "Количество рыбалок", daytime_group);

            //Group notes count by flow
            var flow_group = (from n in notes
                              group n by n.Flow into g
                              select new { Name = g.Key, Count = g.Count() }).ToDictionary(t => t.Name, t => t.Count);
            DrawBarChart("По течению", row+=22, "течение", "Количество рыбалок", flow_group);

            //Group notes count by bottom
            var bottom_group = (from n in notes
                                group n by n.Bottom into g
                                orderby g.Count() descending
                                select new { Name = g.Key, Count = g.Count() }).ToDictionary(t => t.Name, t => t.Count);
            DrawBarChart("По дну", row+=22, "Дно", "Количество рыбалок", bottom_group);

            //Group catches avg length and catches count by species
            Dictionary<string, int> catch_counts = new Dictionary<string, int>();
            Dictionary<string, int> avg_lenghts = new Dictionary<string, int>();

            var specymodel = new ShowAllSpeciesViewModel();
            foreach (var specy in specymodel.Species)
            {
                totallength = catchmodel.GetTotalLength(c => c.Specy.Name == specy.Name);
                int totalcount = catchmodel.GetCount(c => c.Specy.Name == specy.Name);

                var trophies = trophymodel.Trophies.Where(tr => tr.Specy.Name == specy.Name).ToList();
                totallength += trophies.Sum(tr => tr.Size);
                totalcount += trophies.Count();
                if (totalcount != 0)
                {
                    catch_counts.Add(specy.Name, totalcount);
                    avg_lenghts.Add(specy.Name, (int)(totallength / totalcount));
                }
            }
            DrawBarChart("По рыбам", row+=22, "Вид", "Количество", catch_counts);
            DrawBarChart("По длине рыб", row+=22, "Вид", "Средняя длина", avg_lenghts);
        }

        public void DisplayCommon(string period, int fishingCount, double hours, int catchCount, double catchWeight, double catchLength)
        {
            worksheet.Cells[1, "A"] = "период";
            worksheet.Cells[1, "B"] = period;

            worksheet.Cells[2, "A"] = "количество рыбалок";
            worksheet.Cells[2, "B"] = fishingCount;

            worksheet.Cells[3, "A"] = "проведенное время в часах";
            worksheet.Cells[3, "B"] = (int)hours;

            worksheet.Cells[4, "A"] = "среднее время рыбалки в часах";
            worksheet.Cells[4, "B"] = $"{(hours / fishingCount):#.##}";

            worksheet.Cells[5, "A"] = "Всего поймано штук";
            worksheet.Cells[5, "B"] = catchCount;

            worksheet.Cells[6, "A"] = "всего поймано кг";
            worksheet.Cells[6, "B"] = catchWeight;

            worksheet.Cells[7, "A"] = "среднее количество рыб за рыбалку";
            worksheet.Cells[7, "B"] = $"{catchCount/(double)fishingCount:#.##}";

            worksheet.Cells[8, "A"] = "среднее количество рыб за час";
            worksheet.Cells[8, "B"] = $"{catchCount / hours:#.##}";

            worksheet.Cells[9, "A"] = "средняя длина рыбы";
            worksheet.Cells[9, "B"] = $"{catchLength / catchCount:#.##}";

            var cells = worksheet.Range["A1", "B8"];
            cells.Borders.ColorIndex = 1;
            cells.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            cells.Borders.Weight = Excel.XlBorderWeight.xlThin;
        }

        public void DrawTable(string name, int row, string[] columnNames, string[] CSValues)
        {
            int firstrow = row;
            var header = worksheet.Range[worksheet.Cells[firstrow, 1], worksheet.Cells[firstrow, columnNames.Length + 1]];
            header.Merge(Type.Missing);
            header.HorizontalAlignment = Excel.Constants.xlCenter;
            worksheet.Cells[row++, "A"] = name;

            int column = 1;
            worksheet.Cells[row, column++] = "№";
            foreach (string columnname in columnNames)
            {
                worksheet.Cells[row, column].NumberFormat = "@";
                worksheet.Cells[row, column++] = columnname;
            }
            row++;

            int num = 1;
            foreach (string value in CSValues)
            {
                string[] rowvalues = value.Split(';');
                worksheet.Cells[row, 1] = num++;
                column = 2;
                foreach (string val in rowvalues)
                {
                    worksheet.Cells[row, column++] = val;
                }
                row++;
            }

            var cells = worksheet.Range[worksheet.Cells[firstrow,1],worksheet.Cells[row-1,column-1]];//[$"A{firstrow}", $"R{row}C{column}"];
            cells.Borders.ColorIndex = 1;
            cells.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            cells.Borders.Weight = Excel.XlBorderWeight.xlThin;
        }

        public void DrawPointChart(string chartname, int row, string xname, string yname, string[] xvalues, double[] yvalues)
        {
            var v = string.Join(";", yvalues);

            DrawTable(chartname, row, xvalues, new string[] { v });

            var chartobjs = worksheet.ChartObjects(Type.Missing);
            var chartobj = chartobjs.Add(20, 15 * row + 45, 500, 200);
            chartobj.Chart.ChartWizard(
                worksheet.Range[worksheet.Cells[row + 1, 2], worksheet.Cells[row + 2, yvalues.Length + 1]],
                Excel.XlChartType.xlLineMarkersStacked, 2, Excel.XlRowCol.xlRows, Type.Missing, 0, false, 
                chartname, xname, yname, Type.Missing);
        }

        public void DrawBarChart(string chartname, int row, string xname, string yname, Dictionary<string, int> vals)
        {
            var v = string.Join(";", vals.Values.ToArray());

            DrawTable(chartname, row, vals.Keys.ToArray(), new string[] { v });

            var chartobjs = worksheet.ChartObjects(Type.Missing);
            var chartobj = chartobjs.Add(20, 15 * row + 40, 500, 250);
            chartobj.Chart.ChartWizard(
                worksheet.Range[worksheet.Cells[row + 1, 2], worksheet.Cells[row + 2, vals.Values.Count + 1]],
                Excel.XlChartType.xlColumnStacked, 5, Excel.XlRowCol.xlRows, Type.Missing, 0, false,
                chartname, xname, yname, Type.Missing);
        }

        public void Save(string filename)
        {
            workbook.Saved = true;
            app.DisplayAlerts = false;
            app.DefaultSaveFormat = Excel.XlFileFormat.xlOpenXMLWorkbook;
            workbook.SaveAs(filename, Excel.XlFileFormat.xlOpenXMLWorkbook, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            //app.Windows[1].Close();
        }

        public void Close()
        {
            workbook.Close(false);
        }

        public void Dispose()
        {
            app?.Quit();
        }
    }
}
