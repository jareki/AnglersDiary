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
    /// Interaction logic for ChangeTrophyWindow.xaml
    /// </summary>
    public partial class AddTrophyWindow
    {
        Photo photo;

        public AddTrophyWindow()
        {
            InitializeComponent();            
        }

        private void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void SelectImg_Click(object sender, RoutedEventArgs e)
        {
            if (photo.FindImg())
            {
                Img.Source = photo.Thumbnail;
                (DataContext as AddTrophyViewModel).ImageSource = photo.FromPath.AbsolutePath;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            photo = new Photo();
        }
    }
}
