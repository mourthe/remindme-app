using Remind.Me.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Geolocation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Remind.Me
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        // settings
        private SettingsFile _settings = null;

        public SettingsPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;              
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);

            // not loaded, try to load
            if (_settings == null)
            {
                await LoadSettings();
            }
            else
            {
                await SetupMap();    
            }
        }

        private async System.Threading.Tasks.Task LoadSettings()
        {
            var fileContent = Database.Main.GetSettings().Result;

            // if there is a saved setting load it 
            if (fileContent != null)
            {
                var stream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent)); 
                var ser = new DataContractJsonSerializer(typeof(SettingsFile));

                _settings = (SettingsFile) ser.ReadObject(stream);
                await SetupMap();          
            }
            else // get the postion from the GPS
            {
                var locator = new Geolocator();
                locator.DesiredAccuracyInMeters = 50;

                var position = await locator.GetGeopositionAsync();

                await SettingsMap.TrySetViewAsync(position.Coordinate.Point, 16D);
            }

            mapZoomSlider.Value = SettingsMap.ZoomLevel;
        }

        private async System.Threading.Tasks.Task SetupMap()
        {
            var position = new Windows.Devices.Geolocation.BasicGeoposition();
            position.Latitude = _settings.Latitude;
            position.Longitude = _settings.Longitude;

            var point = new Windows.Devices.Geolocation.Geopoint(position);

            await SettingsMap.TrySetViewAsync(point, 16D);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _settings = new SettingsFile(RemoveTheUnit(((ComboBoxItem)Distance.SelectedItem).Content.ToString()), 
                                        SettingsMap.Center.Position.Latitude, 
                                        SettingsMap.Center.Position.Longitude);

            var stream = new MemoryStream(); 
            var ser = new DataContractJsonSerializer(typeof(SettingsFile));
            
            ser.WriteObject(stream, _settings);

            stream.Position = 0;
            var serialized = new StreamReader(stream).ReadToEnd();

            Database.Main.SaveSettings(serialized);
            TaskBackground.GeofencesHelper.SetSettings(serialized);

            Frame.Navigate(typeof(MainPage));
        }

        private double RemoveTheUnit(string v)
        {
            var idx = v.IndexOf(' ');

            return Convert.ToDouble(v.Substring(0, idx));
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void mapZoom_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (SettingsMap != null)
                SettingsMap.ZoomLevel = e.NewValue;
        }
    }
}
