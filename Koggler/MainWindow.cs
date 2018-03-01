using System;
using System.Timers;
using System.Diagnostics;
using System.Configuration;
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
        Build();
        InitComponents();
    }


    private void InitComponents(){
        InitButton();
        InitTreeView();
        InitTaskCombobox();
        InitTimeCombobox();
    }

    public void InitButton(){
        Clock = new Timer();
        Clock.Elapsed += new ElapsedEventHandler(Timer_Tick);
        stopwatch = new Stopwatch();
        Clock.Interval = 1000;
        Clock.AutoReset = true;
        changeElementColor(startButton, startColor);
    }

    public void InitTreeView()
    {
        store = CreateModel(1);
        treeview1.Model = store;
        treeview1.RulesHint = true;
        treeview1.SearchColumn = (int)Column.Duration;
        AddColumns(treeview1);
    }

    private void InitTaskCombobox(){
        ListStore taskStore = new ListStore(typeof(String));
        foreach (String s in dbManager.getTasks())
        {
            taskStore.AppendValues(s);
        }
        taskCombobox.Model = taskStore;
        Gtk.EntryCompletion com = new EntryCompletion();
        com.Model = taskStore;
        com.TextColumn = 0;
        taskCombobox.Entry.Completion = com;

    }

    private void InitTimeCombobox(){
        ListStore timeStore = new ListStore(typeof(String), typeof(int));
        timeStore.AppendValues("Last Day",1);
        timeStore.AppendValues("Last Week",7);
        timeStore.AppendValues("Last Month",30);
        timeStore.AppendValues("All",0);
        timeCombobox.Model = timeStore;
        TreeIter ti;
        timeCombobox.Model.GetIterFirst(out ti);
        timeCombobox.SetActiveIter(ti);
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
            store.AppendValues(t);
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
        column.SetCellDataFunc(rendererText, new Gtk.TreeCellDataFunc(RenderDate));
        treeView.AppendColumn(column);


        rendererText = new CellRendererText();
        column = new TreeViewColumn("Task", rendererText, "text", Column.Name);
        column.SortColumnId = (int)Column.Name;
        column.SetCellDataFunc(rendererText, new Gtk.TreeCellDataFunc(RenderText));
        treeView.AppendColumn(column);

        rendererText = new CellRendererText();
        column = new TreeViewColumn("Duration", rendererText, "text", Column.Duration);
        column.SortColumnId = (int)Column.Duration;
        column.SetCellDataFunc(rendererText, new Gtk.TreeCellDataFunc(RenderDurration));
        treeView.AppendColumn(column);
    }

    private void RenderDate(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
    {
        Task task = (Task)model.GetValue(iter, 0);
        (cell as Gtk.CellRendererText).Text = task.Date.ToString("g");
    }

    private void RenderText(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
    {
        Task task = (Task)model.GetValue(iter, 0);
        (cell as Gtk.CellRendererText).Text = task.Name;
    }

    private void RenderDurration(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
    {
        Task task = (Task)model.GetValue(iter, 0);
        (cell as Gtk.CellRendererText).Text = task.Duration.ToString("HH:mm:ss");
    }

    private ListStore CreateModel(int days)
    {
        ListStore store = new ListStore(typeof(Task));
        store.SetSortFunc(0, (model, a, b) => {
            Task ta = (Task)model.GetValue(a, 0);
            Task tb = (Task)model.GetValue(b, 0);
            return ta.Date.CompareTo(tb.Date);
        });
        foreach (Task task in dbManager.getEntries(days)){
            store.AppendValues(task);
        }
        store.SetSortColumnId(0,SortType.Descending);
        return store;
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

    protected void OnTimeComboboxChanged(object sender, EventArgs e)
    {
        if(sender == timeCombobox){
            
        }
    }
}
