using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

class ResultView : Form
{
    public Action SaveItems { get; set; }

    private Label _infoLabel;
    private string _infoString;

    public string Info {
        get {
            return this._infoString;
        }
        set {
            this._infoString = value;
            this._infoLabel.Text = value;
            this._infoLabel.Visible = (value != null && value.Length > 0);
        }
    }

    public ResultView(List<MonitorInfo> data, TableOptions<MonitorInfo> tablemaker) : base()
    {
        this.Text = "Result";
        this.ClientSize = new Size(520, 200);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MinimizeBox = false;
        this.MaximizeBox = false;
        var listView = new TableDataView<MonitorInfo>(data, tablemaker) {
            GridLines = true,
            MultiSelect = false,
            Dock = DockStyle.Fill,
            DefaultColumnWidth = 100,
            BorderStyle = BorderStyle.None
        };
        this.Controls.Add(listView);

        // Set up info label
        this._infoLabel = new Label { 
            AutoSize = true,
            Dock = DockStyle.Bottom,
            Visible = false,
            Padding = new Padding(4)
        };
        this.Controls.Add(this._infoLabel);

        // Set up Menu strip
        var menuStrip = new MenuStrip {
            Dock = DockStyle.Bottom,
            AllowItemReorder = false
        };
        var menuItem = new ToolStripMenuItem { Text = "Menu" };
        menuItem.DropDownItems.Add("Save to file...", null, (sender, eArgs) => {
            if (this.SaveItems != null) this.SaveItems();
        });
        menuItem.DropDownItems.Add("Show / hide info", null, (sender, eArgs) => {
            this._infoLabel.Visible = !this._infoLabel.Visible;
        });
        menuStrip.Items.Add(menuItem);
        this.Controls.Add(menuStrip);
    }
}
