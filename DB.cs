namespace Morninger
{
    using System.Collections.Generic;
    using System.Data.SQLite;
    using System;

    internal class DB
    {
        private string pathToDb;
        internal DB(string pathToDb)
        {
            this.pathToDb = pathToDb;
        }

        internal void CreateDB(string directory)
        {
            throw new NotImplementedException();
        }

        internal List<Month> SelectMonths(long userId)
        {
            using var con = new SQLiteConnection($"Data Source={pathToDb}");
            using var cmd = new SQLiteCommand("select * from months where id = @id ", con);
            con.Open();

            cmd.Parameters.Add(new SQLiteParameter("@id", userId));
            var result = cmd.ExecuteReader();

            var months = new List<Month>();
            while (result.Read())
            {
                months.Add(new Month
                {
                    UserId = (long)result["UserId"],
                    Year = (int)result["Year"],
                    Number = (int)result["Month"],
                    Done = (int)result["Done"],
                    DayOff = (int)result["DayOff"]
                });
            }

            return months;
        }

        internal User SelectUser(long userId)
        {
            using var con = new SQLiteConnection($"Data Source={pathToDb}");
            using var cmd = new SQLiteCommand("select * from users where id = @id ", con);
            con.Open();

            cmd.Parameters.Add(new SQLiteParameter("@id", userId));
            var row = cmd.ExecuteReader();
            row.Read();

            return row.HasRows ? new User
            {
                Id = (long)row["Id"],
                FirstName = (string)row["FirstName"],
                LastName = (string)row["LastName"],
                Username = (string)row["Username"],
                LastUpdate = DateTime.Parse(((string)row["LastUpdate"]))
            } : null;
        }

        internal void InsertUser(User user)
        {
            using var con = new SQLiteConnection($"Data Source={pathToDb}");
            using var cmd = con.CreateCommand();
            con.Open();

            cmd.CommandText = @"INSERT INTO User (Id, FirstName, LastName, Username, LastUpdate) 
                                VALUES (@UserId, @FirstName, @LastName, @Username, @LastUpdate)";

            cmd.Parameters.Add(new SQLiteParameter("@Id", user.Id));
            cmd.Parameters.Add(new SQLiteParameter("@FirstName", user.FirstName));
            cmd.Parameters.Add(new SQLiteParameter("@LastName", user.LastName));
            cmd.Parameters.Add(new SQLiteParameter("@Username", user.Username));
            cmd.Parameters.Add(new SQLiteParameter("@LastUpdate", DateTime.UtcNow.ToShortDateString()));

            cmd.ExecuteNonQuery();
            con.Close();
        }

        internal void UpdateUser(User user)
        {
            using var con = new SQLiteConnection($"Data Source={pathToDb}");
            using var cmd = con.CreateCommand();
            con.Open();

            cmd.CommandText = @"update user set FirstName = @FirstName, LastName = @LastName, Username = @Username, LastUpdate = @LastUpdate where id = @id";

            cmd.Parameters.Add(new SQLiteParameter("@id", user.Id));
            cmd.Parameters.Add(new SQLiteParameter("@FirstName", user.FirstName));
            cmd.Parameters.Add(new SQLiteParameter("@LastName", user.LastName));
            cmd.Parameters.Add(new SQLiteParameter("@Username", user.Username));
            cmd.Parameters.Add(new SQLiteParameter("@LastUpdate", DateTime.UtcNow.ToShortDateString()));

            cmd.ExecuteNonQuery();
            con.Close();
        }

        internal void InsertMonth(Month month)
        {
            using var con = new SQLiteConnection($"Data Source={pathToDb}");
            using var cmd = con.CreateCommand();
            con.Open();

            cmd.CommandText = @"INSERT INTO UserMonth (UserId, Year, LastName, Username, LastUpdate) 
                                VALUES (@UserId, @FirstName, @LastName, @Username, @LastUpdate)";

            cmd.Parameters.Add(new SQLiteParameter("@UserId", month.UserId));
            cmd.Parameters.Add(new SQLiteParameter("@Year", month.Year));
            cmd.Parameters.Add(new SQLiteParameter("@Month", month.Number));
            cmd.Parameters.Add(new SQLiteParameter("@Done", month.Done));
            cmd.Parameters.Add(new SQLiteParameter("@DayOff", month.DayOff));

            cmd.ExecuteNonQuery();
            con.Close();
        }

        internal void UpdateMonth(Month month)
        {
            using var con = new SQLiteConnection($"Data Source={pathToDb}");
            using var cmd = con.CreateCommand();
            con.Open();

            cmd.CommandText = @"update user set Done = @Done, DayOff = @DayOff where UserId = @UserId and Year = @Year and Month = @Month";

            cmd.Parameters.Add(new SQLiteParameter("@UserId", month.UserId));
            cmd.Parameters.Add(new SQLiteParameter("@Year", month.Year));
            cmd.Parameters.Add(new SQLiteParameter("@Month", month.Number));
            cmd.Parameters.Add(new SQLiteParameter("@Done", month.Done));
            cmd.Parameters.Add(new SQLiteParameter("@DayOff", month.DayOff));

            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}