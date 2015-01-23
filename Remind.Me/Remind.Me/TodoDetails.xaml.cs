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

using Todo = Remind.Me.Database.Todo;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Remind.Me
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TodoDetails : Page
    {
        private Todo _todo;

        public TodoDetails()
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
            this._todo = (Todo)e.Parameter;

            todoNameTextBox.Text = _todo.Title;
            todoDetailsTextBox.Text = _todo.Details;
        }

        private void BackBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), null);
        }

        private void EditBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddTodoPage), this._todo);
        }
    }
}
