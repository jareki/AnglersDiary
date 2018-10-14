using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnglersDiary.Models
{
    public class Note : INotifyPropertyChanged
    {
        CultureInfo provider = CultureInfo.InvariantCulture;
        string cloud, winddir,  moon, weathernote, watertransparency,
            waterheight, flow, waste, bottom, waternote, notestring;
        int windspeed, tempmin, tempmax, tempwater, rainfall, pressure, distance, catchcount;
        double depth, catchweight;
        DateTime date, starttime, endtime;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; set; }

        public DateTime Date  
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }
        
        

        public DateTime StartTime
        {
            get
            {
                return starttime;
            }
            set
            {
                starttime = value;
                OnPropertyChanged("StartTime");
            }
        }
    

        public DateTime EndTime
        {
            get
            {
                return endtime;
            }
            set
            {
                endtime = value;
                OnPropertyChanged("EndTime");
            }
        }
    

        public int? Location_id { get; set; }
        public virtual Location Location { get; set; }

        public int TempMin
        {
            get
            {
                return tempmin;
            }
            set
            {
                tempmin = value;
                OnPropertyChanged("TempMin");
            }
        }

        public int TempMax
        {
            get
            {
                return tempmax;
            }
            set
            {
                tempmax = value;
                OnPropertyChanged("TempMax");
            }
        }

        public int TempWater
        {
            get
            {
                return tempwater;
            }
            set
            {
                tempwater = value;
                OnPropertyChanged("TempWater");
            }
        }

        public int Pressure
        {
            get
            {
                return pressure;
            }
            set
            {
                pressure = value;
                OnPropertyChanged("Pressure");
            }
        }

        public string Cloud
        {
            get
            {
                return cloud;
            }
            set
            {
                cloud = value;
                OnPropertyChanged("Cloud");
            }
        }

        public bool Rainfall
        {
            get
            {
                return rainfall == 1 ? true : false;
            }
            set
            {
                if (value == true)
                    rainfall = 1;
                else
                    rainfall = 0;
            }
        }

        public string WindDir
        {
            get
            {
                return winddir;
            }
            set
            {
                winddir = value;
                OnPropertyChanged("WindDir");
            }
        }

        public int WindSpeed
        {
            get
            {
                return windspeed;
            }
            set
            {
                windspeed = value;
                OnPropertyChanged("WindSpeed");
            }
        }

        public string Moon
        {
            get
            {
                return moon;
            }
            set
            {
                moon = value;
                OnPropertyChanged("Moon");
            }
        }

        public string WeatherNote
        {
            get
            {
                return weathernote;
            }
            set
            {
                weathernote = value;
                OnPropertyChanged("WeatherNote");
            }
        }

        public string WaterTransparency
        {
            get
            {
                return watertransparency;
            }
            set
            {
                watertransparency = value;
                OnPropertyChanged("WaterTransparency");
            }
        }

        public string WaterHeight
        {
            get
            {
                return waterheight;
            }
            set
            {
                waterheight = value;
                OnPropertyChanged("WaterHeight");
            }
        }

        public string Flow
        {
            get
            {
                return flow;
            }
            set
            {
                flow = value;
                OnPropertyChanged("Flow");
            }
        }

        public string Waste
        {
            get
            {
                return waste;
            }
            set
            {
                waste = value;
                OnPropertyChanged("Waste");
            }
        }

        public double Depth
        {
            get
            {
                return depth;
            }
            set
            {
                depth = value;
                OnPropertyChanged("Depth");
            }
        }

        public int Distance
        {
            get
            {
                return distance;
            }
            set
            {
                distance = value;
                OnPropertyChanged("Distance");
            }
        }

        public string Bottom
        {
            get
            {
                return bottom;
            }
            set
            {
                bottom = value;
                OnPropertyChanged("Bottom");
            }
        }

        public string WaterNote
        {
            get
            {
                return waternote;
            }
            set
            {
                waternote = value;
                OnPropertyChanged("WaterNote");
            }
        }

        public double CatchWeight
        {
            get
            {
                return catchweight;
            }
            set
            {
                catchweight = value;
                OnPropertyChanged("CatchWeight");
            }
        }

        public int CatchCount
        {
            get
            {
                return catchcount;
            }
            set
            {
                catchcount = value;
                OnPropertyChanged("CatchCount");
            }
        }
                
        public string NoteString
        {
            get
            {
                return notestring;
            }
            set
            {
                notestring = value;
                OnPropertyChanged("NoteString");
            }
        }

        [NotMapped]
        public string Photos
        {
            get
            {
                return $"{App.FolderName}\\Assets\\notes\\{Id}\\photos";
            }
        }

        public virtual ICollection<NoteTackle> NoteTackles { get; set; }
        public virtual ICollection<Trophy> Trophies { get; set; }
        public virtual ICollection<Catch> Catches { get; set; }

        public Note()
        {
            NoteTackles = new List<NoteTackle>();
            Trophies = new List<Trophy>();
            Catches = new List<Catch>();
        }

        public Note(Note obj) : base()
        {
            Id = obj.Id;

            Set(obj);
            /*
            NoteTackles = new List<NoteTackle>();
            obj.NoteTackles?.ToList().ForEach(p => NoteTackles.Add(p));
            Trophies = new List<Trophy>();
            obj.Trophies?.ToList().ForEach(p => Trophies.Add(p));
            */
        }

        public void Set(Note obj)
        {
            Bottom = obj.Bottom;
            CatchCount = obj.CatchCount;
            CatchWeight = obj.CatchWeight;
            Cloud = obj.Cloud;
            Date = obj.Date;
            Depth = obj.Depth;
            Distance = obj.Distance;
            EndTime = obj.EndTime;
            Flow = obj.Flow;
            Location = obj.Location;
            Location_id = obj.Location_id;
            Moon = obj.Moon;
            NoteString = obj.NoteString;
            Pressure = obj.Pressure;
            Rainfall = obj.Rainfall;
            StartTime = obj.StartTime;
            TempMax = obj.TempMax;
            TempMin = obj.TempMin;
            TempWater = obj.TempWater;
            Waste = obj.Waste;
            WaterHeight = obj.WaterHeight;
            WaterNote = obj.WaterNote;
            WaterTransparency = obj.WaterTransparency;
            WeatherNote = obj.WeatherNote;
            WindDir = obj.WindDir;
            WindSpeed = obj.WindSpeed;
        }

        public void SetDefault()
        {
            Bottom = "Песок";
            Cloud = "Ясно";
            Flow = "Нет";
            Moon = "Полнолуние";
            NoteString = "";
            Waste = "Нет";
            WaterHeight = "Средняя";
            WaterNote = "";
            WaterTransparency = "Средняя";
            WeatherNote = "";
            WindDir = "С";
        }

        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public override string ToString()
        {
            return $"{Date.ToString("dd.MM.yyyy")} {StartTime.ToString("HH:mm")}-{EndTime.ToString("HH:mm")}  {Location?.Name}";
        }
    }
}
