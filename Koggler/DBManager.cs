using System;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using System.Data.SQLite;

namespace Koggler
{
    public class DBManager
    {
        private static String dbFileName;
        private SQLiteConnection dbConnection;

        public DBManager()
        {
            String dbConnectionString = ConfigurationManager.AppSettings["connectionString"];
            dbFileName = ConfigurationManager.AppSettings["dataBaseFileName"];
            if (!File.Exists(dbFileName))
            {
                SQLiteConnection.CreateFile(dbFileName);
                dbConnection = new SQLiteConnection(dbConnectionString);
                dbConnection.Open();
                createTables();
            }else{
                dbConnection = new SQLiteConnection(dbConnectionString);
                dbConnection.Open();
            }

        }

        ~DBManager(){
            dbConnection.Close();
        }

        private void createTables(){
            using (SQLiteCommand cmd = dbConnection.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE entry(date int, task text, time int); ";
                cmd.ExecuteNonQuery();
            }
        }

        public void addEntry(MainWindow.Task task){
            using(SQLiteCommand cmd = dbConnection.CreateCommand()){
                cmd.CommandText = "Insert into entry(date, task, time) Values(@date, @task, @time)";
                cmd.Parameters.AddWithValue("@date", task.Date.Ticks);
                cmd.Parameters.AddWithValue("@task", task.Name);
                cmd.Parameters.AddWithValue("@time", task.Duration.Ticks);
                cmd.ExecuteNonQuery();
            }
        }

        public List<MainWindow.Task> getEntries(){
            List<MainWindow.Task> l = new List<MainWindow.Task>();
            string stm = "Select * From entry";
            using (SQLiteCommand cmd = new SQLiteCommand(stm, dbConnection)){
                using(SQLiteDataReader dr = cmd.ExecuteReader()){
                    while(dr.Read())
                        l.Add(new MainWindow.Task(dr.GetInt64(0), dr.GetString(1),dr.GetInt64(2)));
                }
            }
            return l;
        }
    }
}
