using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace DataAccessLibrary;

public class SqliteDataAccess
{
    public List<T> LoadData<T, U>(string sqlStatement, U parameters, string connectionString)
    {
        using (IDbConnection connection = new SQLiteConnection(connectionString))
        {
            List<T> rows = connection.Query<T>(sqlStatement, parameters).ToList();
            return rows;
        }
    }

    public void SaveData<T>(string sqlStatement, T parameters, string connectionString)
    {
        using (IDbConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Execute(sqlStatement, parameters);
        }
    }
}
