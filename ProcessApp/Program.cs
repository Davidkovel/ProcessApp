namespace ProcessApp;

using System;
using System.Diagnostics;
using System.Threading;
using System.Linq;

class ProcessMonitor
{
    public void RunMonitor()
    {
        Console.WriteLine("Process Monitor - Simple Version");
        Console.WriteLine("--------------------------------");

        int refreshInterval = GetRefreshInterval();
        MonitorProcesses(refreshInterval);
    }

    public int GetRefreshInterval()
    {
        Console.Write("Enter refresh interval in seconds (default 7): ");
        string input = Console.ReadLine();

        if (!int.TryParse(input, out int interval) || interval <= 0)
        {
            Console.WriteLine("Using default value: 7 seconds");
            return 7000;
        }

        return interval * 1000;
    }

    public void MonitorProcesses(int refreshInterval)
    {
        try
        {
            while (true)
            {
                Console.Clear();
                DisplayProcessList();
                Console.WriteLine($"\nRefreshing in {refreshInterval / 1000} seconds");
                Thread.Sleep(refreshInterval);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public void DisplayProcessList()
    {
        var processes = Process.GetProcesses()
            .OrderBy(p => p.ProcessName);

        Console.WriteLine($"{"Process Name"} {"ID"} {"Memory (MB)"}");
       // Console.WriteLine("-" * 20);
        
        Console.WriteLine("----------------------------------------");

        foreach (var process in processes)
        {

            Console.WriteLine(
                $"{process.ProcessName} {process.Id} {process.WorkingSet64 / (1024 * 1024)}");
        }

        Console.WriteLine($"\nTotal processes: {Process.GetProcesses().Length}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        var monitor = new ProcessMonitor();
        monitor.RunMonitor();
    }
}