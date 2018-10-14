using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnglersDiary.Models
{

    public class Trophy : INotifyPropertyChanged
    {
        int size;
        double weight;
        string text;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; set; }        
        public int? Specy_id { get; set; }
        public virtual Specy Specy { get; set; }
        public int? Note_id { get; set; }
        public virtual Note Note { get; set; }

        [NotMapped]
        public string Image
        {
            get
            {
                return $"{App.FolderName}\\Assets\\notes\\{Note_id}\\trophies\\{Id}.jpg";
            }
        }

        public int Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
                OnPropertyChanged("Size");
            }
        }

        public double Weight
        {
            get
            {
                return weight;
            }
            set
            {
                weight = value;
                OnPropertyChanged("Weight");
            }
        }
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                OnPropertyChanged("Text");
            }
        }

        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
