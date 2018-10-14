using Microsoft.Maps.MapControl.WPF;
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
using AnglersDiary.Models;
using AnglersDiary.ViewModels;
using MahApps.Metro.Controls;
using System.Configuration;

namespace AnglersDiary.Views
{
    /// <summary>
    /// Interaction logic for LocationWindow.xaml
    /// </summary>
    public partial class LocationWindow
    {
        LocationViewModel model;
        public Pushpin SelectedPin { get; set; }
        public Models.Location SelectLocation
        {
            get { return SelectedPin.Tag as Models.Location; }
        }
        bool isDragging = false;

        public LocationWindow(bool isselectmode = false)
        {
            InitializeComponent();
            model = new LocationViewModel();
            DataContext = model;
            if (isselectmode)
            {
                ButtonsPanel.Visibility = Visibility.Visible;
            }
            else
                ButtonsPanel.Visibility = Visibility.Collapsed;
        }

        private void Update()
        {
            model.Refresh();
            AcceptBtn.IsEnabled = false;
            Map.Children.Clear();

            foreach (var location in model.Locations)
            {
                Pushpin pin = new Pushpin();
                pin.Location = new Microsoft.Maps.MapControl.WPF.Location(location.Latitude, location.Longitude);
                pin.Tag = location;
                pin.Background = new SolidColorBrush(Colors.Red);
                pin.MouseLeftButtonDown += Pin_MouseLeftButtonDown;
                pin.MouseRightButtonDown += Pin_MouseRightButtonDown;
                Map.Children.Add(pin);
            }

        }

        private void Pin_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Pin_MouseLeftButtonDown(sender, e);
            Map.MouseUp += Map_MouseUp;
            Map.MouseMove += Map_MouseMove;
            // Enable Dragging
            this.isDragging = true;

            base.OnMouseRightButtonDown(e);
        }

        private void Pin_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (SelectedPin != null)
                SelectedPin.Background = new SolidColorBrush(Colors.Red);
            SelectedPin = sender as Pushpin;
            SelectedPin.Background = new SolidColorBrush(Colors.Blue);

            model.SelectedLocation = SelectedPin.Tag as Models.Location;
            model.OnPropertyChanged("SelectedLocation");
            AcceptBtn.IsEnabled = true;
        }

        private void Map_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Map.MouseUp -= Map_MouseRightButtonUp;
            Map.MouseMove -= Map_MouseMove;

            this.isDragging = false;
        }

        private void Map_MouseMove(object sender, MouseEventArgs e)
        {
            var map = sender as Microsoft.Maps.MapControl.WPF.Map;
            // Check if the user is currently dragging the Pushpin
            if (this.isDragging)
            {
                // If so, the Move the Pushpin to where the Mouse is.
                var mouseMapPosition = e.GetPosition(map);
                var mouseGeocode = map.ViewportPointToLocation(mouseMapPosition);
                SelectedPin.Location = mouseGeocode;
                model.SelectedLocation.Latitude = SelectedPin.Location.Latitude;
                model.SelectedLocation.Longitude = SelectedPin.Location.Longitude;
            }
        }

        private void Map_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            /*
            Point mouseposition = e.GetPosition(this);
            Microsoft.Maps.MapControl.WPF.Location pinlocation = Map.ViewportPointToLocation(mouseposition);

            Models.Location location = new Models.Location
            {
                Longitude = pinlocation.Longitude,
                Latitude = pinlocation.Latitude,
                Name = "Без Имени"
            };
            (DataContext as LocationViewModel).AddCommand.Execute(location);
            Update();
            /*

            Pushpin pin = new Pushpin();
            pin.Location = pinlocation;
            //pin.Tag
            Map.Children.Add(pin);*/


        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Maps.MapControl.WPF.Location pinlocation = Map.Center;

            Models.Location location = new Models.Location
            {
                Longitude = pinlocation.Longitude,
                Latitude = pinlocation.Latitude,
                Name = "Без Имени"
            };
            (DataContext as LocationViewModel).AddCommand.Execute(location);
            Update();
            SelectedPin = null;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            (DataContext as LocationViewModel).EditCommand.Execute(null);
        }

        private void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Map.CredentialsProvider = new ApplicationIdCredentialsProvider(ConfigurationManager.AppSettings["MapAPIkey"]);
            Update();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
