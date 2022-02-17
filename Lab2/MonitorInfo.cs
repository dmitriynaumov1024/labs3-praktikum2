using System;

class MonitorInfo
{
    private string brand;
    private string model;
    private string matrix;
    private float diagonal;
    private int price;

    public string Brand { 
        get { return this.brand; } 
        set { this.brand = value; }
    }

    public string Model {
        get { return this.model; } 
        set { this.model = value; }
    }

    public string Matrix { 
        get { return this.matrix; }
        set { this.matrix = value; }
    }

    public float Diagonal { 
        get { 
            return this.diagonal; 
        }
        set { 
            Guard("Diagonal", value, 1F, 1000F);
            this.diagonal = value;
        }
    }

    public int Price {
        get {
            return this.price;
        }
        set {
            Guard("Price", value, 1, 1000000);
            this.price = value;
        }
    }

    static void Guard<T>(string name, T value, T lowerBound, T upperBound) 
        where T: IComparable<T>
    {
        if (value.CompareTo(lowerBound) < 0 || value.CompareTo(upperBound) > 0) 
            throw new ArgumentException (String.Format("{0} is {1}, but must be in {2}..{3} range.", name, value, lowerBound, upperBound));
    }
}
