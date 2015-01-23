using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Reminder = Remind.Me.Database.Reminder;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Remind.Me
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ReminderDetails : Page
    {
        private Reminder _reminder;
        private Dictionary<string, int> _cmbb;

        public ReminderDetails()
        {
            this.InitializeComponent();

            this._cmbb = new Dictionary<string, int>();
            this._cmbb.Add("Banco", 0);
            this._cmbb.Add("Farmácia", 1);
            this._cmbb.Add("Supermercado", 2);
            this._cmbb.Add("Casa", 3);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this._reminder = (Reminder)e.Parameter;

            reminderComboBox.SelectedIndex = this._cmbb[this._reminder.Local];
            reminderNameTextBox.Text = this._reminder.Title;
            reminderDetailsTextBox.Text = this._reminder.Details;
        }

        private void EditBarButton_Click(object sender, RoutedEventArgs e)
        {            
            Frame.Navigate(typeof(AddReminderPage), this._reminder); 
        }

        private void BackBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), null); 
        }
    }
}
