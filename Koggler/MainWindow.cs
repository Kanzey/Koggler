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
        treeview1.SearchColumn = (int)Column.Time;
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
            Clock.Stop();
            stopwatch.Reset();
            store.AppendValues(taskCombobox.ActiveText, time.Text);
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

    private void FixedToggled(object o, ToggledArgs args)
    {
        Gtk.TreeIter iter;
        if (store.GetIterFromString(out iter, args.Path))
        {
            bool val = (bool)store.GetValue(iter, 0);
            store.SetValue(iter, 0, !val);
        }
    }

    private void AddColumns(TreeView treeView)
    {
        TreeViewColumn column;
        CellRendererText rendererText;

        // column for severities
        rendererText = new CellRendererText();
        column = new TreeViewColumn("Task", rendererText, "text", Column.Name);
        column.SortColumnId = (int)Column.Name;
        treeView.AppendColumn(column);

        // column for description
        rendererText = new CellRendererText();
        column = new TreeViewColumn("Duration", rendererText, "text", Column.Time);
        column.SortColumnId = (int)Column.Time;
        treeView.AppendColumn(column);
    }

    protected override bool OnDeleteEvent(Gdk.Event evt)
    {
        Destroy();
        return true;
    }

    private ListStore CreateModel()
    {
        ListStore store = new ListStore(
                         typeof(string),
                         typeof(string));

        foreach (Task task in tasks)
        {
            store.AppendValues(task.Name,
                        task.Time);
        }

        return store;
    }

    private enum Column
    {
        Name,
        Time,
    }

    private static Task[] tasks =
    {
        new Task ( "scrollable notebooks and hidden tabs", "00:11:11"),
        new Task ( "gdk_window_clear_area (gdkwindow-win32.c) is not thread-safe", "10:10:00" )
        };

    public class Task
    {
        public string Name;
        public string Time;

        public Task(string name, string time)
        {
            Name = name;
            Time = time;
        }
    }
}
