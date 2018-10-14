using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnglersDiary.Models
{
    public class Catch : INotifyPropertyChanged
    {
        int count;
        string param;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; set; }
        public int? Specy_id { get; set; }
        public virtual Specy Specy { get; set; }
        public int? Note_id { get; set; }
        public virtual Note Note { get; set; }

        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                count = value;
                OnPropertyChanged("Count");
            }
        }

        public string Param
        {
            get
            {
                return param;
            }
            set
            {
                param = value;
                OnPropertyChanged("Param");
            }
        }

        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

