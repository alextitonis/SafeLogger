using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

public class main
{
    static void Main()
    {
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

        stopwatch.Reset();
        stopwatch.Start();

        WebClient webClient = new WebClient();
        byte[] bytes = webClient.DownloadData("http://www.codeproject.com");

        stopwatch.Stop();

        double seconds = stopwatch.Elapsed.TotalSeconds;

        double speed = bytes.Length / seconds;

        Console.WriteLine("Your speed: {0} bytes per second.", speed);
        Console.ReadLine();
    }
}

