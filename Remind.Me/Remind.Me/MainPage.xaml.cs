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

using Todo = Remind.Me.Database.Todo;
using Reminder= Remind.Me.Database.Reminder;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Remind.Me
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class MainPage : Page
    {
        private ObservableCollection<Reminder> _reminders;
        private ObservableCollection<Todo> _todos;
        private Dictionary<string, Reminder> _remindersDic;
        private Dictionary<string, Todo> _todoDic;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.LoadCollections();
        }

        private void LoadCollections()
        {
            // try to load the todos
            var tds = Database.Main.FetchAllTodos().Result;
            if (tds.Count == 0)
                this._todos = new ObservableCollection<Todo>();
            else
                this._todos = new ObservableCollection<Todo>(tds);
            
            // try to load the reminders
            var rmdrs = Database.Main.FetchAllReminders().Result;
            if (rmdrs.Count == 0)
                this._reminders = new ObservableCollection<Reminder>();
            else
                this._reminders = new ObservableCollection<Reminder>(rmdrs);

            // creates the dictionaries
            this._remindersDic = new Dictionary<string, Reminder>();
            this._todoDic = new Dictionary<string, Todo>();
            
            // populates the dictionaries
            if (this._todos.Count > 0)
                foreach (var t in this._todos)
                    this._todoDic.Add(t.Id, t);
                
            if (this._reminders.Count > 0)
                foreach (var r in this._reminders)
                    this._remindersDic.Add(r.Id, r);

            reminderXAML.Source = this._reminders;
            todoXAML.Source = this._todos;


            // TEST
            this.Test();
        }

        #region Navigation

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Load the reminders list and todos
            if (e.Parameter != null && CameFromAddRemindersPage())
            {
                var r = (Reminder)e.Parameter;

                if (!this._remindersDic.ContainsKey(r.Id))
                {
                    this._remindersDic.Add(r.Id, r);
                    Database.Main.SaveReminder(r);
                }
                else
                {
                    Database.Main.UpdateReminder(r);
                }

                this._reminders = new ObservableCollection<Reminder>(this._remindersDic.Values.ToList());

                reminderXAML.Source = this._reminders;
            }

            if (e.Parameter != null &&  CameFromAddTodoPage())
            {
                var t = (Todo)e.Parameter;

                if (!this._todoDic.ContainsKey(t.Id))
                {
                    this._todoDic.Add(t.Id, t);
                    Database.Main.SaveTodo(t);
                }
                else
                {
                    Database.Main.UpdateTodo(t);
                }

                this._todos = new ObservableCollection<Todo>(this._todoDic.Values.ToList());

                todoXAML.Source = this._todos;
            }
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

        #endregion

        #region Commands

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
            Frame.Navigate(typeof(SettingsPage));
        }

        #endregion

        #region Geofence

        private void SendActiveReminders()
        {
            var localList = _reminders.Select(r => r.Local).ToList();

            TaskBackground.GeofencesHelper.RemindersPlaces = localList;
        }

        private void SendSettings()
        {
            var settings = Remind.Me.Database.Main.GetSettings().Result;

            TaskBackground.GeofencesHelper.SetSettings(settings);
        }

        private void Test()
        {
            var bingS = new TaskBackground.BingService();

            foreach (var b in bingS.ListNearbyPlaces)
            {
                TaskBackground.GeofencesHelper.CreateGeofence(b.Key, b.Value.Position.Latitude, b.Value.Position.Longitude);
            }

            TaskBackground.LocationTask.Register();
        }

        #endregion

        #region TODO

        private void TodoCheckBox_Click(object sender, RoutedEventArgs e)
        {
            // delete the checked todo
            var idx = GetTodoIdx();

            // remove from the database and lists
            var current = this._todos.ElementAt(idx);

            this._todoDic.Remove(current.Id);
            Database.Main.RemoveTodo(current.Id);
            this._todos.RemoveAt(idx);

        }

        private void lvtodos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lvi = ((sender as ListView).SelectedItem as ListViewItem);
        }
        
        private void TodoEdit_Click(object sender, RoutedEventArgs e)
        {
            // get the index of the TODO
            var idx = GetTodoIdx();

            Frame.Navigate(typeof(AddTodoPage), this._todos.ElementAt(idx));
            this._todos.RemoveAt(idx);
        }

        private int GetTodoIdx()
        {
            var idx = -1;
            for (int i = 0; i < this._todos.Count; i++)
            {
                if (this._todos.ElementAt(i).Id.Equals((lvtodos.SelectedItems[0] as Todo).Id))
                {
                    idx = i; break;
                }
            }
            return idx;
        }

        private int GetTodoIdx(string id)
        {
            var idx = -1;
            for (int i = 0; i < this._todos.Count; i++)
            {
                if (this._todos.ElementAt(i).Id.Equals(id))
                {
                    idx = i; break;
                }
            }
            return idx;
        }

        private void TodoDetails_Click(object sender, RoutedEventArgs e)
        {
            var idx = GetTodoIdx();

            Frame.Navigate(typeof(TodoDetails), this._todos.ElementAt(idx));
        }

        #endregion

        #region Reminder

        private void ListViewItem_Holding(object sender, HoldingRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);

            flyoutBase.ShowAt(senderElement);
        }

        private void DeleteRemindersItem_Click(object sender, RoutedEventArgs e)
        {
            var idx = GetReminderIdx();

            // remove from the database
            var current = this._reminders.ElementAt(idx);

            Database.Main.RemoveReminder(current.Id);
            this._remindersDic.Remove(current.Id);
            this._reminders.RemoveAt(idx);
        }

        private int GetReminderIdx()
        {
            var idx = -1;
            for (int i = 0; i < this._reminders.Count; i++)
            {
                if (this._reminders.ElementAt(i).Id.Equals((lvreminders.SelectedItem as Reminder).Id))
                {
                    idx = i; break;
                }
            }
            return idx;
        }

        private int GetReminderIdx(string id)
        {
            var idx = -1;
            for (int i = 0; i < this._reminders.Count; i++)
            {
                if (this._reminders.ElementAt(i).Id.Equals(id))
                {
                    idx = i; break;
                }
            }
            return idx;
        }

        private void EditReminder_Click(object sender, RoutedEventArgs e)
        {
            // get the index of the Reminder
            var idx = -1;
            for (int i = 0; i < this._reminders.Count; i++)
            {
                if (this._reminders.ElementAt(i).Title.Equals((lvreminders.SelectedItems[0] as Reminder).Title))
                {
                    idx = i; break;
                }
            }

            Frame.Navigate(typeof(AddReminderPage), this._reminders.ElementAt(idx));
            this._reminders.RemoveAt(idx);
        }

        private void reminderToggle_Toggled(object sender, RoutedEventArgs e)
        {
            if (this._reminders.Count > 0)
            {
                var idx = GetReminderIdx();

                var r = this._reminders.ElementAt(idx);
                r.Active = (sender as ToggleSwitch).IsOn;
                Database.Main.UpdateReminder(r);
            }
        }
        
        private void ReminderDetails_Click(object sender, RoutedEventArgs e)
        {
            var idx = GetReminderIdx();
            Frame.Navigate(typeof(ReminderDetails), this._reminders.ElementAt(idx));
        }

        #endregion

    }
}
