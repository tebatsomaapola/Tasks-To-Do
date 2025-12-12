using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace XamarinForms
{
    public class TaskDataBase
    {
        private readonly string _connectionString;

        public TaskDataBase(string dbPath)
        {
            if (string.IsNullOrEmpty(dbPath))
                throw new ArgumentException("Database path cannot be null or empty", nameof(dbPath));

            _connectionString = $"Data Source={dbPath};Version=3;";
            CreateDataBase();
        }

        private SQLiteConnection CreateConnection()
        {
            var conn = new SQLiteConnection(_connectionString);
            conn.Open();
            return conn;
        }

        private void CreateDataBase()
        {
            using var conn = CreateConnection();

            string tableCmd = @"CREATE TABLE IF NOT EXISTS Tasks (
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Title TEXT NOT NULL,
                                    Description TEXT,
                                    Status TEXT,
                                    DueDate TEXT,
                                    CreatedOn TEXT,
                                    IsCompleted INTEGER)";
            using var cmd = new SQLiteCommand(tableCmd, conn);
            cmd.ExecuteNonQuery();

            // Seed sample data only if table is empty
            string countCmd = "SELECT COUNT(*) FROM Tasks";
            using var countCheck = new SQLiteCommand(countCmd, conn);
            long count = (long)countCheck.ExecuteScalar();

            if (count == 0)
            {
                string insertCmd = @"INSERT INTO Tasks 
                    (Title, Description, Status, DueDate, CreatedOn, IsCompleted)
                    VALUES (@title, @desc, @status, @due, @created, @done)";

                using var insert = new SQLiteCommand(insertCmd, conn);

                // Sample Task 1
                insert.Parameters.AddWithValue("@title", "Buy groceries");
                insert.Parameters.AddWithValue("@desc", "Milk, bread, eggs, fruit");
                insert.Parameters.AddWithValue("@status", "Pending");
                insert.Parameters.AddWithValue("@due", DateTime.Now.AddDays(1).ToString("s"));
                insert.Parameters.AddWithValue("@created", DateTime.Now.ToString("s"));
                insert.Parameters.AddWithValue("@done", 0);
                insert.ExecuteNonQuery();

                // Sample Task 2
                insert.Parameters["@title"].Value = "Finish project report";
                insert.Parameters["@desc"].Value = "Complete the final draft for submission";
                insert.Parameters["@status"].Value = "Doing";
                insert.Parameters["@due"].Value = DateTime.Now.AddDays(3).ToString("s");
                insert.Parameters["@created"].Value = DateTime.Now.ToString("s");
                insert.Parameters["@done"].Value = 0;
                insert.ExecuteNonQuery();

                // Sample Task 3
                insert.Parameters["@title"].Value = "Call plumber";
                insert.Parameters["@desc"].Value = "Fix leaking kitchen sink";
                insert.Parameters["@status"].Value = "Parked";
                insert.Parameters["@due"].Value = DateTime.Now.AddDays(7).ToString("s");
                insert.Parameters["@created"].Value = DateTime.Now.ToString("s");
                insert.Parameters["@done"].Value = 0;
                insert.ExecuteNonQuery();
            }
        }

        public List<TaskItem> GetTasks()
        {
            var tasks = new List<TaskItem>();

            using var conn = CreateConnection();
            string selectCmd = "SELECT Id, Title, Description, Status, DueDate, CreatedOn, IsCompleted FROM Tasks";

            using var cmd = new SQLiteCommand(selectCmd, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                tasks.Add(new TaskItem
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Title = reader["Title"]?.ToString(),
                    Description = reader["Description"]?.ToString(),
                    Status = reader["Status"]?.ToString(),
                    DueDate = reader["DueDate"] != DBNull.Value ? DateTime.Parse(reader["DueDate"].ToString()) : DateTime.MinValue,
                    CreatedOn = reader["CreatedOn"] != DBNull.Value ? DateTime.Parse(reader["CreatedOn"].ToString()) : DateTime.MinValue,
                    IsCompleted = reader["IsCompleted"] != DBNull.Value && Convert.ToInt32(reader["IsCompleted"]) == 1
                });
            }

            return tasks;
        }
        public void UpdateTask(TaskItem task)
{
    using var conn = CreateConnection();

    string updateCmd = @"UPDATE Tasks 
                         SET Title = @title,
                             Description = @desc,
                             Status = @status,
                             DueDate = @due,
                             IsCompleted = @done
                         WHERE Id = @id";

    using var cmd = new SQLiteCommand(updateCmd, conn);
    cmd.Parameters.AddWithValue("@title", task.Title);
    cmd.Parameters.AddWithValue("@desc", task.Description);
    cmd.Parameters.AddWithValue("@status", task.Status);
    cmd.Parameters.AddWithValue("@due", task.DueDate.ToString("s"));
    cmd.Parameters.AddWithValue("@done", task.IsCompleted ? 1 : 0);
    cmd.Parameters.AddWithValue("@id", task.Id);

    cmd.ExecuteNonQuery();
}


        public void SaveTask(TaskItem task)
        {
           using var conn = CreateConnection();

           string insertCmd = @"INSERT INTO Tasks 
                         (Title, Description, Status, DueDate, CreatedOn, IsCompleted)
                         VALUES (@title, @desc, @status, @due, @created, @done)";
           using var cmd = new SQLiteCommand(insertCmd, conn);
                     cmd.Parameters.AddWithValue("@title", task.Title);
                     cmd.Parameters.AddWithValue("@desc", task.Description);
                     cmd.Parameters.AddWithValue("@status", task.Status);
                     cmd.Parameters.AddWithValue("@due", task.DueDate.ToString("s"));
                     cmd.Parameters.AddWithValue("@created", DateTime.Now.ToString("s"));
                     cmd.Parameters.AddWithValue("@done", task.IsCompleted ? 1 : 0);
                     cmd.ExecuteNonQuery();

    // Debug: confirm row count
    using var checkCmd = new SQLiteCommand("SELECT COUNT(*) FROM Tasks", conn);
    var count = (long)checkCmd.ExecuteScalar();
    System.Diagnostics.Debug.WriteLine($"Tasks in DB: {count}");
   }

    
    public void DeleteTask(int Id)
     {
    using var conn = CreateConnection();

    string deleteCmd = "DELETE FROM Tasks WHERE Id = @id";
    using var cmd = new SQLiteCommand(deleteCmd, conn);
    cmd.Parameters.AddWithValue("@id", Id);

    cmd.ExecuteNonQuery();
     }
}
}
