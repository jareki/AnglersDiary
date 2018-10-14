using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace AnglersDiary.Models
{
    public class ImgFrame
    {
        public BitmapImage Image { get; set; }
        public int RotateAngle { get; set; }

        public ImgFrame (BitmapImage image, int angle)
        {
            Image = image;
            RotateAngle = angle;
        }
    }
}
