using Microsoft.Data.Sqlite;
using TodoApi.Models;
using Microsoft.EntityFrameworkCore.Sqlite;

namespace TodoApi.Data
{
    public class TodoItemData
    {
        
        private string _connectionString;

        public TodoItemData(string filename)
        {

            if (!File.Exists(filename))
            {
                File.Create(filename);
            }

            _connectionString = @$"Data Source={filename}";
            EnsureTablesCreated();
        }


        private void EnsureTablesCreated()
        {
            using var connection = new SqliteConnection(_connectionString);

            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());

            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"CREATE TABLE IF NOT EXISTS ""TodoItem"" (
	                                    ""id""	INTEGER NOT NULL,
	                                    ""created""	TEXT NOT NULL,
	                                    ""title""	TEXT,
	                                    ""isComplete""	INTEGER NOT NULL,
	                                    PRIMARY KEY(""id"" AUTOINCREMENT)
                                    );";

            command.ExecuteNonQuery();
        }

        public bool AddTodoItem(TodoItem todoItem)
        {
            try
            {
                using (var connection = new SqliteConnection(_connectionString))
                {
                    connection.Open();

                    SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());

                    var command = connection.CreateCommand();

                    command.CommandText = @"INSERT INTO ""TodoItem""
                                    (created, title, isComplete)
                                    VALUES ($created, $title, $isComplete)";

                    command.Parameters.AddWithValue("$created", todoItem.Created);
                    command.Parameters.AddWithValue("$title", todoItem.Title);
                    command.Parameters.AddWithValue("$isComplete", todoItem.IsComplete);

                    command.ExecuteNonQuery();

                    return true;
                }
            }catch(Exception ex)
            {
                return false;
            }
            
        }

        public IList<TodoItem> GetAllTodoItems()
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();

                SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());

                var command = connection.CreateCommand();

                command.CommandText = @"SELECT id, created, title, isComplete FROM ""TodoItem""";

                List<TodoItem> items = new();

                using var reader = command.ExecuteReader();
                
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    DateTime created = reader.GetDateTime(1);
                    string title = reader.GetString(2);
                    bool isComplete = reader.GetBoolean(3);

                    TodoItem todoItem = new()
                    {
                        Id = id,
                        Created = created,
                        Title = title,
                        IsComplete = isComplete
                    };

                    items.Add(todoItem);
                }

                return items;
            }
            catch
            {
                return new List<TodoItem>();
            }
        }

        public TodoItem GetTodoItem(int id)
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);

                SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());

                connection.Open();

                var command = connection.CreateCommand();

                command.CommandText = @"SELECT 
                                    created, title, isComplete 
                                    FROM ""TodoItem"" 
                                    WHERE id = $id
                                    LIMIT 1";

                command.Parameters.AddWithValue("$id", id);

                using var reader = command.ExecuteReader();

                reader.Read();

                DateTime created = reader.GetDateTime(0);
                string title = reader.GetString(1);
                bool isComplete = reader.GetBoolean(2);

                return new TodoItem()
                {
                    Id = id,
                    Created = created,
                    Title = title,
                    IsComplete = isComplete
                };
            }
            catch
            {
                return null;
            }
        }
    }
}
