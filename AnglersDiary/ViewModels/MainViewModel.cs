using AnglersDiary.CS;
using AnglersDiary.Models;
using AnglersDiary.Views;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AnglersDiary.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        RelayCommand showtacklescommand, showtrophiescommand, showlocationscommand, deleteallcommand;
        AsyncCommand excelstatcommand, exportcommand, importcommand;
        
        public MainViewModel()
        {
        }

        public string ExcelFileName { get; set; }

        
        public RelayCommand ShowTacklesCommand
        {
            get
            {
                return showtacklescommand ??
                    (showtacklescommand = new RelayCommand(obj => 
                    {
                        ViewRequest.ShowTackles();
                    }));
            }
        }

        public RelayCommand ShowTrophiesCommand
        {
            get
            {
                return showtrophiescommand ??
                    (showtrophiescommand = new RelayCommand(obj =>
                    {
                        ViewRequest.ShowTrophies();                        
                    }));
            }
        }

        public RelayCommand ShowLocationsCommand
        {
            get
            {
                return showlocationscommand ??
                    (showlocationscommand = new RelayCommand(obj => //Note obj
                    {
                        ViewRequest.ShowLocations();
                    }));
            }
        }

        public AsyncCommand ExportCommand
        {
            get
            {
                return exportcommand ??
                    (exportcommand = new AsyncCommand(async obj =>
                    {
                        var savedlg = new SaveFileDialog();
                        savedlg.Filter = "ZIP-archives (*.zip)|*.zip";
                        savedlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        if (savedlg.ShowDialog() == true)
                        {
                            try
                            {
                                await Task.Run(()=>App.Export(savedlg.FileName));
                                MessageBox.Show("Экспорт завершен удачно");
                            }
                            catch { MessageBox.Show("Не удалось экспортировать базу"); }
                        }
                    }));
            }
        }

        public AsyncCommand ImportCommand
        {
            get
            {
                return importcommand ??
                    (importcommand = new AsyncCommand(async obj =>
                    {
                        var opendlg = new OpenFileDialog();
                        opendlg.Filter = "ZIP-archives (*.zip)|*.zip";
                        opendlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        if (opendlg.ShowDialog() == true)
                        {
                            try
                            {
                                await Task.Run(()=>App.Import(opendlg.FileName));
                                MessageBox.Show("Импорт завершен удачно");
                            }
                            catch (Exception exc) { MessageBox.Show($"Не удалось импортировать базу - {exc.Message}"); }
                        }
                    }));
            }
        }

        public RelayCommand DeleteAllCommand
        {
            get
            {
                return deleteallcommand ??
                    (deleteallcommand = new RelayCommand(obj =>
                    {
                        var result = MessageBox.Show("ВЫ действительно хотите очистить базу и удалить все записи безвозвратно?",
                        "Удаление базы", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning, MessageBoxResult.No);
                        if (result == MessageBoxResult.Yes)
                        {
                            try
                            {
                               App.DelAllData();
                            }
                            catch { }
                        }
                    }));
            }
        }

        public AsyncCommand ExcelStatCommand
        {
            get
            {
                return excelstatcommand ??
                    (excelstatcommand = new AsyncCommand(async obj =>
                    {
                        if (obj == null || !(obj is List<Note>)) return;
                        var notes = obj as List<Note>;
                        if (string.IsNullOrEmpty(ExcelFileName))
                            ExcelFileName = "custom";
                        using (ExcelBook book = new ExcelBook())
                        {
                            await Task.Run(() => book.CalculateStat(notes, ExcelFileName));

                            var dlg = new SaveFileDialog();                            
                            dlg.Filter = "XLSX-files (*.xlsx)|*.xlsx";
                            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                            dlg.FileName = ExcelFileName;

                            if (dlg.ShowDialog() == true)
                            {
                                try
                                {
                                    book.Save(dlg.FileName);
                                    MessageBox.Show("Файл успешно сохранен");
                                }
                                catch { MessageBox.Show("Не удалось сохранить файл"); }
                            }
                            else
                                book.Close();
                        }
                    }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
