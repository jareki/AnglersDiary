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
    class AddTackleCategoryViewModel : BaseViewModel
    {
        NoteContext _db;
        TackleCategory _category;

        public TackleCategory Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged(nameof(_category));
            }
        }

        NoteContext DB
        {
            get => _db;
            set
            {
                _db = value;
                OnPropertyChanged(nameof(_db));
            }
        }

        public event EventHandler RequestClose;

        public ICommand SaveCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        public AddTackleCategoryViewModel()
        {
            DB = new NoteContext();
            Category = new TackleCategory();

            SaveCommand = new RelayCommand(obj => Save());
            ExitCommand = new RelayCommand(obj => CloseWindow());
        }

        private void Save()
        {
            try
            {
                DB.TackleCategories.Add(Category);
                DB.SaveChanges();
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
