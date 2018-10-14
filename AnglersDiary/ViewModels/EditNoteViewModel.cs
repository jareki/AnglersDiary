using AnglersDiary.Models;
using AnglersDiary.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AnglersDiary.ViewModels
{
    class EditNoteViewModel : BaseViewModel
    {
        NoteContext _db;
        Note _note;
        IList<Icon> _cloudicons, _moonicons;
        Icon _selectedcloud, _selectedmoon;
        Location _selectedlocation;        

        public Note Note
        {
            get => _note;
            set
            {
                _note = value;
                OnPropertyChanged(nameof(_note));
            }
        }

        public Icon SelectedCloud
        {
            get => _selectedcloud;
            set
            {
                _selectedcloud = value;
                OnPropertyChanged(nameof(_selectedcloud));
            }
        }

        public IList<Icon> CloudIcons
        {
            get => _cloudicons;
            set
            {
                _cloudicons = value;
                OnPropertyChanged(nameof(_cloudicons));
            }
        }

        public Icon SelectedMoon
        {
            get => _selectedmoon;
            set
            {
                _selectedmoon = value;
                OnPropertyChanged(nameof(_selectedmoon));
            }
        }

        public IList<Icon> MoonIcons
        {
            get => _moonicons;
            set
            {
                _moonicons = value;
                OnPropertyChanged(nameof(_moonicons));
            }
        }

        public Location SelectedLocation
        {
            get => _selectedlocation;
            set
            {
                _selectedlocation = value;
                OnPropertyChanged(nameof(_selectedlocation));
            }
        }

        public NoteContext DB
        {
            get => _db;
            set
            {
                _db = value;
                OnPropertyChanged(nameof(_db));
            }
        }

        public ShowAllTrophiesViewModel TrophyVM { get; set; }
        public ShowAllCatchesViewModel CatchVM { get; set; }
        public ShowAllNoteTacklesViewModel NoteTackleVM { get; set; }

        public event EventHandler RequestClose;

        public ICommand SaveCommand { get; set; }
        public ICommand SelectLocationCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        public EditNoteViewModel(Note selectednote, NoteContext db)
        {
            DB = db;
            Note = selectednote;
            MoonIcons = Icon.GetMoonList();
            SelectedMoon = MoonIcons.FirstOrDefault(i => i.Text == Note.Moon);

            CloudIcons = Icon.GetCloudList();
            SelectedCloud = CloudIcons.FirstOrDefault(i => i.Text == Note.Cloud);

            TrophyVM = new ShowAllTrophiesViewModel(Note);
            CatchVM = new ShowAllCatchesViewModel(Note);
            NoteTackleVM = new ShowAllNoteTacklesViewModel(Note);

            SaveCommand = new RelayCommand(obj => Save());
            SelectLocationCommand = new RelayCommand(obj => SelectLocation());
            ExitCommand = new RelayCommand(obj => CloseWindow());
        }

        private void Save()
        {
            try
            {
                Note.Cloud = SelectedCloud?.Text ?? "Ясно";
                Note.Moon = SelectedMoon.Text ?? "Полнолуние";

                DB.SaveChanges();
                CloseWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SelectLocation()
        {
            LocationWindow wnd = new LocationWindow(true);
            if (wnd.ShowDialog() == true)
            {
                Note.Location_id = wnd.SelectLocation.Id;
                SelectedLocation = wnd.SelectLocation;
                OnPropertyChanged(nameof(SelectedLocation));
            }
        }

        private void CloseWindow() => RequestClose(this, new EventArgs());
    }
}
