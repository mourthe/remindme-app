using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.Maps;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Geolocation;

using Reminder = Remind.Me.Database.Reminder;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Remind.Me
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ReminderLaunch : Page
    {
        private Container _container;
        private Geopoint _start;
        private Geopoint _finish;

        public ReminderLaunch()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _container = (Container)e.Parameter;

            Load();
        }

        private async void Load()
        {
            var locator = new Geolocator();
            locator.DesiredAccuracyInMeters = 50;

            var position = await locator.GetGeopositionAsync();

            var goal = new MapIcon();
            goal.Location = _container.Point;
            goal.Title = _container.R.Title;

            var you = new MapIcon();
            you.Location = new Geopoint(new BasicGeoposition() 
                { 
                    Latitude = position.Coordinate.Latitude, 
                    Longitude = position.Coordinate.Longitude
                });
            you.Title = "Você";

            SettingsMap.MapElements.Add(goal);
            SettingsMap.MapElements.Add(you);

            _start = you.Location;
            _finish = goal.Location;

            await SettingsMap.TrySetViewAsync(position.Coordinate.Point, 16D);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), _container.R);
        }

        private void Aceitar_Click(object sender, RoutedEventArgs e)
        {
            RequestDirections();           
        }

        private async void RequestDirections()
        {
            var result = await MapRouteFinder.GetWalkingRouteAsync(_start, _finish);

            Frame.Navigate(typeof(MapResult), result);

            //var uri = new Uri("bingmaps:?destination.latitude=" + _container.Point.Position.Latitude.ToString() +
            //   "&destination.longitude=" + _container.Point.Position.Longitude.ToString() + "&destination.name=RJ");
            //var success = await Windows.System.Launcher.LaunchUriAsync(uri);
        }
    }
}
