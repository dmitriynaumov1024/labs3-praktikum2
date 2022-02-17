using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

static class Actions
{
    public static void SaveToFile (List<MonitorInfo> items)
    {
        var dialog = new SaveFileDialog {
            RestoreDirectory = true,
            Title = "Save as"
        };
        if (dialog.ShowDialog() == DialogResult.OK) {
            StreamWriter writer = new StreamWriter(dialog.OpenFile());
            foreach (MonitorInfo item in items) {
                writer.Write(CsvConverters.ToCsvString(item));
                writer.Write("\r\n");
            } 
            writer.Flush();
            writer.Dispose();
        }    
    }

    public static void LoadFromFile (List<MonitorInfo> items)
    {
        var dialog = new OpenFileDialog {
            RestoreDirectory = true,
            Title = "Select a file"
        };
        if (dialog.ShowDialog() == DialogResult.OK) {
            StreamReader reader = new StreamReader(dialog.OpenFile());
            items.Clear();
            do {
                string line = reader.ReadLine();
                if (line.StartsWith("#")) continue;
                var currentItem = CsvConverters.CsvStringToMonitor(line);
                if (currentItem != null) items.Add(currentItem);
            } while (!reader.EndOfStream);
            reader.Dispose();
        }
    }
    
}
