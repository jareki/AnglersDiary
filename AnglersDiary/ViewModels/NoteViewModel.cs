using AnglersDiary.Models;
using AnglersDiary.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AnglersDiary.ViewModels
{

    public class NoteViewModel : INotifyPropertyChanged
    {
        NoteContext db;
        RelayCommand addcommand, editcommand, deletecommand, shownotecommand;
        IEnumerable<Note> notes;

        public IEnumerable<Note> Notes
        {
            get { return notes; }
            set
            {
                notes = value;
                OnPropertyChanged("Notes");
            }
        }

        Note selectedNote;
        public Note SelectedNote
        {
            get { return selectedNote; }
            set
            {
                selectedNote = value;
                OnPropertyChanged("SelectedNote");
            }
        }

        public NoteViewModel()
        {
            Refresh();
        }

        public void SQL(string query)
        {
            using (db = new NoteContext())
            {
                var ids = db.Database.SqlQuery<int>(query).ToList();
                Notes = db.Notes.Where(n => ids.Contains(n.Id)).Include(n=>n.Location).OrderBy(n=>n.Date).ToList();
            }
        }

        public void SelectByYear(int year)
        {
            using (db = new NoteContext())
                Notes = db.Notes.Where(n => n.Date.Year == year).OrderBy(n => n.Date).Include(n => n.Location).ToList();
        }

        public void SelectByMonth(int month)
        {
            using (db = new NoteContext())
                Notes = db.Notes.Where(n => n.Date.Month == month).OrderBy(n => n.Date).Include(n => n.Location).ToList();
        }

        public void SelectByDate(DateTime date)
        {
            using (db = new NoteContext())
                Notes = db.Notes.Where(n => n.Date == date).Include(n => n.Location).OrderBy(n => n.StartTime).ToList();
        }

        public void Refresh()
        {
            using (db = new NoteContext())
            {
                db.Notes.Include(n => n.Location).Load();
                Notes = db.Notes.Local.ToBindingList();
            }
            SelectedNote = Notes.FirstOrDefault();
        }

        public RelayCommand AddCommand
        {
            get
            {
                return addcommand ??
                    (addcommand = new RelayCommand(obj => //DateTime obj
                    {
                        if (obj == null) return;
                        Note note = new Note();
                        note.SetDefault();
                        note.Date = (DateTime)obj;
                        using (db = new NoteContext())
                        {
                            db.Notes.Add(note);
                            db.SaveChanges();
                        }

                        var lastnote = GetLastNote();
                        EditNoteWindow wnd = new EditNoteWindow();

                        bool delflag = false;
                        using (db = new NoteContext())
                        {
                            note = db.Notes.Where(n => n.Id == lastnote.Id).FirstOrDefault();
                            var vm = new EditNoteViewModel(note, db);
                            vm.RequestClose += (s, e) => wnd.Close();
                            wnd.DataContext = vm;
                            if (wnd.ShowDialog() != true)
                                delflag = true;
                        }
                        if (delflag)
                            DeleteCommand.Execute(note);

                    }));
            }
        }

        public RelayCommand EditCommand
        {
            get
            {
                return editcommand ??
                    (editcommand = new RelayCommand(obj => //Note obj
                    {
                        if (obj == null) return;

                        Note temp = obj as Note;
                        EditNoteWindow wnd = new EditNoteWindow();
                        using (db = new NoteContext())
                        {
                            temp = db.Notes.Where(n => n.Id == temp.Id).FirstOrDefault();
                            var vm = new EditNoteViewModel(temp, db);
                            vm.RequestClose += (s, e) => wnd.Close();
                            wnd.DataContext = vm;
                            wnd.ShowDialog();
                        }
                        SelectByDate(temp.Date);
                    }));
            }
        }

        public RelayCommand DeleteCommand
        {
            get
            {
                return deletecommand ??
                    (deletecommand = new RelayCommand(obj => //Note obj
                    {
                        if (obj == null || !(obj is Note)) return;
                        var result = MessageBox.Show("Вы действительно хотите удалить запись о рыбалке безвозвратно?",
                                    "Удаление рыбалки", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning, MessageBoxResult.No);
                        if (result == MessageBoxResult.Yes)
                        {
                            using (db = new NoteContext())
                            {
                                Note note = db.Notes.Find((obj as Note).Id);
                                db.Notes.Remove(note);
                                db.Trophies.RemoveRange(db.Trophies.Where(t => t.Note_id == note.Id));
                                db.Catches.RemoveRange(db.Catches.Where(t => t.Note_id == note.Id));
                                db.NoteTackles.RemoveRange(db.NoteTackles.Where(t => t.Note_id == note.Id));
                                db.SaveChanges();

                                string path = $"{App.FolderName}\\Assets\\notes\\{note.Id}";
                                App.DeleteFolder(path);
                            }
                            SelectByDate((obj as Note).Date);
                        }
                    }));
            }
        }

        public RelayCommand ShowNoteCommand
        {
            get
            {
                return shownotecommand ??
                    (shownotecommand = new RelayCommand(obj =>
                      {
                          NoteDoc wnd = new NoteDoc(SelectedNote);
                          wnd.ShowDialog();
                      }));
            }
        }

        public List<DateTime> GetDates()
        {
            //string param = $"{date.Year}-{date.ToString("MM")}%";
            using (db = new NoteContext())
                return db.Notes.Select(n=>n.Date).ToList();
        }

        public Note GetNote(int noteId)
        {
            using (var context = new NoteContext())
            {
                context.Notes.Where(n => n.Id == noteId)
                    .Include(n => n.Location)
                    .Load();
                var note = context.Notes.Local.ToBindingList().FirstOrDefault();
                return note;
            }
        }

        public Note GetLastNote()
        {
            int maxid = GetMaxId();
            using (db = new NoteContext())
                return db.Notes.Where(p => p.Id == maxid).First();
        }

        public int GetMaxId()
        {
            using (db = new NoteContext())
            {
                var id = db.Notes.Max(n=>n.Id);
                try
                {
                    return id;
                }
                catch
                {
                    return 0;
                }
            }
        }

        public void DelAll()
        {
            using (db = new NoteContext())
                db.Database.ExecuteSqlCommand("DELETE FROM Notes");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
