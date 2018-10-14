using AnglersDiary.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Compression;
using AnglersDiary.CS;
using AnglersDiary.Models;
using System.Text.RegularExpressions;

namespace AnglersDiary.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        CheckBox[] search_checks;
        ComboBox[] search_combos;
        string[] search_names =
        {
            "По времени суток:", "По Т воздуха:", "По Т воды:", "По погоде:", "По прозрачности:", "По уровню воды:",
            "По силе течения:", "По типу дна:", "По мусору", "По глубине:", "По дальности:"
        };
        string[][] search_params =
        {
            new string[] { "утро (0-9)", "день (9-15)", "вечер (15-0)" },
            new string[] { "ниже 0", "0-10", "10-20", "20-30", "выше 30" },
            new string[] { "ниже 5","5-10", "10-15", "15-20", "20-25", "выше 25" },
            new string[] { "ясно", "переменная", "пасмурно", "осадки" },
            new string[] { "прозрачная", "нормальная", "мутная" },
            new string[] { "низкая", "нормальная", "высокая" },
            new string[] { "нет", "медленное", "среднее", "быстрое" },
            new string[] { "Ил", "песок", "камни", "ракушка", "глина", "трава" },
            new string[] { "нет", "мало", "много" },
            new string[] { "менее 1м", "1-2м", "2-3м", "более 3м" },
            new string[] { "менее 5м", "5-10м", "10-20м", "дальше 20м" }
        };
        string[][] search_queries =
        {
            new string[] { "StartTime < '0001-01-01 09:00:00'", "StartTime BETWEEN '0001-01-01 09:00:00' AND '0001-01-01 15:00:00'", "StartTime >'0001-01-01 15:00:00'" },
            new string[] { "(TempMin+TempMax)/2<0", "(TempMin+TempMax)/2 BETWEEN 0 AND 10", "(TempMin+TempMax)/2 BETWEEN 11 AND 20", "(TempMin+TempMax)/2 BETWEEN 21 AND 30", "(TempMin+TempMax)/2 >30" },
            new string[] { "TempWater<5", "TempWater BETWEEN 5 AND 10", "TempWater BETWEEN 11 AND 15", "TempWater BETWEEN 16 AND 20", "TempWater BETWEEN 21 AND 25", "TempWater>25" },
            new string[] {"Cloud='Ясно'", "Cloud='Переменная'", "Cloud='Пасмурно'", "Cloud='Осадки'"},
            new string[] {"WaterTransparency='Прозрачная'", "WaterTransparency='Нормальная'", "WaterTransparency='Мутная'" },
            new string[] {"WaterHeight='Низкая'", "WaterHeight='Нормальная'", "WaterHeight='Высокая'"},
            new string[] { "Flow='нет'", "Flow='Медленное'", "Flow='Среднее'", "Flow='Быстрое'"},
            new string[] {"Bottom='Ил'", "Bottom='Песок'", "Bottom='Камни'", "Bottom='Ракушка'", "Bottom='Глина'", "Bottom='Трава'"},
            new string[] {"Waste='Нет'", "Waste='Мало'", "Waste='Много'"},
            new string[] {"Depth<1", "Depth BETWEEN 1 AND 2", "Depth BETWEEN 2.01 AND 3", "Depth>3"},
            new string[] {"Distance<5", "Distance BETWEEN 5 AND 10", "Distance BETWEEN 11 AND 20", "Distance>20"}
        };

        public NoteViewModel NoteModel { get; set; }
        public MainViewModel MainModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            
            search_checks = new CheckBox[11];
            search_combos = new ComboBox[11];                     
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainModel = new MainViewModel();
            DataContext = MainModel;
            NoteModel = new NoteViewModel();
            NoteModel.SelectByDate(DateTime.Now);           
            DatesPanel.DataContext = NoteModel;
            NoteModel.SelectByDate(DateTime.Now);
            CalendarUpdate();

            SpecyCombo.DataContext = new ShowAllSpeciesViewModel();

            for (int i=0;i<11;i++)
            {
                search_checks[i] = new CheckBox();
                search_checks[i].Name = $"Check{i}";
                search_checks[i].Tag = "";
                search_checks[i].Content = search_names[i];
                search_checks[i].SetValue(Grid.RowProperty, i + 6);

                search_combos[i] = new ComboBox();
                search_combos[i].Name = $"Combo{i}";
                search_combos[i].Tag = "";
                search_combos[i].SetValue(Grid.RowProperty, i + 6);
                search_combos[i].SetValue(Grid.ColumnProperty, 1);
                
                foreach (var str in search_params[i])
                {
                    search_combos[i].Items.Add(str);
                }
                SearchGrid.Children.Add(search_checks[i]);
                SearchGrid.Children.Add(search_combos[i]);
            }
        }

        private void CalendarUpdate()
        {
            CalenderBackground background;
            background = new CalenderBackground(MainCalendar);

            background.AddOverlay("blue", $"Assets\\blue.png");

            var model = new NoteViewModel();
            var dates = model.GetDates();
            foreach (var date in dates)
            {
                background.AddDate(date, "blue");
            }

            MainCalendar.Background = background.GetBackground();
            MainCalendar.DisplayDateChanged += (sender, e) =>  MainCalendar.Background = background.GetBackground();
            MainCalendar.DisplayModeChanged += (s, e) => MainCalendar.Background = background.GetBackground();
        }

        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)e.Source;
            switch (item.Header.ToString())
            {
                case "_Обновить":
                    CalendarUpdate();
                    break;
                case "_Выход":
                    Close();
                    break;
                case "_Экспорт":
                    MainBusy.IsBusy = true;
                    await MainModel.ExportCommand.ExecuteAsync(null);
                    MainBusy.IsBusy = false;
                    break;
                case "_Импорт":
                    MainBusy.IsBusy = true;
                    await MainModel.ImportCommand.ExecuteAsync(null);
                    MainBusy.IsBusy = false;
                    break;
                case "_О программе":
                    AboutWindow wnd = new AboutWindow();
                    wnd.ShowDialog();
                    break;
            }
        }

        private void MainCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            //NoteListBox.Items.Clear();
            if (MainCalendar.SelectedDate.HasValue)
            {                
                NoteModel.SelectByDate(MainCalendar.SelectedDate.Value);
            }
            NoteListBox.Focus();
        }

        private void AddFishingBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void NoteListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (NoteListBox.SelectedItem == null) return;
            NoteDoc wnd = new NoteDoc(NoteListBox.SelectedItem as Note);
            wnd.ShowDialog();
            CalendarUpdate();
            MainCalendar_SelectedDatesChanged(MainCalendar, null);
        }

        private void NoteListBox2_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (NoteListBox2.SelectedItem == null) return;
            Note note = NoteListBox2.SelectedItem as Note;
            NoteDoc wnd = new NoteDoc(note);
            wnd.ShowDialog();
            CalendarUpdate();
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            var model = new NoteViewModel();
            string query = "SELECT Id FROM Notes WHERE ";

            for (int i=0;i<11;i++)
            {
                if (search_checks[i].IsChecked==true)
                {
                    query += search_queries[i][search_combos[i].SelectedIndex];
                    query += " AND ";
                }
            }
            if (TextCheck.IsChecked == true)
                query += $"(WeatherNote LIKE '%{TextTxt.Text}%' OR WaterNote LIKE '%{TextTxt.Text}%' OR NoteString LIKE '%{TextTxt.Text}%') AND ";
            if (LocationCheck.IsChecked == true)
                query += $"Location_id = {(int)LocationTb.Tag} AND ";
            if (RainfallCheck.IsChecked == true)
                query += "Rainfall=1 AND ";
            if (SpecyCheck.IsChecked == true)
            {
                if (TrophySpecyCheck.IsChecked == true)
                    query += $"(Id IN (SELECT Note_id FROM Trophies WHERE Specy_id={((Specy)SpecyCombo.SelectedItem).Id})) AND ))";
                else
                    query += $"(Id IN (SELECT Note_id FROM Catches WHERE Specy_id={((Specy)SpecyCombo.SelectedItem).Id}) OR "
                        + $"Id IN (SELECT Note_id FROM Trophies WHERE Specy_id={((Specy)SpecyCombo.SelectedItem).Id})) AND ";
            }
            if (TackleCheck.IsChecked == true)
            {
                if (string.IsNullOrEmpty(TackleParamTxt.Text))
                    query += $"Id IN (SELECT Note_id FROM NoteTackles WHERE Tackle_id={(int)TackleTb.Tag}) AND ";
                else
                    query += $"Id IN (SELECT Note_id FROM NoteTackles WHERE Tackle_id={(int)TackleTb.Tag} AND Parameter LIKE '%{TackleParamTxt.Text}%') AND ";
            }
            if (CatchCountCheck.IsChecked==true)
            {
                query += $"CatchCount BETWEEN {(int)(FromCatchCountUpDown.Value ?? 0)} AND {(int)(ToCatchCountUpDown.Value ?? 0)} AND ";
            }

            try
            {
                query = query.Substring(0, query.LastIndexOf(" AND"));
                query += " ORDER BY date(Date) ASC";

                NoteListBox2.DataContext = model;
                //NoteListBox2.Items.Clear();
                model.SQL(query);
                //model.Refresh();

                //notes.ForEach(v => NoteListBox2.Items.Add(model.Notes.FirstOrDefault(n => n.Id == v.Id)));
            }
            catch { MessageBox.Show("Ошибка в параметрах поиска!"); }
            if (NoteListBox2.Items.Count > 0)
                SearchExcelBtn.IsEnabled = true;
            else
                SearchExcelBtn.IsEnabled = false;
        }

        private void SelectLocationBtn_Click(object sender, RoutedEventArgs e)
        {
            LocationWindow wnd = new LocationWindow(true);
            if (wnd.ShowDialog() == true)
            {
                LocationTb.Tag = wnd.SelectLocation.Id;
                LocationTb.Text = wnd.SelectLocation.Name;
            }
        }

        private void SelectTackleBtn_Click(object sender, RoutedEventArgs e)
        {
            TackleWindow wnd = new TackleWindow(true);
            if (wnd.ShowDialog()==true)
            {
                TackleTb.Tag = wnd.SelectedTackle.Id;
                TackleTb.Text = wnd.SelectedTackle.Name;
            }
        }

        private async void ExcelBtn_Click(object sender, RoutedEventArgs e)
        {
            MainBusy.IsBusy = true;
            var notemodel = new NoteViewModel();
            string period = string.Empty;
            var mainModel = DataContext as MainViewModel;

            if (MonthStatRadio.IsChecked == true) //Month
            {
                if (MonthStatCombo.SelectedIndex == -1) return;
                int month = MonthStatCombo.SelectedIndex + 1;
                notemodel.SelectByMonth(month);
                mainModel.ExcelFileName = MonthStatCombo.SelectedItem.ToString();
            }
            if (YearStatRadio.IsChecked == true) //Year
            {
                int year = (int)(YearStatUpDown.Value ?? DateTime.Now.Year);
                notemodel.SelectByYear(year);
                mainModel.ExcelFileName = year.ToString();
            }            
            await mainModel.ExcelStatCommand.ExecuteAsync(notemodel.Notes);
                
            MainBusy.IsBusy = false;
        }

        private void Window_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);
            //release the mouse from naughty calendar!!
            if (Mouse.Captured is Calendar || Mouse.Captured is System.Windows.Controls.Primitives.CalendarItem)
            {
                Mouse.Capture(null);
            }
        }

        private void SelectSpecyBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        

        private async void SearchExcelBtn_Click(object sender, RoutedEventArgs e)
        {
            MainBusy.IsBusy = true;

            var notes = NoteListBox2.Items.Cast<Note>().ToList();
            if (notes == null || notes.Count == 0)
            {
                MainBusy.IsBusy = false;
                return;
            }
            await MainModel.ExcelStatCommand.ExecuteAsync(notes);
            MainBusy.IsBusy = false; 
        }
    }
}
