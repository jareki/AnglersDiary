using AnglersDiary.Models;
using AnglersDiary.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AnglersDiary.ViewModels
{
    class ShowAllCatchesViewModel: BaseViewModel
    {
        NoteContext _db;
        ObservableCollection<Catch> _catches;
        Note note;

        public ObservableCollection<Catch> Catches
        {
            get => _catches;
            set
            {
                _catches = value;
                OnPropertyChanged("_catches");
            }
        }
        
        public NoteContext DB
        {
            get => _db;
            set
            {
                _db = value;
                OnPropertyChanged("_db");
            }
        }

        public event EventHandler RequestClose;

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        public ShowAllCatchesViewModel()
        {
            DB = new NoteContext();

            Catches = new ObservableCollection<Catch>(DB.Catches.Include(p => p.Note).Include(p => p.Specy));
            CreateCommands();            
        }

        public ShowAllCatchesViewModel(Note note)
        {
            DB = new NoteContext();
            this.note = note;

            Catches = new ObservableCollection<Catch>(DB.Catches.Where(p => p.Note_id == note.Id)
                                                                .Include(p => p.Note)
                                                                .Include(p => p.Specy));
            CreateCommands();
        }

        public ShowAllCatchesViewModel(List<int> note_ids)
        {
            DB = new NoteContext();
            note = null;

            Catches = new ObservableCollection<Catch>(DB.Catches.Where(c => note_ids.Contains(c.Note_id ?? 0)).Include(c => c.Note).Include(c => c.Specy));
            CreateCommands();
        }

        void CreateCommands()
        {
            AddCommand = new RelayCommand(obj => Add());
            EditCommand = new RelayCommand(obj => Edit(obj));
            DeleteCommand = new RelayCommand(obj => Delete(obj));
            ExitCommand = new RelayCommand(obj => CloseWindow());
        }

        void Refresh()
        {
            Catches.Clear();
            var collection = DB.Catches.Where(p => p.Note_id == note.Id)
                                        .Include(p => p.Note)
                                        .Include(p => p.Specy);
            foreach (var item in collection)
                Catches.Add(item);
        }
        
        void Add()
        {
            AddCatchViewModel vm = new AddCatchViewModel(DB.Notes.Single(n => n.Id == note.Id));
            ViewRequest.AddCatch(vm);
            Refresh();
        }

        void Delete(object selecteditems)
        {
            if (selecteditems == null || !(selecteditems is IList<object>)) return;
            List<Catch> catches = (selecteditems as IList<object>).Cast<Catch>().ToList();
            foreach (var catchy in catches)
            {
                var c = DB.Catches.Find(catchy.Id);
                DB.Catches.Remove(c);                                
            }
            DB.SaveChanges();
            Refresh();
        }

        void Edit(object selected)
        {
            if (selected == null || !(selected is Catch)) return;
            Catch catchy = (Catch)selected;

            EditCatchViewModel vm = new EditCatchViewModel(catchy, DB);
            ViewRequest.EditCatch(vm);
            Refresh();
        }

        public int GetCount(Predicate<Catch> predicate=null)
        {
            if (predicate == null)
                predicate = c => c!=null;
            return (from c in Catches where predicate(c) select c.Count).Sum();
        }

        public double GetTotalLength(Predicate<Catch> predicate = null)
        {
            if (predicate == null)
                predicate = c => c != null;
            double totallength = 0;
            double avg = 0;
            var parameters = from c in Catches
                             where predicate(c)
                             select new { c.Param, c.Count };
            foreach (var c in parameters)
            {
                if (string.IsNullOrEmpty(c.Param)) continue;
                var matches = Regex.Matches(c.Param, "[0-9]+");
                if (matches.Count == 0) continue;

                avg = (from m in matches.Cast<string>()
                      select int.Parse(m)).Average();
                totallength += avg * c.Count;
            }
            return totallength;
        }
        

        public int GetMaxId()
        {
            var id = DB.Database.SqlQuery<int>("SELECT MAX(Id) FROM Catches");
            try
            {
                return id.First();
            }
            catch
            {
                return 0;
            }
        }

        public void DelAll() => DB.Database.ExecuteSqlCommand("DELETE FROM Catches");

        void CloseWindow()
        {
            RequestClose(this, new EventArgs());
        }
    }
}

