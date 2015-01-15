using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Storage;
using SQLite;

namespace Remind.Me.Database
{
    public class Helper
    {
        private String DB_NAME = "remindme.db";

        public SQLiteAsyncConnection Conn { get; set; }

        public Helper()
        {
            Conn = new SQLiteAsyncConnection(DB_NAME);
            this.InitDb();
        }

        public async void InitDb()
        {
            // Create Db if not exist
            bool dbExist = await CheckDbAsync();

            if (!dbExist)
            {
                await CreateDatabaseAsync();
            }
        }

        public async Task<bool> CheckDbAsync()
        {
            bool dbExist = true;

            try
            {
                StorageFile sf = await ApplicationData.Current.LocalFolder.GetFileAsync(DB_NAME);
            }
            catch (Exception)
            {
                dbExist = false;
            }

            return dbExist;
        }

        private async Task CreateDatabaseAsync()
        {
            await Conn.CreateTableAsync<Reminder>();
        }
    }
}
