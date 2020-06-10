namespace Edomozh
{
    using System.Collections.Generic;
    using System;
    using System.IO;
    using System.Data.SQLite;

    internal class SQLiteProvider
    {
        private readonly string connectionString;

        internal SQLiteProvider()
        {
            var dbPath = $"{Directory.GetCurrentDirectory()}\\morninger.db";

            if (!File.Exists(dbPath))
                CreateDB(dbPath);

            connectionString = $"Data Source={dbPath}";
        }

        internal void CreateDB(string dbPath)
        {
            SQLiteConnection.CreateFile(dbPath);
            using (var con = new SQLiteConnection($"Data Source={dbPath}"))
            {
                con.Open();
                var createTable = new SQLiteCommand(Properties.Queries.CreateTables, con);
                createTable.ExecuteNonQuery();
            }
        }

        #region user

        internal User UserSelect(long userId)
        {
            Console.WriteLine($"{nameof(UserSelect)}");

            User user = null;
            using (var con = new SQLiteConnection(connectionString))
            {
                con.Open();
                using (var cmd = new SQLiteCommand(Properties.Queries.User_Select, con))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@id", userId));
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows) return null;

                        reader.Read();
                        user = new User()
                        {
                            Id = (long)reader["Id"],
                            FirstName = reader["FirstName"] == DBNull.Value ? string.Empty : reader["FirstName"].ToString(),
                            LastName = reader["LastName"] == DBNull.Value ? string.Empty : reader["LastName"].ToString(),
                            Username = reader["Username"] == DBNull.Value ? string.Empty : reader["Username"].ToString(),
                        };
                    }
                }
                con.Close();
            }

            user.Entries = EntrySelect(user.Id);
            return user;
        }

        internal void UserInsert(User user)
        {
            Console.WriteLine(nameof(UserInsert));

            using (var con = new SQLiteConnection(connectionString))
            {
                con.Open();
                using (var cmd = new SQLiteCommand(Properties.Queries.User_Insert, con))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@Id", user.Id));
                    cmd.Parameters.Add(new SQLiteParameter("@FirstName", user.FirstName));
                    cmd.Parameters.Add(new SQLiteParameter("@LastName", user.LastName));
                    cmd.Parameters.Add(new SQLiteParameter("@Username", user.Username));
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
        }

        internal void UpdateUser(User user)
        {
            Console.WriteLine($"{nameof(UpdateUser)}");

            using (var con = new SQLiteConnection(connectionString))
            {
                con.Open();
                using (var cmd = new SQLiteCommand(Properties.Queries.User_Update, con))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@id", user.Id));
                    cmd.Parameters.Add(new SQLiteParameter("@FirstName", user.FirstName));
                    cmd.Parameters.Add(new SQLiteParameter("@LastName", user.LastName));
                    cmd.Parameters.Add(new SQLiteParameter("@Username", user.Username));
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
        }

        #endregion user

        #region month

        internal List<Entry> EntrySelect(long userId)
        {
            Console.WriteLine($"{nameof(EntrySelect)}");

            var entries = new List<Entry>();
            using (var con = new SQLiteConnection(connectionString))
            {
                con.Open();
                using (var cmd = new SQLiteCommand(Properties.Queries.Entry_Select, con))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@UserId", userId));
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows) return entries;

                        while (reader.Read())
                        {
                            entries.Add(new Entry(userId)
                            {
                                Year = (int)(long)reader["Year"],
                                Month = (int)(long)reader["Month"],
                                Day = (int)(long)reader["Day"],
                                Status = reader["Status"] == DBNull.Value ? string.Empty : reader["Status"].ToString()
                            });
                        }
                    }
                }

                con.Close();
            }

            return entries;
        }

        internal void EntryInsert(Entry entry)
        {
            Console.WriteLine($"{nameof(EntryInsert)}");

            using (var con = new SQLiteConnection(connectionString))
            {
                con.Open();
                using (var cmd = new SQLiteCommand(Properties.Queries.Entry_Insert, con))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@UserId", entry.UserId));
                    cmd.Parameters.Add(new SQLiteParameter("@Year", entry.Year));
                    cmd.Parameters.Add(new SQLiteParameter("@Month", entry.Month));
                    cmd.Parameters.Add(new SQLiteParameter("@Day", entry.Day));
                    cmd.Parameters.Add(new SQLiteParameter("@Status", entry.Status));
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
        }

        #endregion month
    }
}