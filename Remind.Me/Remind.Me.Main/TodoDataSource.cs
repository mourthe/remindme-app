using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remind.Me.Database
{
    public class TodoDataSource
    {   
        private Helper _db;

        public TodoDataSource(Helper databaseHelper)
        {
            this._db = databaseHelper;
        }

        public async void AddTodo(Todo todo)
        {
            await _db.Conn.InsertAsync(todo);
        }

        public async void RemoveTodo(string id)
        {
            var todo = await _db.Conn.Table<Todo>().Where(t => t.Id == id).FirstOrDefaultAsync();
            if (todo != null)
            {
                await _db.Conn.DeleteAsync(todo);
            }
        }

        public async void UpdateTodo(Todo td)
        {
            var todo = await _db.Conn.Table<Todo>().Where(t => t.Id == td.Id).FirstOrDefaultAsync();
            if (todo != null)
            {
                _db.Conn.UpdateAsync(td);
            }
        }

        public async Task<List<Todo>> FetchAllTodos()
        {
            return await _db.Conn.Table<Todo>().ToListAsync();
        }
    }
}
