using AnglersDiary.Models;
using AnglersDiary.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AnglersDiary.ViewModels
{
    public class AddTrophyViewModel : BaseViewModel
    {
        NoteContext _db;
        Trophy _trophy;
        ObservableCollection<Specy> _species;
        Specy _selectedspecy;
        string _imagesource;

        public Trophy Trophy
        {
            get => _trophy;
            set
            {
                _trophy = value;
                OnPropertyChanged("_trophy");
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

        public string ImageSource
        {
            get => _imagesource;
            set
            {
                _imagesource = value;
                OnPropertyChanged("_imagesource");
            }
        }

        public event EventHandler RequestClose;

        public ICommand SaveCommand { get; set; }
        public ICommand ExitCommand { get; set; }


        public AddTrophyViewModel()
        {
            DB = new NoteContext();
            Trophy = new Trophy();
            Species = new ObservableCollection<Specy>(DB.Species);
            SelectedSpecy = null;

            SaveCommand = new RelayCommand(obj => Save());
            ExitCommand = new RelayCommand(obj => CloseWindow());
        }

        public AddTrophyViewModel(Note note)
        {
            DB = new NoteContext();
            Trophy = new Trophy() { Note_id = note.Id };
            Species = new ObservableCollection<Specy>(DB.Species);
            SelectedSpecy = null;

            SaveCommand = new RelayCommand(obj => Save());
            ExitCommand = new RelayCommand(obj => CloseWindow());
        }

        private void Save()
        {
            try
            {
                Trophy.Specy_id = SelectedSpecy.Id;
                DB.Trophies.Add(Trophy);
                DB.SaveChanges();

                Trophy trophy = DB.Trophies.OrderByDescending(t => t.Id).FirstOrDefault();
                Photo photo = new Photo(ImageSource);
                photo.ToPath = new Uri(trophy.Image);
                photo.Save();

                CloseWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CloseWindow() => RequestClose(this, new EventArgs());

    }
}
