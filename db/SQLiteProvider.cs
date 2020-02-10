namespace Morninger
{
    using System.Collections.Generic;
    using System.Data.SQLite;
    using System;

    internal class SQLiteProvider
    {
        private string connectionString;

        internal SQLiteProvider(string pathToDb)
        {
            this.connectionString = $"Data Source={pathToDb}";
        }

        internal void CreateDB(string directory)
        {
            throw new NotImplementedException();
        }

        #region user

        internal User SelectUser(long userId)
        {
            Console.WriteLine($"{nameof(SelectUser)}");

            User user = null;
            using (var con = new SQLiteConnection(connectionString))
            {
                con.Open();
                using (var cmd = new SQLiteCommand("select * from users where id = @id ", con))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@id", userId));
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            return null;
                        }

                        reader.Read();
                        user = new User
                        {
                            Id = (long)reader["Id"],
                            FirstName = reader["FirstName"] == DBNull.Value ? string.Empty : reader["FirstName"].ToString(),
                            LastName = reader["LastName"] == DBNull.Value ? string.Empty : reader["LastName"].ToString(),
                            Username = reader["Username"] == DBNull.Value ? string.Empty : reader["Username"].ToString(),
                            LastUpdate = DateTime.Parse(((string)reader["LastUpdate"]))
                        };
                    }
                }
                con.Close();
            }

            user.Statistic = SelectMonths(user.Id);
            return user;
        }

        internal void InsertUser(User user)
        {
            Console.WriteLine($"{nameof(InsertUser)}");

            using (var con = new SQLiteConnection(connectionString))
            {
                con.Open();
                using (var cmd = new SQLiteCommand("INSERT INTO users (Id, FirstName, LastName, Username, LastUpdate) VALUES (@Id, @FirstName, @LastName, @Username, @LastUpdate)", con))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@Id", user.Id));
                    cmd.Parameters.Add(new SQLiteParameter("@FirstName", user.FirstName));
                    cmd.Parameters.Add(new SQLiteParameter("@LastName", user.LastName));
                    cmd.Parameters.Add(new SQLiteParameter("@Username", user.Username));
                    cmd.Parameters.Add(new SQLiteParameter("@LastUpdate", DateTime.UtcNow.ToShortDateString()));
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
                using (var cmd = new SQLiteCommand("update users set FirstName = @FirstName, LastName = @LastName, Username = @Username, LastUpdate = @LastUpdate where id = @id", con))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@id", user.Id));
                    cmd.Parameters.Add(new SQLiteParameter("@FirstName", user.FirstName));
                    cmd.Parameters.Add(new SQLiteParameter("@LastName", user.LastName));
                    cmd.Parameters.Add(new SQLiteParameter("@Username", user.Username));
                    cmd.Parameters.Add(new SQLiteParameter("@LastUpdate", DateTime.UtcNow.ToShortDateString()));
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
        }

        #endregion user

        #region month

        internal List<Month> SelectMonths(long userId)
        {
            Console.WriteLine($"{nameof(SelectMonths)}");

            var months = new List<Month>();
            using (var con = new SQLiteConnection(connectionString))
            {
                con.Open();
                using (var cmd = new SQLiteCommand("select * from months where userid = @userid", con))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@userid", userId));
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            return null;
                        }

                        while (reader.Read())
                        {
                            var month = new Month(userId);
                            month.Year = (int)(long)reader["Year"];
                            month.Number = (int)(long)reader["Month"];
                            month.Done = (int)(long)reader["Done"];
                            month.DayOff = (int)(long)reader["DayOff"];
                            month.LastUpdate = DateTime.Parse((string)reader["LastUpdate"]);
                            months.Add(month);
                        }
                    }
                }

                con.Close();
            }

            return months;
        }

        internal void InsertMonth(Month month)
        {
            Console.WriteLine($"{nameof(InsertMonth)}");

            using (var con = new SQLiteConnection(connectionString))
            {
                con.Open();
                using (var cmd = new SQLiteCommand("INSERT INTO months (UserId, Year, Month, Done, DayOff, LastUpdate) VALUES (@UserId, @Year, @Month, @Done, @DayOff, @LastUpdate)", con))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@UserId", month.UserId));
                    cmd.Parameters.Add(new SQLiteParameter("@Year", month.Year));
                    cmd.Parameters.Add(new SQLiteParameter("@Month", month.Number));
                    cmd.Parameters.Add(new SQLiteParameter("@Done", month.Done));
                    cmd.Parameters.Add(new SQLiteParameter("@DayOff", month.DayOff));
                    cmd.Parameters.Add(new SQLiteParameter("@LastUpdate", month.LastUpdate.ToShortDateString()));
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
        }

        internal void UpdateMonth(Month month)
        {
            Console.WriteLine($"{nameof(UpdateMonth)}");

            using (var con = new SQLiteConnection(connectionString))
            {
                con.Open();
                using (var cmd = new SQLiteCommand("update months set Done = @Done, DayOff = @DayOff, LastUpdate = @LastUpdate where UserId = @UserId and Year = @Year and Month = @Month", con))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@UserId", month.UserId));
                    cmd.Parameters.Add(new SQLiteParameter("@Year", month.Year));
                    cmd.Parameters.Add(new SQLiteParameter("@Month", month.Number));
                    cmd.Parameters.Add(new SQLiteParameter("@Done", month.Done));
                    cmd.Parameters.Add(new SQLiteParameter("@DayOff", month.DayOff));
                    cmd.Parameters.Add(new SQLiteParameter("@LastUpdate", month.LastUpdate.ToShortDateString()));
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
        }

        #endregion month
    }
}