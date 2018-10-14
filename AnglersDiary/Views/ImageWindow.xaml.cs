using AnglersDiary.Models;
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
    /// Interaction logic for ImageWindow.xaml
    /// </summary>
    public partial class ImageWindow
    {
        public ImageWindow(string capiton, Photo image)
        {
            InitializeComponent();
            
            Img.Source = image.Image;
            Height = image.Image.Height / image.Image.Width * Width;
            Title = capiton;
        }

        private void ImageWindow_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
