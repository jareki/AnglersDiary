using AnglersDiary.Models;
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
    public class LocationViewModel : BaseViewModel
    {
        NoteContext _db;
        ObservableCollection<Location> _locations;
        Location _selectedlocation;

        public Location SelectedLocation
        {
            get => _selectedlocation;
            set
            {
                _selectedlocation = value;
                OnPropertyChanged(nameof(_selectedlocation));
            }
        }

        public ObservableCollection<Location> Locations
        {
            get => _locations;
            set
            {
                _locations = value;
                OnPropertyChanged(nameof(_locations));
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

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }

        public LocationViewModel()
        {
            DB = new NoteContext();
            Locations = new ObservableCollection<Location>(DB.Locations);
            CreateCommands();
        }

        void CreateCommands()
        {
            AddCommand = new RelayCommand(obj => Add(obj));
            EditCommand = new RelayCommand(obj => Edit(obj));
        }
               
        public void Refresh()
        {
            Locations.Clear();
            var collection = DB.Locations;
            foreach (var item in collection)
                Locations.Add(item);
        }

        void Add(object obj)
        {
            if (obj == null) return;
            Location location = obj as Location;
            DB.Locations.Add(location);
            DB.SaveChanges();
        }

        void Edit (object obj)
        {
            DB.SaveChanges();
        }

        public int GetMaxId()
        {
            var id = DB.Database.SqlQuery<int>("SELECT MAX(Id) FROM Locations");
            try
            {
                return id.First();
            }
            catch
            {
                return 0;
            }
        }

        public void DelAll() => DB.Database.ExecuteSqlCommand("DELETE FROM Locations");

        public Dictionary<string,int> Top(List<Note> notes, int num)
        {
            var locations_ids = (from n in notes
                                 group n by n.Location_id into g
                                 orderby g.Count() descending
                                 select new { id = g.Key, Count = g.Count() }).Take(num);
            var result = new Dictionary<string,int>();
            var location_counts = new List<int>();
            foreach (var item in locations_ids)
            {
                result.Add(Locations.Where(p => p.Id == item.id).First().Name,item.Count);
            }
            return result;
        }

        internal void SelectLocation(Location location)
        {
            SelectedLocation = Locations.FirstOrDefault();
        }
    }
}
