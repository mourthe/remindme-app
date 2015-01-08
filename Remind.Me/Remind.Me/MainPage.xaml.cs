using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Remind.Me
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<Reminder> reminders;
        private ObservableCollection<Todo> todos;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.reminders = new ObservableCollection<Reminder>();
            this.todos = new ObservableCollection<Todo>();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Load the reminders list and todos
            if (CameFromAddRemindersPage())
            {
                this.reminders.Add(((Reminder)e.Parameter));

                reminderXAML.Source = this.reminders;
            }

            if (CameFromAddTodoPage())
            {
                this.todos.Add(((Todo)e.Parameter));

                todoXAML.Source = this.todos;
            }

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }       

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {            
            switch (pivot.SelectedIndex)
            {
                case 0: // Bring AddReminderPage
                    Frame.Navigate(typeof(AddReminderPage));
                    break;

                case 1:
                    Frame.Navigate(typeof(AddTodoPage));
                    break;
            }
        }

        private void configuracoes_Click(object sender, RoutedEventArgs e)
        {
            //Frame.Navigate(typeof(SettingsPage));
        }

        private bool CameFromAddRemindersPage()
        {
            return Frame.BackStack.FirstOrDefault() != null &&
                Frame.BackStack.Last().SourcePageType.ToString() == "Remind.Me.AddReminderPage";
        }

        private bool CameFromAddTodoPage()
        {
            return Frame.BackStack.FirstOrDefault() != null &&
                Frame.BackStack.Last().SourcePageType.ToString() == "Remind.Me.AddTodoPage";
        }

        private void TodoCheckBox_Click(object sender, RoutedEventArgs e)
        {
            // delete the checked todo
            try
            {
                var idx = -1;
                for (int i = 0; i < this.todos.Count; i++)
                {
                    if (this.todos.ElementAt(i).Title.Equals((lvtodos.SelectedItems[0] as Todo).Title))
                    {
                        idx = i; break;
                    }
                }

                this.todos.RemoveAt(idx);
            }            
            catch(Exception){

            }
        }

        private void lvtodos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lvi = ((sender as ListView).SelectedItem as ListViewItem);
        }
    }
}
