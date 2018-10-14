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
    class ShowAllNoteTacklesViewModel : BaseViewModel
    {
        NoteContext _db;
        Note Note;
        ObservableCollection<NoteTackle> _notetackles;

        public ObservableCollection<NoteTackle> NoteTackles
        {
            get { return _notetackles; }
            set
            {
                _notetackles = value;
                OnPropertyChanged(nameof(_notetackles));
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
        public ICommand DeleteCommand { get; set; }
        public ICommand CopyCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        public ShowAllNoteTacklesViewModel(Note note)
        {
            Note = note;
            DB = new NoteContext();
            NoteTackles = new ObservableCollection<NoteTackle>(DB.NoteTackles.Where(p => p.Note_id==Note.Id)
                                                                            .Include(p => p.Note)
                                                                            .Include(p => p.Tackle)
                                                                            .Include(p=>p.Tackle.TackleCategory));
            CreateCommands();
        }

        void CreateCommands()
        {
            AddCommand = new RelayCommand(obj => Add());
            EditCommand = new RelayCommand(obj => Edit(obj));
            DeleteCommand = new RelayCommand(obj => Delete(obj));
            CopyCommand = new RelayCommand(obj => Copy(obj));
            ExitCommand = new RelayCommand(obj => CloseWindow());
        }

        void Refresh()
        {
            NoteTackles.Clear();
            var collection = DB.NoteTackles.Where(p => p.Note_id == Note.Id)
                                            .Include(p => p.Note)
                                            .Include(p => p.Tackle)
                                            .Include(p => p.Tackle.TackleCategory);
            foreach (var item in collection)
                NoteTackles.Add(item);
        }

        void Add()
        {
            Tackle tackle =ViewRequest.AddNoteTackle();
            if (tackle == null) return;
            NoteTackle notetackle = new NoteTackle
            {
                Note_id = Note.Id,
                Tackle_id = tackle.Id,
                Parameter = ""
            };

            DB.NoteTackles.Add(notetackle);
            DB.SaveChanges();
            Refresh();
        }

        void Edit(object selectednotetackle)
        {
            if (selectednotetackle == null || !(selectednotetackle is NoteTackle)) return;
            NoteTackle notetackle = selectednotetackle as NoteTackle;
            EditNoteTackleViewModel vm = new EditNoteTackleViewModel(notetackle, DB);
            ViewRequest.EditNoteTackle(vm);
            Refresh();
        }

        void Delete(object selectednotetackles)
        {
            if (selectednotetackles == null || !(selectednotetackles is IList<object>)) return;
            var tackles = (selectednotetackles as IList<object>).Cast<NoteTackle>().ToList();
            foreach (var tackle in tackles)
            {
                var t = DB.NoteTackles.Find(tackle.Id);
                DB.NoteTackles.Remove(t);
            }
            DB.SaveChanges();
            Refresh();
        }
        
        void Copy(object selecteddate)
        {
            if (selecteddate == null || !(selecteddate is DateTime)) return;

            DateTime date = (DateTime)selecteddate;
            Note note = DB.Notes.Where(p => p.Date == date).FirstOrDefault();
            if (note == null)
            {
                System.Windows.MessageBox.Show("Не удлаось выполнить копирование комплекта");
                return;
            }
            var model = new ShowAllNoteTacklesViewModel(note);
            if (model.NoteTackles?.Count() > 0)
                foreach (var item in model.NoteTackles)
                {
                    var temp = new NoteTackle()
                    {
                        Note_id = Note.Id,
                        Parameter = item.Parameter,
                        Tackle_id = item.Tackle_id
                    };
                    DB.NoteTackles.Add(temp);
                    DB.SaveChanges();
                }
            Refresh();
        }

        public int GetMaxId()
        {
            var id = DB.Database.SqlQuery<int>("SELECT MAX(Id) FROM NoteTackles");
            try
            {
                return id.First();
            }
            catch
            {
                return 0;
            }
        }

        public void DelAll() => DB.Database.ExecuteSqlCommand("DELETE FROM NoteTackles");

        void CloseWindow() => RequestClose(this, new EventArgs());
    }
}
