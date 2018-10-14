using AnglersDiary.Models;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AnglersDiary.Views
{
    /// <summary>
    /// Interaction logic for ChangeSpecyWindow.xaml
    /// </summary>
    public partial class EditSpecyWindow
    {
        public EditSpecyWindow()
        {
            InitializeComponent();
        }

        private void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
