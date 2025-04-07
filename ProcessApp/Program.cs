using System;
using System.Diagnostics;
using System.Linq;

class ProcessMonitor
{
    public void RunMonitor()
    {
        Console.WriteLine("Process Monitor with Details");
        Console.WriteLine("---------------------------");

        while (true)
        {
            Console.Clear();
            DisplayProcessList();
            ShowMenu();

            var choice = Console.ReadLine();
            HandleUserChoice(choice);
        }
    }

    private void ShowMenu()
    {
        Console.WriteLine("\nOptions:");
        Console.WriteLine("1. Enter process ID to view details");
        Console.WriteLine("2. Terminate a process by ID");
        Console.WriteLine("3. Exit");
        Console.Write("Your choice: ");
    }

    private void HandleUserChoice(string choice)
    {
        switch (choice)
        {
            case "1":
                Console.Write("\nEnter Process ID: ");
                if (int.TryParse(Console.ReadLine(), out int pid))
                {
                    var process = Process.GetProcesses().FirstOrDefault(p => p.Id == pid);
                    ShowProcessDetails(process);
                }
                else
                {
                    Console.WriteLine("Invalid Process ID!");
                    Console.ReadKey();
                }

                break;

            case "2":
                Console.Write("\nEnter Process ID to terminate: ");
                if (int.TryParse(Console.ReadLine(), out int pidToKill))
                {
                    TerminateProcess(pidToKill);
                }
                else
                {
                    Console.WriteLine("Invalid Process ID!");
                    Console.ReadKey();
                }

                break;

            case "3":
                Environment.Exit(0);
                break;

            default:
                Console.WriteLine("Invalid choice! Press any key to continue...");
                Console.ReadKey();
                break;
        }
    }

    private void TerminateProcess(int processId)
    {
        try
        {
            Process process = Process.GetProcessById(processId);
            

            process.Kill();
            Console.WriteLine($"Process {process.ProcessName} terminated successfully.");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error terminating process: {ex.Message}");
            Console.ReadKey();
        }
    }

    private void DisplayProcessList()
    {
        var processes = Process.GetProcesses()
            .OrderBy(p => p.ProcessName)
            .ToArray();

        Console.WriteLine($"Process Name || PID || Memory (MB) || Instances");
        Console.WriteLine(new string('-', 60));

        foreach (var process in processes)
        {
            int instanceCount = Process.GetProcessesByName(process.ProcessName).Length;
            Console.WriteLine(
                $"{process.ProcessName} {process.Id} {process.WorkingSet64 / (1024 * 1024)} {instanceCount}");
        }

        Console.WriteLine($"\nTotal processes: {processes.Length}");
    }


    private void ShowProcessDetails(Process process)
    {
        if (process == null)
        {
            Console.WriteLine("Process not found!");
            Console.ReadKey();
            return;
        }

        try
        {
            Console.Clear();
            Console.WriteLine($"Process Details - {process.ProcessName} (PID: {process.Id})");
            Console.WriteLine("----------------------------------------");

            Console.WriteLine($"Start Time:        {process.StartTime:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"Total CPU Time:    {process.TotalProcessorTime}");
            Console.WriteLine($"Thread Count:      {process.Threads.Count}");

            var instances = Process.GetProcessesByName(process.ProcessName);
            Console.WriteLine($"Instance Count:    {instances.Length}");

            Console.WriteLine($"Memory Usage:      {process.WorkingSet64 / (1024 * 1024)} MB");
            Console.WriteLine($"Priority:          {process.BasePriority}");
            Console.WriteLine($"Responding:        {process.Responding}");

            Console.WriteLine("\nPress any key to return to process list...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Cannot show details: {ex.Message}");
            Console.ReadKey();
        }
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