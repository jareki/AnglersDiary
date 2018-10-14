using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace AnglersDiary
{
    public class Photo
    {
        //public Uri ImagePath { get; private set; }
        //public Uri SourcePath { get; private set; }
        public Uri FromPath { get; private set; }
        public Uri ToPath { get; set; }
        public BitmapImage Image => GetBitmap();
        public BitmapImage Thumbnail => LoadThumbnail();

        public Photo (string imagepath, bool isfromassets=false)
        {
            this.FromPath = new Uri(imagepath, UriKind.Absolute);
            if (isfromassets)
                ToPath = FromPath;
        }

        public Photo() { }

        public bool FindImg()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "JPEG Images (*.jpg)|*.jpg";
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            dlg.Multiselect = false;
            if (dlg.ShowDialog() == true)
            {
                FromPath = new Uri(dlg.FileName);
                return true;
            }
            else
                return false;
        }

        public static List<Photo> OpenImages()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "JPEG Images (*.jpg)|*.jpg";
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            dlg.Multiselect = true;
            if (dlg.ShowDialog() == true)
            {
                List<Photo> photos = new List<Photo>();
                foreach (string filename in dlg.FileNames)
                    photos.Add(new Photo(filename));
                return photos;
            }
            else
                return null;
        }

        BitmapImage GetBitmap()
        {
            BitmapImage image = new BitmapImage();

            try
            {
                image.BeginInit();
                image.UriSource = FromPath;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                image.EndInit();
            }
            catch
            { return null; }

            return image;
        }

        public static List<Photo> LoadListFromFolder(string directory, bool isfromassets=false)
        {
            var photos = new List<Photo>();
            if (Directory.Exists(directory))
            {
                var files = Directory.GetFiles(directory);
                if (files != null)
                    files.ToList().ForEach(filename => photos.Add(new Photo(filename, isfromassets)));
            }
            return photos;
        }

        BitmapImage LoadThumbnail()
        {             
            BitmapImage image = new BitmapImage();
            try
            {
                image.BeginInit();
                image.UriSource = FromPath;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                image.DecodePixelWidth = 100;
                image.EndInit();
            }
            catch { return null; }
            return image;
        }

        public void Save()
        {
            if (ToPath == null || FromPath==null || ToPath==FromPath)
                return;

            BitmapImage image = new BitmapImage(FromPath);
            Directory.CreateDirectory(Path.GetDirectoryName(ToPath.LocalPath));

            var encoder = new JpegBitmapEncoder();
            encoder.QualityLevel = 70;
            encoder.Frames.Add(BitmapFrame.Create(image));
            using (var fstream = new FileStream(ToPath.LocalPath, FileMode.Create))
            {
                encoder.Save(fstream);
            }
        }
    }
}
