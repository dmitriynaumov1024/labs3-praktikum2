using System;

static class CsvConverters
{
    public static string ToCsvString(MonitorInfo item)
    {
        return String.Format("{0}\u0003{1}\u0003{2:F2}\u0003{3}\u0003{4}", 
            item.Brand,
            item.Model,
            item.Diagonal,
            item.Matrix,
            item.Price
        );
    }

    public static MonitorInfo CsvStringToMonitor(string line)
    {
        string[] splitLine = line.Split('\u0003');
        if (splitLine.Length < 5) return null;
        int price = 1;
        int.TryParse(splitLine[4], out price);
        float diagonal = 1;
        float.TryParse(splitLine[2], out diagonal);
        return new MonitorInfo {
            Brand = splitLine[0],
            Model = splitLine[1],
            Diagonal = diagonal,
            Matrix = splitLine[3],
            Price = price
        };
    }
}
