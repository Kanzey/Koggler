
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow
{
	private global::Gtk.VPaned vpaned1;

	private global::Gtk.HPaned hpaned1;

	private global::Gtk.VPaned vpaned4;

	private global::Gtk.HPaned hpaned4;

	private global::Gtk.ComboBoxEntry taskCombobox;

	private global::Gtk.Label time;

	private global::Gtk.HPaned hpaned2;

	private global::Gtk.ComboBox timeCombobox;

	private global::Gtk.CheckButton timeCheckbutton;

	private global::Gtk.Alignment alignment2;

	private global::Gtk.Button startButton;

	private global::Gtk.ScrolledWindow GtkScrolledWindow;

	private global::Gtk.TreeView treeview1;

	protected virtual void Build()
	{
		global::Stetic.Gui.Initialize(this);
		// Widget MainWindow
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString("MainWindow");
		this.WindowPosition = ((global::Gtk.WindowPosition)(4));
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.vpaned1 = new global::Gtk.VPaned();
		this.vpaned1.CanFocus = true;
		this.vpaned1.Position = 88;
		// Container child vpaned1.Gtk.Paned+PanedChild
		this.hpaned1 = new global::Gtk.HPaned();
		this.hpaned1.CanFocus = true;
		this.hpaned1.Name = "hpaned1";
		this.hpaned1.Position = 522;
		// Container child hpaned1.Gtk.Paned+PanedChild
		this.vpaned4 = new global::Gtk.VPaned();
		this.vpaned4.CanFocus = true;
		this.vpaned4.Name = "vpaned4";
		this.vpaned4.Position = 36;
		// Container child vpaned4.Gtk.Paned+PanedChild
		this.hpaned4 = new global::Gtk.HPaned();
		this.hpaned4.CanFocus = true;
		this.hpaned4.Name = "hpaned4";
		this.hpaned4.Position = 412;
		// Container child hpaned4.Gtk.Paned+PanedChild
		this.taskCombobox = global::Gtk.ComboBoxEntry.NewText();
		this.taskCombobox.Name = "taskCombobox";
		this.hpaned4.Add(this.taskCombobox);
		global::Gtk.Paned.PanedChild w1 = ((global::Gtk.Paned.PanedChild)(this.hpaned4[this.taskCombobox]));
		w1.Resize = false;
		// Container child hpaned4.Gtk.Paned+PanedChild
		this.time = new global::Gtk.Label();
		this.time.Name = "time";
		this.time.LabelProp = global::Mono.Unix.Catalog.GetString("00:00:00");
		this.hpaned4.Add(this.time);
		this.vpaned4.Add(this.hpaned4);
		global::Gtk.Paned.PanedChild w3 = ((global::Gtk.Paned.PanedChild)(this.vpaned4[this.hpaned4]));
		w3.Resize = false;
		// Container child vpaned4.Gtk.Paned+PanedChild
		this.hpaned2 = new global::Gtk.HPaned();
		this.hpaned2.CanFocus = true;
		this.hpaned2.Name = "hpaned2";
		this.hpaned2.Position = 422;
		// Container child hpaned2.Gtk.Paned+PanedChild
		this.timeCombobox = global::Gtk.ComboBox.NewText();
		this.timeCombobox.Name = "timeCombobox";
		this.hpaned2.Add(this.timeCombobox);
		global::Gtk.Paned.PanedChild w4 = ((global::Gtk.Paned.PanedChild)(this.hpaned2[this.timeCombobox]));
		w4.Resize = false;
		// Container child hpaned2.Gtk.Paned+PanedChild
		this.timeCheckbutton = new global::Gtk.CheckButton();
		this.timeCheckbutton.CanFocus = true;
		this.timeCheckbutton.Name = "timeCheckbutton";
		this.timeCheckbutton.Label = global::Mono.Unix.Catalog.GetString("Total time");
		this.timeCheckbutton.DrawIndicator = true;
		this.timeCheckbutton.UseUnderline = true;
		this.hpaned2.Add(this.timeCheckbutton);
		this.vpaned4.Add(this.hpaned2);
		this.hpaned1.Add(this.vpaned4);
		global::Gtk.Paned.PanedChild w7 = ((global::Gtk.Paned.PanedChild)(this.hpaned1[this.vpaned4]));
		w7.Resize = false;
		// Container child hpaned1.Gtk.Paned+PanedChild
		this.alignment2 = new global::Gtk.Alignment(0.5F, 0.5F, 1F, 1F);
		this.alignment2.Name = "alignment2";
		this.alignment2.LeftPadding = ((uint)(5));
		this.alignment2.RightPadding = ((uint)(5));
		// Container child alignment2.Gtk.Container+ContainerChild
		this.startButton = new global::Gtk.Button();
		this.startButton.CanFocus = true;
		this.startButton.Name = "startButton";
		this.startButton.UseUnderline = true;
		this.startButton.Label = global::Mono.Unix.Catalog.GetString("Start");
		this.alignment2.Add(this.startButton);
		this.hpaned1.Add(this.alignment2);
		this.vpaned1.Add(this.hpaned1);
		global::Gtk.Paned.PanedChild w10 = ((global::Gtk.Paned.PanedChild)(this.vpaned1[this.hpaned1]));
		w10.Resize = false;
		// Container child vpaned1.Gtk.Paned+PanedChild
		this.GtkScrolledWindow = new global::Gtk.ScrolledWindow();
		this.GtkScrolledWindow.Name = "GtkScrolledWindow";
		this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
		this.treeview1 = new global::Gtk.TreeView();
		this.treeview1.CanFocus = true;
		this.treeview1.Name = "treeview1";
		this.GtkScrolledWindow.Add(this.treeview1);
		this.vpaned1.Add(this.GtkScrolledWindow);
		this.Add(this.vpaned1);
		if ((this.Child != null))
		{
			this.Child.ShowAll();
		}
		this.DefaultWidth = 640;
		this.DefaultHeight = 300;
		this.Show();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler(this.OnDeleteEvent);
		this.timeCombobox.Changed += new global::System.EventHandler(this.OnTimeComboboxChanged);
		this.timeCheckbutton.Toggled += new global::System.EventHandler(this.OnTimeCheckbuttonToggled);
		this.startButton.Clicked += new global::System.EventHandler(this.OnStartButtonClicked);
	}
}
