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

        public async void RemoveTodo(string title)
        {
            var todo = await _db.Conn.Table<Todo>().Where(t => t.Title == title).FirstOrDefaultAsync();
            if (todo != null)
            {
                await _db.Conn.DeleteAsync(todo);
            }
        }

        public async void UpdateTodo(Todo old, Todo newTodo)
        {
            var todo = await _db.Conn.Table<Todo>().Where(t => t.Title == newTodo.Title).FirstOrDefaultAsync();
            if (todo != null)
            {
                _db.Conn.InsertAsync(newTodo);
            }
        }

        public async Task<List<Todo>> FetchAllTodos()
        {
            return await _db.Conn.Table<Todo>().ToListAsync();
        }
    }
}
