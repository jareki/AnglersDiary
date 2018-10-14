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
    public class EditTackleViewModel : BaseViewModel
    {
        NoteContext _db;
        Tackle _tackle;
        ObservableCollection<TackleCategory> _categories;
        TackleCategory _selectedcategory;
        string _imagesource;

        public Tackle Tackle
        {
            get => _tackle;
            set
            {
                _tackle = value;
                OnPropertyChanged(nameof(_tackle));
            }
        }

        public ObservableCollection<TackleCategory> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(_categories));
            }
        }

        public TackleCategory SelectedCategory
        {
            get => _selectedcategory;
            set
            {
                _selectedcategory = value;
                OnPropertyChanged(nameof(_selectedcategory));
            }
        }

        public string ImageSource
        {
            get => _imagesource;
            set
            {
                _imagesource = value;
                OnPropertyChanged(nameof(_imagesource));
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

        public EditTackleViewModel()
        {
            DB = new NoteContext();
            Tackle = new Tackle();
            Categories = new ObservableCollection<TackleCategory>(DB.TackleCategories);
            SelectedCategory = null;

            SaveCommand = new RelayCommand(obj => Save());
            ExitCommand = new RelayCommand(obj => CloseWindow());
        }

        public EditTackleViewModel(Tackle selectedtackle, NoteContext db)
        {
            DB = db;
            Tackle = selectedtackle;
            Categories = new ObservableCollection<TackleCategory>(DB.TackleCategories);
            SelectedCategory = db.TackleCategories.Where(c=>c.Id==selectedtackle.TackleCategory_id).FirstOrDefault();

            SaveCommand = new RelayCommand(obj => Save());
            ExitCommand = new RelayCommand(obj => CloseWindow());
        }

        private void Save()
        {
            try
            {
                Tackle.TackleCategory_id = SelectedCategory.Id;
                DB.SaveChanges();

                Photo photo = new Photo(ImageSource);
                photo.ToPath = new Uri(Tackle.Image);
                photo.Save();

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
