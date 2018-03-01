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

        public List<String> getTasks(){
            List<String> l = new List<String>();
            string stm = "Select distinct task From entry";
            using (SQLiteCommand cmd = new SQLiteCommand(stm, dbConnection))
            {
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        l.Add(dr.GetString(0));
                }
            }
            return l;
        }

        public List<MainWindow.Task> getEntries(int days, bool sum)
        {
            if (sum)
                return getAccEntries(days);
            else
                return getEntries(days);
        }

        public List<MainWindow.Task> getAccEntries(int days){
            List<MainWindow.Task> l = new List<MainWindow.Task>();
            string stm = "Select min(date), task, sum(time) From entry";
            if (days != 0)
                stm += " where date >= " + DateTime.Now.AddDays(-days).Ticks.ToString();
            stm += " Group by task";

            using (SQLiteCommand cmd = new SQLiteCommand(stm, dbConnection))
            {
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        l.Add(new MainWindow.Task(dr.GetInt64(0), dr.GetString(1), dr.GetInt64(2)));
                }
            }
            return l;
        }

        public List<MainWindow.Task> getEntries(int days){
            List<MainWindow.Task> l = new List<MainWindow.Task>();
            string stm = "Select * From entry";
            if (days != 0)
                stm += " where date >= " + DateTime.Now.AddDays(-days).Ticks.ToString() + ";";
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
