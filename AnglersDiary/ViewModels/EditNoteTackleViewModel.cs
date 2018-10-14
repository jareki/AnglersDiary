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
    class EditNoteTackleViewModel : BaseViewModel
    {
        NoteContext _db;
        NoteTackle _notetackle;

        public NoteTackle NoteTackle
        {
            get => _notetackle;
            set
            {
                _notetackle = value;
                OnPropertyChanged(nameof(_notetackle));
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

        public EditNoteTackleViewModel(NoteTackle selectedtackle, NoteContext db)
        {
            DB = db;
            NoteTackle = selectedtackle;

            SaveCommand = new RelayCommand(obj => Save());
            ExitCommand = new RelayCommand(obj => CloseWindow());
        }

        private void Save()
        {
            try
            {
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
