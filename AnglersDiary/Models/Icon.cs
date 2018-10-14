using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnglersDiary.Models
{
    public class Icon
    {
        public string Text { get; set; }
        public string Image { get; set; }

        public static List<Icon> GetCloudList()
        {
            return new List<Icon>
            {
                new Icon { Text = "Ясно", Image = "/Assets/Icons/Cloud-01.png" },
                new Icon { Text = "Переменная", Image = "/Assets/Icons/Cloud-02.png" },
                new Icon { Text = "Пасмурно", Image = "/Assets/Icons/Cloud-03.png" }
            };
        }

        public static List<Icon> GetMoonList()
        {
            return new List<Icon>
            {
                new Icon { Text = "Полнолуние", Image = "/Assets/Icons/moon-02.png" },
                new Icon { Text = "Новолуние", Image = "/Assets/Icons/moon-01.png" },
                new Icon { Text = "Растущий месяц", Image = "/Assets/Icons/moon-06.png" },
                new Icon { Text = "Стареющий месяц", Image = "/Assets/Icons/moon-05.png" },
                new Icon { Text = "Растущая луна", Image = "/Assets/Icons/moon-04.png" },
                new Icon { Text = "Стареющая луна", Image = "/Assets/Icons/moon-03.png" }
            };
        }
    }
}
