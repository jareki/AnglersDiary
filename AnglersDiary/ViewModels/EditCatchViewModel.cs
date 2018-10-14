using AnglersDiary.Models;
using AnglersDiary.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AnglersDiary.ViewModels
{
    public class EditCatchViewModel : BaseViewModel
    {
        NoteContext _db;
        Catch _catch;
        ObservableCollection<Specy> _species;
        Specy _selectedspecy;

        public Catch Catch
        {
            get { return _catch; }
            set
            {
                _catch = value;
                OnPropertyChanged("_catch");
            }
        }

        public ObservableCollection<Specy> Species
        {
            get => _species;
            set
            {
                _species = value;
                OnPropertyChanged("_species");
            }
        }

        public Specy SelectedSpecy
        {
            get => _selectedspecy;
            set
            {
                _selectedspecy = value;
                OnPropertyChanged("_selectedspecy");
            }
        }

        public NoteContext DB
        {
            get => _db;
            set
            {
                _db = value;
                OnPropertyChanged("_db");
            }
        }

        public event EventHandler RequestClose;

        public ICommand SaveCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        public EditCatchViewModel()
        {
            DB = new NoteContext();
            Catch = new Catch();
            Species = new ObservableCollection<Specy>(DB.Species);
            SelectedSpecy = null;

            SaveCommand = new RelayCommand(obj => Save());
            ExitCommand = new RelayCommand(obj => CloseWindow());
        }

        public EditCatchViewModel(Catch selectedcatch, NoteContext db)
        {
            DB = db;
            Catch = selectedcatch;
            Species = new ObservableCollection<Specy>(DB.Species);
            SelectedSpecy = db.Species.Where(s=>s.Id==Catch.Specy_id).FirstOrDefault();

            SaveCommand = new RelayCommand(obj => Save());
            ExitCommand = new RelayCommand(obj => CloseWindow());
        }

        private void Save()
        {
            try
            {
                Catch.Specy_id = SelectedSpecy.Id;
                DB.SaveChanges();
                CloseWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CloseWindow()
        {
            RequestClose(this, new EventArgs());
        }
    }
}

