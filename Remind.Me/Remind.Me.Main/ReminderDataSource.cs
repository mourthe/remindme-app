using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remind.Me.Database
{
    public class ReminderDataSource
    {
        private Helper _db;

        public ReminderDataSource(Helper databaseHelper)
        {
            this._db = databaseHelper;
        }

        public async void AddReminder(Reminder reminder)
        {
            await _db.Conn.InsertAsync(reminder);
        }

        public async void RemoveReminder(string id)
        {
            var reminder = await _db.Conn.Table<Reminder>().Where(r => r.Id == id).FirstOrDefaultAsync();
            if (reminder != null)
            {
                await _db.Conn.DeleteAsync(reminder);
            }
        }

        public async void UpdateReminder(Reminder newReminder)
        {
            var Reminder = await _db.Conn.Table<Reminder>().Where(t => t.Id == newReminder.Id).FirstOrDefaultAsync();
            if (Reminder != null)
            {
                _db.Conn.UpdateAsync(newReminder);
            }
        }

        public async Task<List<Reminder>> FetchAllReminders()
        {
            return await _db.Conn.Table<Reminder>().ToListAsync();
        }
    }
}
