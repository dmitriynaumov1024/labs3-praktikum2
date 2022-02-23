using System;
using System.IO;

class MultiStreamLogger
{
    protected TextWriter[] writers;

    public MultiStreamLogger (params TextWriter[] writers)
    {
        this.writers = writers;
    }

    public void Write (string format, params object[] args)
    {
        foreach (var writer in writers)
        {
            if (writer!=null) writer.Write(format, args);
        }
    }

    ~MultiStreamLogger ()
    {
        foreach (var writer in writers)
        {
            if (writer!=null) writer.Flush();
        }
    }
}
