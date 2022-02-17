using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

class MainView : Form
{
    // Data items are shared between view and main program
    private Observable<List<MonitorInfo>> dataItems;
    private TableOptions<MonitorInfo> tableMaker;

    // View elements
    private TableDataView<MonitorInfo> listView;
    private TableLayoutPanel newItemPanel;

    public Action SaveItems { get; set; }
    public Action LoadItems { get; set; }
    public Action FilterItems { get; set; } 

    public MainView(Observable<List<MonitorInfo>> dataSource, TableOptions<MonitorInfo> tableOptions) : base()
    {
        this.dataItems = dataSource;
        this.tableMaker = tableOptions;

        this.Text = "Lab 2";
        this.ClientSize = new Size(520, 440);

        this.listView = new TableDataView<MonitorInfo>(dataSource, tableOptions) {
            GridLines = true,
            MultiSelect = false,
            Dock = DockStyle.Fill,
            DefaultColumnWidth = 100,
            BorderStyle = BorderStyle.None
        };
        this.Controls.Add(this.listView);

        this.newItemPanel = new TableLayoutPanel {
            Dock = DockStyle.Bottom,
            Height = 204,
            ColumnCount = 3,
            Padding = new Padding(2)
        };
        
        TextBox inputForBrand = new TextBox(),
                inputForModel = new TextBox(),
                inputForDiagonal = new TextBox(),
                inputForMatrix = new TextBox(),
                inputForPrice = new TextBox();

        var addButton = new Button {
            Text = "Add to list",
            Dock = DockStyle.Fill
        };

        var errorLabel = new Label {
            Dock = DockStyle.Fill,
            Height = 40,
            TextAlign = ContentAlignment.MiddleLeft
        };

        addButton.Click += (sender, eventArgs) => {
            try {
                MonitorInfo newMonitorInfo = new MonitorInfo {
                    Brand = inputForBrand.Text,
                    Model = inputForModel.Text,
                    Diagonal = float.Parse(inputForDiagonal.Text),
                    Matrix = inputForMatrix.Text,
                    Price = int.Parse(inputForPrice.Text)
                };
                this.dataItems.Use(list => list.Add(newMonitorInfo));
                inputForBrand.Text = String.Empty;
                inputForModel.Text = String.Empty;
                inputForPrice.Text = String.Empty;
                inputForDiagonal.Text = String.Empty;
                inputForMatrix.Text = String.Empty;
                errorLabel.Text = String.Empty;
            }
            catch (ArgumentException ex) {
                errorLabel.Text = ex.Message;
            }
            catch (FormatException ex) {
                errorLabel.Text = ex.Message;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        };

        var panel = this.newItemPanel.Controls;

        Func<string, Label> makeLabel = (text) => new Label { 
            Text = text, 
            TextAlign = ContentAlignment.MiddleLeft 
        };

        panel.Add(makeLabel("Brand"), row: 0, column: 0);
        panel.Add(makeLabel("Model"), row: 1, column: 0);
        panel.Add(makeLabel("Diagonal, inch"), row: 2, column: 0);
        panel.Add(makeLabel("Matrix type"), row: 3, column: 0);
        panel.Add(makeLabel("Price, UAH"), row: 4, column: 0);

        panel.Add(inputForBrand, row: 0, column: 1);
        panel.Add(inputForModel, row: 1, column: 1);
        panel.Add(inputForDiagonal, row: 2, column: 1);
        panel.Add(inputForMatrix, row: 3, column: 1);
        panel.Add(inputForPrice, row: 4, column: 1);

        panel.Add(errorLabel, row: 5, column: 0);
        this.newItemPanel.SetColumnSpan(errorLabel, 2);
        panel.Add(addButton, row: 6, column: 0);
        this.newItemPanel.SetColumnSpan(addButton, 2);

        this.Controls.Add(this.newItemPanel);

        // Set up Menu strip
        var menuStrip = new MenuStrip {
            Dock = DockStyle.Top,
            AllowItemReorder = false,
        };
        var menuItem = new ToolStripMenuItem { Text = "Menu" };
        menuItem.DropDownItems.Add("Load from file...", null, (sender, eArgs) => {
            if (this.LoadItems != null) this.LoadItems();
        });
        menuItem.DropDownItems.Add("Save to file...", null, (sender, eArgs) => {
            if (this.SaveItems != null) this.SaveItems();
        });
        menuItem.DropDownItems.Add("Show filtered items", null, (sender, eArgs) => {
            if (this.FilterItems != null) this.FilterItems();
        });
        menuItem.DropDownItems.Add("Clear list", null, (sender, eArgs) => {
            this.dataItems.Use(list => list.Clear());
        });
        menuStrip.Items.Add(menuItem);
        this.Controls.Add(menuStrip);
    }
    
}
