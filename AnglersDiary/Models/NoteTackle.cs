using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnglersDiary.Models
{
    public class NoteTackle : INotifyPropertyChanged
    {
        string parameter;
        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; set; }
        public int? Tackle_id { get; set; }
        public virtual Tackle Tackle { get; set; }
        public int? Note_id { get; set; }
        public virtual Note Note { get; set; }

        public string Parameter
        {
            get
            {
                return parameter;
            }
            set
            {
                parameter = value;
                OnPropertyChanged("Parameter");
            }
        }

        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
