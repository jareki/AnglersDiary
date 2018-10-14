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
    public class AddCatchViewModel : BaseViewModel
    {
        NoteContext _db;
        Catch _catch;
        ObservableCollection<Specy> _species;
        Specy _selectedspecy;

        public Catch Catch
        {
            get => _catch;
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
        
        NoteContext DB
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

        public AddCatchViewModel()
        {
            DB = new NoteContext();
            Catch = new Catch();
            Species = new ObservableCollection<Specy>(DB.Species);
            SelectedSpecy = null;

            SaveCommand = new RelayCommand(obj => Save());
            ExitCommand = new RelayCommand(obj => CloseWindow());
        }       

        public AddCatchViewModel(Note note)
        {
            DB = new NoteContext();
            Catch = new Catch() { Note_id = note.Id };
            Species = new ObservableCollection<Specy>(DB.Species);
            SelectedSpecy = null;

            SaveCommand = new RelayCommand(obj => Save());
            ExitCommand = new RelayCommand(obj => CloseWindow());
        }

        private void Save()
        {
            try
            {
                Catch.Specy_id = SelectedSpecy.Id;
                DB.Catches.Add(Catch);
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

