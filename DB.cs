namespace Morninger
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Data.SQLite;
    using System;

    internal class DB
    {
        private string pathToDb;
        DB(string pathToDb)
        {
            this.pathToDb = pathToDb;
        }
        
        internal User SelectUser()
        {
            using var con = new SQLiteConnection($"Data Source={pathToDb}");
            con.Open();

            using var cmd = new SQLiteCommand("SELECT SQLITE_VERSION()", con);
            string version = cmd.ExecuteScalar().ToString();

            Console.WriteLine($"SQLite version: {version}");
            return new User { };
        }
    }
}