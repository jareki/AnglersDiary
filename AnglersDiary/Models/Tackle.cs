using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnglersDiary.Models
{
    public class Tackle : INotifyPropertyChanged
    {
        string name;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; set; }
        public int? TackleCategory_id { get; set; }  
        public virtual TackleCategory TackleCategory { get; set; }      

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        [NotMapped]
        public string Image
        {
            get
            {
                return $"{App.FolderName}\\Assets\\tackle\\{Id}.jpg";
            }
        }

        public virtual ICollection<NoteTackle> NoteTackles { get; set; }

        public Tackle()
        {
            NoteTackles = new List<NoteTackle>();
        }

        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public string ToCSVString() => $"{TackleCategory.Name};{Name};";
    }
}
