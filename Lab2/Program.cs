using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Threading;
using System.Globalization;

/*
    Input file format: [
        {
            "id": "Number",
            "brand": "String",
            "model": "String",
            "diagonal": "Number",
            "matrix": "String",
            "price": "Number"
        }
    ]
    
    Operations:
    Select most expensive monitors, then select those with smallest diagonal. 
*/
static class Program
{
    static List<MonitorInfo> monitorsList = new List<MonitorInfo> {
        new MonitorInfo {
            Brand = "Asus",
            Model = "HM507",
            Diagonal = 24.0F,
            Matrix = "IPS",
            Price = 6749
        },
        new MonitorInfo {
            Brand = "Asus",
            Model = "HH2550",
            Diagonal = 25.5F,
            Matrix = "IPS+",
            Price = 9599
        },
        new MonitorInfo {
            Brand = "LG",
            Model = "G190-3980au",
            Diagonal = 19.9F,
            Matrix = "IPS",
            Price = 5999
        },
        new MonitorInfo {
            Brand = "LG",
            Model = "G180-3600au",
            Diagonal = 18.8F,
            Matrix = "IPS",
            Price = 5659
        }
    };

    static Observable<List<MonitorInfo>> monitors = 
        new Observable<List<MonitorInfo>>(monitorsList);

    static TableOptions<MonitorInfo> tablemaker = new TableOptions<MonitorInfo> {
        Columns = new[] {
            new Column<MonitorInfo> {
                Header = "Brand",
                GetValue = (obj) => obj.Brand
            },
            new Column<MonitorInfo> {
                Header = "Model",
                GetValue = (obj) => obj.Model
            },
            new Column<MonitorInfo> {
                Header = "Diagonal",
                GetValue = (obj) => String.Format("{0:F2}''", obj.Diagonal),
                SortingCompare = (left, right) => left.Diagonal.CompareTo(right.Diagonal)
            },
            new Column<MonitorInfo> {
                Header = "Matrix",
                GetValue = (obj) => obj.Matrix
            },
            new Column<MonitorInfo> {
                Header = "Price",
                GetValue = (obj) => String.Format("{0}UAH", obj.Price),
                SortingCompare = (left, right) => left.Price.CompareTo(right.Price)
            }
        }
    };


    static List<MonitorInfo> CustomFilter (List<MonitorInfo> list, int limit)
    {
        return list.OrderByDescending(item => item.Price)
                   .ThenBy(item => item.Diagonal)
                   .Take(limit).ToList();
    }

    [STAThread]
    static void Main (string[] args)
    {
        //Application.EnableVisualStyles();
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en");
        Form view = new MainView(monitors, tablemaker) {
            SaveItems = () => Actions.SaveToFile(monitorsList),
            LoadItems = () => monitors.Use(list => Actions.LoadFromFile(list)),
            FilterItems = () => {
                var resultList = CustomFilter(monitorsList, limit: 3);
                new ResultView(resultList, tablemaker) {
                    Info = "Select most expensive monitors, then select those with smallest diagonal. Limit: 3.",
                    SaveItems = () => Actions.SaveToFile(resultList)
                }.ShowDialog();
            }
        };
        Application.Run(view);
    }
} 
