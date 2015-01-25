using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class AddTodoPage : Page
    {
        private bool _edit;
        private Todo _oldTodo;

        public AddTodoPage()
        {            
            this._edit = false;
            this._oldTodo = null;

            this.InitializeComponent();
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
                this._oldTodo = (Todo)e.Parameter;

                todoNameTextBox.Text = _oldTodo.Title;
                todoDetailsTextBox.Text = _oldTodo.Details;
            }
        }

        private void CancelBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (this._edit)
            {
                Frame.Navigate(typeof(MainPage), _oldTodo);
            }

            this._edit = false;
            Frame.Navigate(typeof(MainPage), null);
        }

        private async void SaveBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (todoNameTextBox.Text == string.Empty)
            {
                var msgbox = new MessageDialog("Titulo não pode ser vazio.");
                await msgbox.ShowAsync();
            }
            else
            {
                if (_edit)
                {
                    _oldTodo.Title = todoNameTextBox.Text;
                    _oldTodo.Details = todoDetailsTextBox.Text;

                    Frame.Navigate(typeof(MainPage), _oldTodo);
                }
                else
                {
                    var newTodo = new Todo(todoNameTextBox.Text, todoDetailsTextBox.Text);

                    Frame.Navigate(typeof(MainPage), newTodo);
                }
            }
        }
    }
}
