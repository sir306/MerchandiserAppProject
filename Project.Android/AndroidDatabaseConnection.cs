using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using SQLite;
using Project.Droid;
using Project.Database;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidDatabaseConnection))]

namespace Project.Droid
{
    public class AndroidDatabaseConnection : IDatabaseConnection
    {
        public SQLiteConnection DbConnection()
        {
            var dbName = "TicketDatabase.db3";
            var path = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.Personal), dbName);
            return new SQLiteConnection(path);
        }
        public SQLiteConnection DbConnectionStore()
        {
            var dbName = "StoreDatabase.db3";
            var path = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.Personal), dbName);
            return new SQLiteConnection(path);
        }
        public SQLiteConnection DbConnectionCompleted()
        {
            var dbName = "CompletedTicketDatabase.db3";
            var path = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.Personal), dbName);
            return new SQLiteConnection(path);
        }
        public SQLiteConnection DbConnectionMerchList()
        {
            var dbName = "MerchListDatabase.db3";
            var path = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.Personal), dbName);
            return new SQLiteConnection(path);
        }
    }
}
