using AnglersDiary.Models;
using AnglersDiary.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace AnglersDiary.Controls
{
    /// <summary>
    /// Interaction logic for GalleryControl.xaml
    /// </summary>
    public partial class GalleryControl : UserControl
    {
        public ObservableCollection<Photo> Images
        {
            get { return (ObservableCollection<Photo>)GetValue(ImagesProperty); }
            set { SetValue(ImagesProperty, value); }
        }
                
        // Using a DependencyProperty as the backing store for Images.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImagesProperty =
            DependencyProperty.Register("Images", typeof(ObservableCollection<Photo>), typeof(GalleryControl));
        
        public int ImgWidth
        {
            get { return (int)GetValue(ImgWidthProperty); }
            set { SetValue(ImgWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImgWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImgWidthProperty =
            DependencyProperty.Register("ImgWidth", typeof(int), typeof(GalleryControl));



        public Visibility ButtonsVisible
        {
            get { return (Visibility)GetValue(ButtonsVisibleProperty); }
            set { SetValue(ButtonsVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ReadOnly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonsVisibleProperty =
            DependencyProperty.Register("ButtonsVisible", typeof(Visibility), typeof(GalleryControl));



        public GalleryControl()
        {
            InitializeComponent();
            Images = new ObservableCollection<Photo>();
            this.DataContext = this;
            ImgWidth = 150;  
        }

        public void LoadFromFolder(string path, bool isfromassets=false)
        {
            try
            {
                var imgs = Photo.LoadListFromFolder(path, isfromassets);
                Images.Clear();
                imgs.ForEach(img => Images.Add(img));
            }
            catch { }
        }

        public void SaveToFolder(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            foreach (var image in Images)
            {
                if (image.ToPath==null)
                    image.ToPath = new Uri($"{path}\\{Guid.NewGuid()}.jpg", UriKind.Absolute);
                image.Save();
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var imgs = Photo.OpenImages();
            if (imgs == null) return;

            try
            {
                imgs.ForEach(img => Images.Add(img));
            }
            catch { }
        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (ImgListBox.SelectedItems?.Count!=0)
            {
                List<ImgFrame> temp = new List<ImgFrame>();
                ImgListBox.SelectedItems.for //????
                foreach (ImgFrame item in temp)
                {
                    Images.Remove(item);
                }
            }*/
            
            while (ImgListBox.SelectedItem != null)
            {
                var image = (Photo)ImgListBox.SelectedItem;
                try
                {
                    if (image.FromPath==image.ToPath)
                        File.Delete(image.FromPath.LocalPath);
                }
                catch { }
                Images.Remove(image);
            }
                
        }

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var photo = ((ListBoxItem)sender).Content as Photo;
            ImageWindow wnd = new ImageWindow("",photo);
            wnd.ShowDialog();
        }
    }

    
}
