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
    public class ShowAllTacklesViewModel : BaseViewModel
    {
        NoteContext _db;
        ObservableCollection<Tackle> _tackles;
        TackleCategory category;

        public ObservableCollection<Tackle> Tackles
        {
            get => _tackles;
            set
            {
                _tackles = value;
                OnPropertyChanged(nameof(_tackles));
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
        
        public ShowAllTacklesViewModel(TackleCategory category)
        {
            DB = new NoteContext();
            Tackles = new ObservableCollection<Tackle>(DB.Tackles.Where(t=>t.TackleCategory_id==category.Id)
                                                                .OrderBy(t=>t.Name));
            this.category = category;
            CreateCommands();
        }

        void CreateCommands()
        {
            AddCommand = new RelayCommand(obj => Add());
            EditCommand = new RelayCommand(obj => Edit(obj));
            ExitCommand = new RelayCommand(obj => CloseWindow());
        }

        void Refresh()
        {
            Tackles.Clear();
            var collection = DB.Tackles.Where(p => p.TackleCategory_id == category.Id).OrderBy(t => t.Name);
            foreach (var item in collection)
                Tackles.Add(item);
        }

        void Add()
        {
            AddTackleViewModel vm = new AddTackleViewModel(category);
            ViewRequest.AddTackle(vm);
            Refresh();
        }
                
        void Edit(object selected)
        {
            if (selected == null || !(selected is Tackle)) return;
            Tackle tackle = (Tackle)selected;

            EditTackleViewModel vm = new EditTackleViewModel(tackle, DB);
            ViewRequest.EditTackle(vm);
            Refresh();
        }

        public int GetMaxId()
        {
            var id = DB.Database.SqlQuery<int>("SELECT MAX(Id) FROM Tackles");
            try
            {
                return id.First();
            }
            catch
            {
                return 0;
            }
        }

        public void DelAll() => DB.Database.ExecuteSqlCommand("DELETE FROM Tackles");

        public List<Tackle> SQL(string query)
        {
            return DB.Tackles.SqlQuery(query).ToList();
        }

        void CloseWindow()
        {
            RequestClose(this, new EventArgs());
        }
    }
}
