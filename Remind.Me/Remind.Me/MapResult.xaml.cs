using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.Maps;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Remind.Me
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapResult : Page
    {
        public MapResult()
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
            var routeResult = (MapRouteFinderResult)e.Parameter;
            
            // Display summary info about the route.
            tbOutputText.Inlines.Add(new LineBreak());
            tbOutputText.Inlines.Add(new Run()
            {
                Text = "Distancia (quilometros) = "
                    + (routeResult.Route.LengthInMeters / 1000).ToString()
            });
            tbOutputText.Inlines.Add(new LineBreak());
            tbOutputText.Inlines.Add(new LineBreak());

            // Display the directions.
            tbOutputText.Inlines.Add(new Run()
            {
                Text = "Direções: "
            });
            tbOutputText.Inlines.Add(new LineBreak());

            foreach (MapRouteLeg leg in routeResult.Route.Legs)
            {
                foreach (MapRouteManeuver maneuver in leg.Maneuvers)
                {
                    tbOutputText.Inlines.Add(new Run()
                    {
                        Text = maneuver.InstructionText
                    });
                    tbOutputText.Inlines.Add(new LineBreak());
                }
            }            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
