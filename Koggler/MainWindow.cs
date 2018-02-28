using System;
using System.Timers;
using System.Diagnostics;
using System.Data.SQLite;
using Gtk;

public partial class MainWindow : Gtk.Window
{
    Timer Clock;
    Stopwatch stopwatch;
    static Gdk.Color startColor = new Gdk.Color(114, 165, 85);
    static Gdk.Color stopColor = new Gdk.Color(202, 86, 112);
    Koggler.DBManager dbManager = new Koggler.DBManager();

    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Clock = new Timer();
        Clock.Elapsed += new ElapsedEventHandler(Timer_Tick);
        stopwatch = new Stopwatch();
        Clock.Interval = 1000;
        Clock.AutoReset = true;
        Build();
        //changeElementColor(this, new Gdk.Color(99,140,204));
        store = CreateModel();
        treeview1.Model = store;
        treeview1.RulesHint = true;
        treeview1.SearchColumn = (int)Column.Duration;
        AddColumns(treeview1);
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }


    public void updateTime(){
        Gtk.Application.Invoke(delegate{
            time.Text = string.Format("{0:HH:mm:ss}", new DateTime(stopwatch.ElapsedTicks));
        });
    }

    public void Timer_Tick(object sender, EventArgs eArgs){
        if (sender == Clock)
            updateTime();
    }

    protected void OnStartButtonClicked(object sender, EventArgs e)
    {
        
        if (Clock.Enabled)
        {
            Task t = new Task(DateTime.Now.Ticks, taskCombobox.ActiveText, stopwatch.ElapsedTicks);
            dbManager.addEntry(t);
            Clock.Stop();
            stopwatch.Reset();
            store.AppendValues(t.Date.ToString(), t.Name, t.Duration.ToString());
            taskCombobox.Sensitive = true;
            time.Text = "00:00:00";
            changeElementColor(startButton, startColor);
            startButton.Label = "Start";
        }
        else
        {
            changeElementColor(startButton, stopColor);
            startButton.Label = "Stop";
            stopwatch.Start();
            Clock.Start();
            taskCombobox.Sensitive = false;
        }
    }

    private void changeElementColor(Gtk.Widget widget, Gdk.Color color){
        widget.ModifyBg(StateType.Active, color);
        widget.ModifyBg(StateType.Prelight, color);
        widget.ModifyBg(StateType.Normal, color);
    }

    ListStore store;

    private void AddColumns(TreeView treeView)
    {
        TreeViewColumn column;
        CellRendererText rendererText;

        rendererText = new CellRendererText();
        column = new TreeViewColumn("Date", rendererText, "text", Column.Date);
        column.SortColumnId = (int)Column.Date;
        treeView.AppendColumn(column);

        rendererText = new CellRendererText();
        column = new TreeViewColumn("Task", rendererText, "text", Column.Name);
        column.SortColumnId = (int)Column.Name;
        treeView.AppendColumn(column);

        rendererText = new CellRendererText();
        column = new TreeViewColumn("Duration", rendererText, "text", Column.Duration);
        column.SortColumnId = (int)Column.Duration;
        treeView.AppendColumn(column);
    }

    class CellRendererDate : CellRendererText{
        
    }


    private ListStore CreateModel()
    {
        ListStore store = new ListStore(
                         typeof(string),
                         typeof(string),
                         typeof(string));
        foreach (Task task in dbManager.getEntries()){
            addValue(store, task);
        }

        return store;
    }
    private void addValue(ListStore store, Task task){
        store.AppendValues(task.Date.ToString("g"), task.Name, task.Duration.ToString("HH:mm:ss"));
    }

    private enum Column
    {
        Date,
        Name,
        Duration
    }


    public class Task
    {
        public DateTime Date;
        public string Name;
        public DateTime Duration;

        public Task(long ticksDate , string name, long ticksDuration)
        {
            Date = new DateTime(ticksDate);
            Name = name;
            Duration = new DateTime(ticksDuration);
        }
    }
}
