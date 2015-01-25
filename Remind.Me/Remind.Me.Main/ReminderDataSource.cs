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

        public async void AddReminder(Reminder r)
        {
            await _db.Conn.InsertAsync(r);
        }

        public async void RemoveReminder(string id)
        {
            var reminder = await _db.Conn.Table<Reminder>().Where(r => r.Id == id).FirstOrDefaultAsync();
            if (reminder != null)
            {
                await _db.Conn.DeleteAsync(reminder);
            }
        }

        public async void UpdateReminder(Reminder r)
        {
            var Reminder = await _db.Conn.Table<Reminder>().Where(t => t.Id == r.Id).FirstOrDefaultAsync();
            if (Reminder != null)
            {
                _db.Conn.UpdateAsync(r);
            }
        }

        public async Task<List<Reminder>> FetchAllReminders()
        {
            var content = _db.Conn.Table<Reminder>().ToListAsync().Result;

            return content;
        }
    }
}
