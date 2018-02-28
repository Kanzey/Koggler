using System;
using System.IO;
using System.Configuration;
using System.Data.SQLite;

namespace Koggler
{
    public class DBmanager
    {
        private static String dbFileName;
        private static 
        private SQLiteConnection dbConnection;

        public DBmanager()
        {
            String dbConnectionString = ConfigurationManager.AppSettings["connectionString"];
            dbFileName = ConfigurationManager.AppSettings["dataBaseFileName"];
            if (!File.Exists(dbFileName))
            {
                SQLiteConnection.CreateFile(dbFileName);
                dbConnection = new SQLiteConnection(dbConnectionString);
                createTables();
            }else{
                dbConnection = new SQLiteConnection(dbConnectionString);
            }
            dbConnection.Open();
        }

        ~DBmanager(){
            dbConnection.Close();
        }

        private void createTables(){
            using (SQLiteCommand cmd = dbConnection.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE " +
                    "IF NOT EXISTS entry(date int, task text, time int);";
                cmd.ExecuteNonQuery();
            }
        }

        public void addEntry(MainWindow.Task task){
            using(SQLiteCommand cmd = dbConnection.CreateCommand()){
                cmd.CommandText = "";
            }
        }
    }
}
