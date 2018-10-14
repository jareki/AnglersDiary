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
    public class AddTackleViewModel : BaseViewModel
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
                OnPropertyChanged("_tackle");
            }
        }

        public ObservableCollection<TackleCategory> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged("_categories");
            }
        }

        public TackleCategory SelectedCategory
        {
            get => _selectedcategory;
            set
            {
                _selectedcategory = value;
                OnPropertyChanged("_selectedcategory");
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

        public AddTackleViewModel()
        {
            DB = new NoteContext();
            Tackle = new Tackle();
            Categories = new ObservableCollection<TackleCategory>(DB.TackleCategories);
            SelectedCategory = null;

            SaveCommand = new RelayCommand(obj => Save());
            ExitCommand = new RelayCommand(obj => CloseWindow());
        }

        public AddTackleViewModel(TackleCategory category)
        {
            DB = new NoteContext();
            Tackle = new Tackle() { TackleCategory_id = category.Id };
            Categories = new ObservableCollection<TackleCategory>(DB.TackleCategories);
            SelectedCategory = category;

            SaveCommand = new RelayCommand(obj => Save());
            ExitCommand = new RelayCommand(obj => CloseWindow());
        }

        private void Save()
        {
            try
            {
                Tackle.TackleCategory_id = SelectedCategory.Id;
                DB.Tackles.Add(Tackle);
                DB.SaveChanges();

                Tackle tackle = DB.Tackles.OrderByDescending(t => t.Id).FirstOrDefault();
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
