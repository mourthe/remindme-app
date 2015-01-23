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
namespace Remind.Me
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddReminderPage : Page
    {
        private bool _edit;
        private Reminder _oldReminder;
        private Dictionary<string, int> _cmbb;

        public AddReminderPage()
        {
            this.InitializeComponent();

            this._edit = false;
            this._oldReminder = null;

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
            if (e.Parameter != null)
            {
                this._edit = true;
                this._oldReminder = (Reminder)e.Parameter;

                reminderComboBox.SelectedIndex = this._cmbb[this._oldReminder.Local];
                reminderNameTextBox.Text = this._oldReminder.Title;
                reminderDetailsTextBox.Text = this._oldReminder.Details;
            }
        }

        private void CancelBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (this._edit)
            {
                Frame.Navigate(typeof(MainPage), this._oldReminder);
            }

            this._edit = false;
            Frame.Navigate(typeof(MainPage), null); 
        }

        private void SaveBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (!this._edit)
            {
                var reminder = new Reminder(reminderNameTextBox.Text,
                                  reminderDetailsTextBox.Text,
                                  ((ComboBoxItem)reminderComboBox.SelectedItem).Content.ToString());

                Frame.Navigate(typeof(MainPage), reminder);
            }
            else
            {
                this._oldReminder.Title = reminderNameTextBox.Text;
                this._oldReminder.Details = reminderDetailsTextBox.Text;
                this._oldReminder.Local = ((ComboBoxItem)reminderComboBox.SelectedItem).Content.ToString();

                Frame.Navigate(typeof(MainPage), this._oldReminder);
            }
        }
    }
}