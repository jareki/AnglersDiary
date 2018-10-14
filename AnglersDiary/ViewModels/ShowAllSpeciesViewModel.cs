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
using System.Windows.Input;

namespace AnglersDiary.ViewModels
{
    class ShowAllSpeciesViewModel : BaseViewModel
    {
        NoteContext _db;
        ObservableCollection<Specy> _species;

        public ObservableCollection<Specy> Species
        {
            get => _species;
            set
            {
                _species = value;
                OnPropertyChanged(nameof(_species));
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

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        public ShowAllSpeciesViewModel()
        {
            DB = new NoteContext();
            Species = new ObservableCollection<Specy>(DB.Species.OrderBy(s => s.Name));
            CreateCommands();
        }

        void CreateCommands()
        {
            AddCommand = new RelayCommand(obj => Add());
            EditCommand = new RelayCommand(obj => Edit(obj));
            ExitCommand = new RelayCommand(obj => CloseWindow());
        }

        public void Refresh()
        {
            Species.Clear();
            var collection = DB.Species.OrderBy(s=>s.Name);
            foreach (var item in collection)
                Species.Add(item);
        }

        void Add()
        {
            AddSpecyViewModel vm = new AddSpecyViewModel();
            ViewRequest.AddSpecy(vm);
            Refresh();
        }

        void Edit(object selected)
        {
            if (selected == null || !(selected is Specy)) return;
            Specy specy = (Specy)selected;

            EditSpecyViewModel vm = new EditSpecyViewModel(specy, DB);
            ViewRequest.EditSpecy(vm);
            Refresh();
        }

        void CloseWindow() => RequestClose(this, new EventArgs());
    }
}
