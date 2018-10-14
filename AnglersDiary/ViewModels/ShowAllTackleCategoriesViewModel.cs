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
    class ShowAllTackleCategoriesViewModel : BaseViewModel
    {
        NoteContext _db;
        ObservableCollection<TackleCategory> _categories;

        public ObservableCollection<TackleCategory> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(_categories));
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

        public ShowAllTackleCategoriesViewModel()
        {
            DB = new NoteContext();
            Categories = new ObservableCollection<TackleCategory>(DB.TackleCategories.OrderBy(c=>c.Name));
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
            Categories.Clear();
            var collection = DB.TackleCategories.OrderBy(c=>c.Name);
            foreach (var item in collection)
                Categories.Add(item);
        }

        void Add()
        {
            AddTackleCategoryViewModel vm = new AddTackleCategoryViewModel();
            ViewRequest.AddTackleCategory(vm);
            Refresh();
        }

        void Edit(object selected)
        {
            if (selected == null || !(selected is TackleCategory)) return;
            TackleCategory category = (TackleCategory)selected;

            EditTackleCategoryViewModel vm = new EditTackleCategoryViewModel(category, DB);
            ViewRequest.EditTackleCategory(vm);
            Refresh();
        }

        void CloseWindow() => RequestClose(this, new EventArgs());
    }
}
