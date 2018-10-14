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
    class ShowAllTrophiesViewModel : BaseViewModel
    {
        NoteContext _db;
        ObservableCollection<Trophy> _trophies;
        Note note;

        public ObservableCollection<Trophy> Trophies
        {
            get => _trophies;
            set
            {
                _trophies = value;
                OnPropertyChanged("_trophies");
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
        public ICommand ShowImageCommand { get; set; }
        public ICommand ShowNoteCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        public ShowAllTrophiesViewModel()
        {
            DB = new NoteContext();
            Trophies = new ObservableCollection<Trophy>(DB.Trophies.Include(p => p.Note).Include(p => p.Specy));
            CreateCommands();
        }

        public ShowAllTrophiesViewModel(Specy specy)
        {
            DB = new NoteContext();
            Trophies = new ObservableCollection<Trophy>(DB.Trophies.Where(p=>p.Specy_id==specy.Id)
                                                                    .Include(p => p.Note)
                                                                    .Include(p => p.Specy));
            CreateCommands();
        }

        public ShowAllTrophiesViewModel(Note note)
        {
            DB = new NoteContext();
            this.note = note;
            Trophies = new ObservableCollection<Trophy>(DB.Trophies.Where(p=>p.Note_id==note.Id)
                                                                    .Include(p => p.Note)
                                                                    .Include(p => p.Specy));
            CreateCommands();
        }

        public ShowAllTrophiesViewModel(List<int> note_ids)
        {
            DB = new NoteContext();
            Trophies = new ObservableCollection<Trophy>(DB.Trophies.Where(c => note_ids.Contains(c.Note_id ?? 0))
                                                                    .Include(c => c.Note)
                                                                    .Include(c => c.Specy));
            CreateCommands();
        }

        void CreateCommands()
        {
            AddCommand = new RelayCommand(obj => Add());
            EditCommand = new RelayCommand(obj => Edit(obj));
            DeleteCommand = new RelayCommand(obj => Delete(obj));
            ShowImageCommand = new RelayCommand(obj => ShowImage(obj));
            ShowNoteCommand = new RelayCommand(obj => ShowNote(obj));
            ExitCommand = new RelayCommand(obj => CloseWindow());
        }

        void Refresh()
        {
            Trophies.Clear();
            var collection = DB.Trophies.Where(p => p.Note_id == note.Id)
                                        .Include(p => p.Note)
                                        .Include(p => p.Specy);
            foreach (var item in collection)
                Trophies.Add(item);
        }

        void Add()
        {
            AddTrophyViewModel vm = new AddTrophyViewModel(DB.Notes.Single(n => n.Id == note.Id));
            ViewRequest.AddTrophy(vm);
            Refresh();
        }

        void Delete(object selecteditems)
        {
            if (selecteditems == null || !(selecteditems is IList<object>)) return;
            List<Trophy> trophies = (selecteditems as IList<object>).Cast<Trophy>().ToList();
            foreach (var trophy in trophies)
            {
                var c = DB.Trophies.Find(trophy.Id);
                DB.Trophies.Remove(c);
            }
            DB.SaveChanges();
            Refresh();
        }

        void Edit(object selected)
        {
            if (selected == null || !(selected is Trophy)) return;
            Trophy trophy = (Trophy)selected;

            EditTrophyViewModel vm = new EditTrophyViewModel(trophy, DB);
            ViewRequest.EditTrophy(vm);
            Refresh();
        }

        void ShowImage(object selected)
        {
            if (selected == null || !(selected is Trophy)) return;
            Trophy trophy = (Trophy)selected;
            ViewRequest.ShowImage(trophy.Image);
        }

        public void ShowNote(object selected)
        {
            if (selected == null || !(selected is Trophy)) return;
            Trophy trophy = (Trophy)selected;
            ViewRequest.ShowNote(DB.Notes.Where(n=>n.Id==trophy.Note_id).FirstOrDefault());
        }

        public int GetCount()
        {
            if (Trophies != null)
                return Trophies.Count();
            else
                return 0;
        }

        public int GetTotalLength()
        {
            return (from t in Trophies select t.Size).Sum();
        }

        public int GetMaxId()
        {
            var id = DB.Database.SqlQuery<int>("SELECT MAX(Id) FROM Trophies");
            try
            {
                return id.First();
            }
            catch
            {
                return 0;
            }
        }

        public void DelAll() => DB.Database.ExecuteSqlCommand("DELETE FROM Trophies");

        void CloseWindow() => RequestClose(this, new EventArgs());
    }
}
