using AnglersDiary.Models;
using AnglersDiary.ViewModels;
using MahApps.Metro.Controls;
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
using System.Windows.Shapes;

namespace AnglersDiary.Views
{
    /// <summary>
    /// Interaction logic for ChangeNoteWindow.xaml
    /// </summary>
    public partial class EditNoteWindow
    {
        public EditNoteWindow()
        {
            InitializeComponent();            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {           
            LocationGallery.LoadFromFolder((DataContext as EditNoteViewModel).Note.Photos, true);            
        }

        private void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {
            /*
            Note.Bottom = BottomCombo.Text ?? "Песок";            
            Note.CatchWeight = CatchWeightUpDown.Value ?? 0;
            Note.CatchCount = ((ShowAllCatchesViewModel)(CatchPanel.DataContext)).GetCount() 
                + ((ShowAllTrophiesViewModel)(TrophyPanel.DataContext)).GetCount();
            Note.Cloud = (CloudCombo.SelectedItem as Icon)?.Text ?? "Ясно";
            Note.Date = (DateTime)DatePick.SelectedDate;
            Note.Depth = DepthUpDown.Value ?? 0.0;
            Note.Distance = (int)(DistanceUpDown.Value ?? 0);
            Note.EndTime = new DateTime(1,1,1,0,0,0,0) + EndTimePick.SelectedTime.Value;
            Note.Flow = FlowCombo.Text;
            Note.Moon = (MoonCombo.SelectedItem as Icon)?.Text ?? "Полнолуние";
            Note.NoteString = NoteStringTxt.Text;
            Note.Rainfall = RainfallCheck.IsChecked ?? false;
            Note.Pressure = (int)(PressureUpDown.Value ?? 0);
            Note.StartTime = new DateTime(1, 1, 1, 0, 0, 0, 0) + StartTimePick.SelectedTime.Value;
            Note.TempMax = (int)(TMaxUpDown.Value ?? 0);
            Note.TempMin = (int)(TMinUpDown.Value ?? 0);
            Note.TempWater = (int)(TWaterUpDown.Value ?? 0);
            Note.Waste = WasteCombo.Text;
            Note.WaterHeight = WaterHeightCombo.Text;
            Note.WaterNote = WaterNoteTxt.Text;
            Note.WaterTransparency = WaterTransparencyCombo.Text;
            Note.WeatherNote = WeatherNoteTxt.Text;
            Note.WindDir = WindDirCombo.Text;
            Note.WindSpeed = (int)(WindSpeedUpDown.Value ?? 0);
            */
            LocationGallery.SaveToFolder((DataContext as EditNoteViewModel).Note.Photos);
            DialogResult = true;
        }
                
        private void TackleGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
                DelTackleBtn.Command.Execute(TackleGrid.SelectedItems);
        }

        private void TrophyGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
                DelTrophyBtn.Command.Execute(TrophyGrid.SelectedItems);
        }

        private void CatchGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Delete)
                DelCatchBtn.Command.Execute(CatchGrid.SelectedItems);
        }
        
        private void TrophyGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TrophyGrid.SelectedItems.Count==1)
            {
                EditTrophyBtn.IsEnabled = true;
                DelTrophyBtn.IsEnabled = true;
            }
            else if (TrophyGrid.SelectedItems.Count==0)
            {
                EditTrophyBtn.IsEnabled = false;
                DelTrophyBtn.IsEnabled = false;
            }
            else
            {
                EditTrophyBtn.IsEnabled = false;
                DelTrophyBtn.IsEnabled = true;
            }
        }

        private void CatchGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CatchGrid.SelectedItems.Count == 1)
            {
                EditCatchBtn.IsEnabled = true;
                DelCatchBtn.IsEnabled = true;
            }
            else if (CatchGrid.SelectedItems.Count == 0)
            {
                EditCatchBtn.IsEnabled = false;
                DelCatchBtn.IsEnabled = false;
            }
            else
            {
                EditCatchBtn.IsEnabled = false;
                DelCatchBtn.IsEnabled = true;
            }
        }

        private void TackleGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TackleGrid.SelectedItems.Count == 0)
                DelCatchBtn.IsEnabled = false;
            else
                DelTackleBtn.IsEnabled = true;
        }
        
    }
}
