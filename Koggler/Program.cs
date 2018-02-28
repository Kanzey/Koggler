using System;
using Gtk;

namespace Koggler
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Application.Init();
            DBmanager dbm = new DBmanager();
            MainWindow win = new MainWindow();
            win.Show();
            Application.Run();
        }
    }
}
