using AnglersDiary.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AnglersDiary
{
    
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string FolderName
        {
            get
            {
                return Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            }
        }

        public static void DeleteFolder(string path)
        {
            if (Directory.Exists(path))
            {
                var di = new DirectoryInfo(path);
                di.GetFiles("*", SearchOption.AllDirectories).ToList().ForEach(f => f.Delete());
                di.GetDirectories("*", SearchOption.AllDirectories).ToList().ForEach(d => d.Delete());
                di.Delete(true);
            }
        }

        public static void Import(string ToFileName)
        {
            Directory.Move($"{App.FolderName}\\Assets", $"{App.FolderName}\\Assets-{DateTime.Now.Ticks}");
            ZipFile.ExtractToDirectory(ToFileName, $"{App.FolderName}\\Assets");
        }

        public static void Export(string FromFileName)
        {
            ZipFile.CreateFromDirectory($"{App.FolderName}\\Assets", FromFileName);
        }

        public static void DelAllData()
        {
            var model1 = new ShowAllCatchesViewModel();            
            var model2 = new LocationViewModel();
            var model3 = new ShowAllNoteTacklesViewModel(new Models.Note());
            var model4 = new ShowAllTacklesViewModel(new Models.TackleCategory());
            var model5 = new ShowAllTrophiesViewModel();
            var model6 = new NoteViewModel();

            model1.DelAll();
            model2.DelAll();
            model3.DelAll();
            model4.DelAll();
            model5.DelAll();
            model6.DelAll();

            DirectoryInfo notes_di = new DirectoryInfo($"{App.FolderName}\\Assets\\notes");
            foreach (var di in notes_di.GetDirectories())
                di.Delete(true);

            DirectoryInfo tackles_di = new DirectoryInfo($"{App.FolderName}\\Assets\\tackle");
            foreach (var file in tackles_di.GetFiles())
                file.Delete();
        }
        
    }
}
