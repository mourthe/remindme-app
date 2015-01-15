using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Remind.Me.Database
{
    public static class Main 
    {
        private static string _fileName = "settings.txt";
        private static string _folderName = "DataFolder";

        public static void SaveTodo(Todo t)
        {

        }

        public static void SaveReminder(Reminder t)
        {

        }

        public static void UpdateTodo(Todo old, Todo newT)
        {

        }

        public static void UpdateReminder(Reminder old, Reminder newR)
        {

        }

        public static void RemoveTodo(Todo t)
        {

        }

        public static void RemoveReminder(Reminder t)
        {
            
        }

        public static async Task<int> SaveSettings(string settings)
        {
            // Get the text data from the textbox. 
            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(settings.ToCharArray());

            // Get the local folder.
            var local = Windows.Storage.ApplicationData.Current.LocalFolder;

            // Create a new folder name DataFolder.
            var dataFolder = await local.CreateFolderAsync(_folderName,
                                        CreationCollisionOption.OpenIfExists);

            // Create a new file named DataFile.txt.
            var file = await dataFolder.CreateFileAsync(_fileName,
                                        CreationCollisionOption.ReplaceExisting);

            // Write the data from the textbox.
            using (var s = await file.OpenStreamForWriteAsync())
            {
                s.Write(fileBytes, 0, fileBytes.Length);
            }

            return 0;
        }

        public static async Task<string> GetSettings()
        {
            // Get the local folder.
            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;

            if (local != null)
            {
                try
                {
                    // Get the DataFolder folder.
                    var dataFolder = await local.GetFolderAsync(_folderName);

                    // Get the file.
                    var file = await dataFolder.OpenStreamForReadAsync(_fileName);

                    // Read the data.
                    using (StreamReader streamReader = new StreamReader(file))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
                catch (Exception) { }
            }

            return null;
        }
    }
}
