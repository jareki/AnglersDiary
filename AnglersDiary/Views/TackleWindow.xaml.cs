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
    /// Interaction logic for TackleWindow.xaml
    /// </summary>
    public partial class TackleWindow
    {
        public Tackle SelectedTackle
        {
            get { return TackleListBox.SelectedItem as Tackle; }
        }

        public TackleWindow(bool isselectmode=false)
        {
            InitializeComponent();
            CategoryPanel.DataContext = new ShowAllTackleCategoriesViewModel();
            CategoryCombo.SelectedIndex = 0;

            if (isselectmode)
                ButtonsPanel.Visibility = Visibility.Visible;
            else
                ButtonsPanel.Visibility = Visibility.Collapsed;
        }

        private void CategoryCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryCombo.SelectedItem != null)
                DataContext = new ShowAllTacklesViewModel((TackleCategory)CategoryCombo.SelectedItem);
        }

        private void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void TackleListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ButtonsPanel.Visibility == Visibility.Visible) //ischangemode==false
                AcceptBtn_Click(this, null);
        }
    }
}
