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
    class AddSpecyViewModel : BaseViewModel
    {
        NoteContext _db;
        Specy _specy;

        public Specy Specy
        {
            get => _specy;
            set
            {
                _specy = value;
                OnPropertyChanged(nameof(_specy));
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

        public event EventHandler RequestClose;

        public ICommand SaveCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        public AddSpecyViewModel()
        {
            DB = new NoteContext();
            Specy = new Specy();

            SaveCommand = new RelayCommand(obj => Save());
            ExitCommand = new RelayCommand(obj => CloseWindow());
        }

        private void Save()
        {
            try
            {
                DB.Species.Add(Specy);
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
