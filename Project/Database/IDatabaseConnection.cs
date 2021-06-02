using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Database
{
    public interface IDatabaseConnection
    {
        SQLite.SQLiteConnection DbConnection();
        SQLite.SQLiteConnection DbConnectionStore();
        SQLite.SQLiteConnection DbConnectionCompleted();
        SQLite.SQLiteConnection DbConnectionMerchList();
    }
}
