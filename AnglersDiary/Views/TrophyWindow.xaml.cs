using AnglersDiary.Models;
using AnglersDiary.ViewModels;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for TrophyWindow.xaml
    /// </summary>
    public partial class TrophyWindow
    {
        public TrophyWindow()
        {
            InitializeComponent();
            SpecyPanel.DataContext = new ShowAllSpeciesViewModel();
            SpecyCombo.SelectedIndex = 0;
        }

        private void SpecyCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SpecyCombo.SelectedItem != null)
            {
                DataContext = new ShowAllTrophiesViewModel((Specy)SpecyCombo.SelectedItem);
            }
        }        
    }
}
